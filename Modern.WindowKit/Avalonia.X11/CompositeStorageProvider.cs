﻿#nullable enable
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Modern.WindowKit.Platform.Storage;

namespace Modern.WindowKit.X11.NativeDialogs;

internal class CompositeStorageProvider : IStorageProvider
{
    private readonly IEnumerable<Func<Task<IStorageProvider?>>> _factories;
    public CompositeStorageProvider(IEnumerable<Func<Task<IStorageProvider?>>> factories)
    {
        _factories = factories;
    }

    public bool CanOpen => true;
    public bool CanSave => true;
    public bool CanPickFolder => true;

    private async Task<IStorageProvider> EnsureStorageProvider()
    {
        foreach (var factory in _factories)
        {
            var provider = await factory();
            if (provider is not null)
            {
                return provider;
            }
        }

        throw new InvalidOperationException("Neither DBus nor GTK are available on the system");
    }

    public async Task<IReadOnlyList<IStorageFile>> OpenFilePickerAsync(FilePickerOpenOptions options)
    {
        var provider = await EnsureStorageProvider().ConfigureAwait(false);
        return await provider.OpenFilePickerAsync(options).ConfigureAwait(false);
    }

    public async Task<IStorageFile?> SaveFilePickerAsync(FilePickerSaveOptions options)
    {
        var provider = await EnsureStorageProvider().ConfigureAwait(false);
        return await provider.SaveFilePickerAsync(options).ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<IStorageFolder>> OpenFolderPickerAsync(FolderPickerOpenOptions options)
    {
        var provider = await EnsureStorageProvider().ConfigureAwait(false);
        return await provider.OpenFolderPickerAsync(options).ConfigureAwait(false);
    }

    public async Task<IStorageBookmarkFile?> OpenFileBookmarkAsync(string bookmark)
    {
        var provider = await EnsureStorageProvider().ConfigureAwait(false);
        return await provider.OpenFileBookmarkAsync(bookmark).ConfigureAwait(false);
    }

    public async Task<IStorageBookmarkFolder?> OpenFolderBookmarkAsync(string bookmark)
    {
        var provider = await EnsureStorageProvider().ConfigureAwait(false);
        return await provider.OpenFolderBookmarkAsync(bookmark).ConfigureAwait(false);
    }
}
