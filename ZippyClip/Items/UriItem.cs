namespace ZippyClip.Items
{
    using System;
    using System.Windows;

    class UriItem: Item
    {
        public UriItem(Uri uri)
        {
            Uri = uri ?? throw new ArgumentNullException(nameof(uri));
        }

        public Uri Uri { get; }

        protected override void CopyContentsToClipboard()
        {
            Clipboard.SetText(Uri.ToString());
        }
    }
}
