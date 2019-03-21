#nullable enable

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

        protected override void CopyContentsToClipboard(IDataObject data)
        {
            data.SetData(DataFormats.Text, Uri.ToString());
        }

        public override int GetHashCode()
        {
            return Uri.GetHashCode();
        }

        public override bool Equals(Item other)
        {
            return other is UriItem u && Uri.Equals(u.Uri);
        }
    }
}
