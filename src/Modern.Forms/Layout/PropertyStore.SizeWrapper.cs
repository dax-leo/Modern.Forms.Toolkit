﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Drawing;

namespace Modern.Forms.Layout;

internal partial class PropertyStore
{
    private sealed class SizeWrapper
    {
        public Size Size;

        public SizeWrapper (Size size) => Size = size;
    }
}
