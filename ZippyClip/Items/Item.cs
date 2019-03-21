﻿#nullable enable

namespace ZippyClip.Items
{
    using System;
    using System.Windows;
    using System.Windows.Media.Imaging;

    public abstract class Item: IEquatable<Item>
    {
        public static Item? MakeFromClipboard()
        {
            if (Clipboard.ContainsText())
            {
                return Make(Clipboard.GetText());
            }

            if (Clipboard.ContainsImage())
            {
                return Make(Clipboard.GetImage());
            }

            return null;
        }

        private static Item Make(string text)
        {
            if (Uri.TryCreate(text, UriKind.Absolute, out Uri uri))
            {
                return new UriItem(uri);
            }
            else
            {
                return new TextItem(text);
            }
        }

        private static Item Make(BitmapSource bitmapSource)
        {
            return new ImageItem(bitmapSource);
        }

        protected abstract void CopyContentsToClipboard(IDataObject data);

        public void CopyToClipboard()
        {
            var data = new DataObject();

            data.SetData(ClipboardNotification.ClipboardIgnoreFormat, 0);
            
            CopyContentsToClipboard(data);

            Clipboard.SetDataObject(data);
        }

        public override abstract int GetHashCode();

        public abstract bool Equals(Item other);
    }
}
