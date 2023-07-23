﻿#if !MONO
#define UseFastResourceLock
#endif
using System;
using System.Runtime.InteropServices;

namespace GMap.NET.Internals
{
#if !MONO
#endif

    /// <summary>
    ///     custom ReaderWriterLock
    ///     in Vista and later uses integrated Slim Reader/Writer (SRW) Lock
    ///     http://msdn.microsoft.com/en-us/library/aa904937(VS.85).aspx
    ///     http://msdn.microsoft.com/en-us/magazine/cc163405.aspx#S2
    /// </summary>
    public sealed class FastReaderWriterLock : IDisposable
    {
#if !MONO
        private static class NativeMethods
        {
            // Methods
            [DllImport("Kernel32", ExactSpelling = true)]
            internal static extern void AcquireSRWLockExclusive(ref IntPtr srw);

            [DllImport("Kernel32", ExactSpelling = true)]
            internal static extern void AcquireSRWLockShared(ref IntPtr srw);

            [DllImport("Kernel32", ExactSpelling = true)]
            internal static extern void InitializeSRWLock(out IntPtr srw);

            [DllImport("Kernel32", ExactSpelling = true)]
            internal static extern void ReleaseSRWLockExclusive(ref IntPtr srw);

            [DllImport("Kernel32", ExactSpelling = true)]
            internal static extern void ReleaseSRWLockShared(ref IntPtr srw);
        }

        IntPtr _lockSRW = IntPtr.Zero;

        public FastReaderWriterLock()
        {
            if (UseNativeSRWLock)
            {
                NativeMethods.InitializeSRWLock(out _lockSRW);
            }
            else
            {
#if UseFastResourceLock
                _pLock = new FastResourceLock();
#endif
            }
        }

#if UseFastResourceLock
        ~FastReaderWriterLock()
        {
            Dispose(false);
        }

        void Dispose(bool disposing)
        {
            if (_pLock != null)
            {
                _pLock.Dispose();
                _pLock = null;
            }
        }

        FastResourceLock _pLock;
#endif

        static readonly bool
            UseNativeSRWLock =
                Stuff.IsRunningOnVistaOrLater() &&
                IntPtr.Size == 4; // works only in 32-bit mode, any ideas on native 64-bit support? 

#endif

#if !UseFastResourceLock
       Int32 busy = 0;
       Int32 readCount = 0;
#endif

        public void AcquireReaderLock()
        {
#if !MONO
            if (UseNativeSRWLock)
            {
                NativeMethods.AcquireSRWLockShared(ref _lockSRW);
            }
            else
#endif
            {
#if UseFastResourceLock
                _pLock.AcquireShared();
#else
            Thread.BeginCriticalRegion();

            while(Interlocked.CompareExchange(ref busy, 1, 0) != 0)
            {
               Thread.Sleep(1);
            }

            Interlocked.Increment(ref readCount);

            // somehow this fix deadlock on heavy reads
            Thread.Sleep(0);
            Thread.Sleep(0);
            Thread.Sleep(0);
            Thread.Sleep(0);
            Thread.Sleep(0);
            Thread.Sleep(0);
            Thread.Sleep(0);

            Interlocked.Exchange(ref busy, 0);
#endif
            }
        }

        public void ReleaseReaderLock()
        {
#if !MONO
            if (UseNativeSRWLock)
            {
                NativeMethods.ReleaseSRWLockShared(ref _lockSRW);
            }
            else
#endif
            {
#if UseFastResourceLock
                _pLock.ReleaseShared();
#else
            Interlocked.Decrement(ref readCount);
            Thread.EndCriticalRegion();
#endif
            }
        }

        public void AcquireWriterLock()
        {
            try
            {
#if !MONO
                if (UseNativeSRWLock)
                {
                    NativeMethods.AcquireSRWLockExclusive(ref _lockSRW);
                }
                else
#endif
                {
#if UseFastResourceLock
                    _pLock.AcquireExclusive();
#else
                    Thread.BeginCriticalRegion();

                    while(Interlocked.CompareExchange(ref busy, 1, 0) != 0)
                    {
                       Thread.Sleep(1);
                    }

                    while(Interlocked.CompareExchange(ref readCount, 0, 0) != 0)
                    {
                       Thread.Sleep(1);
                    }
#endif
                }
            }
            catch (Exception ex) { }
        }

        public void ReleaseWriterLock()
        {
            try
            {
#if !MONO
                if (UseNativeSRWLock)
                {
                    NativeMethods.ReleaseSRWLockExclusive(ref _lockSRW);
                }
                else
#endif
                {
#if UseFastResourceLock
                    _pLock.ReleaseExclusive();
#else
                    Interlocked.Exchange(ref busy, 0);
                    Thread.EndCriticalRegion();
#endif
                }
            }
            catch (Exception ex) { }
        }

        #region IDisposable Members

        public void Dispose()
        {
#if UseFastResourceLock
            Dispose(true);
            GC.SuppressFinalize(this);
#endif
        }

        #endregion
    }
}
