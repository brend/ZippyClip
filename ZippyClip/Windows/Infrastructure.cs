#nullable enable

namespace ZippyClip.Windows
{
    using System;
    using System.Diagnostics;
    using System.Windows;

    public static class Infrastructure
    {
        public static void Navigate(Uri uri)
        {
            Process.Start(new ProcessStartInfo(uri.AbsoluteUri));
        }

        internal static void QuitApplication()
        {
            Application.Current.Shutdown();
        }
    }
}
