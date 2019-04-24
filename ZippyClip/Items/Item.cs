#nullable enable

namespace ZippyClip.Items
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media.Imaging;
    using ZippyClip.Actions;

    public abstract class Item: IEquatable<Item>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int? listIndex;

        public int? ListIndex
        {
            get => listIndex;
            set
            {
                listIndex = value;
                OnPropertyChanged("ListIndex");
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static Item? MakeFromClipboard()
        {
            if (Clipboard.ContainsText())
            {
                return Make(Clipboard.GetText());
            }

            if (Clipboard.ContainsImage())
            {
                try
                {
                    return Make(Clipboard.GetImage());
                }
                catch (System.Runtime.InteropServices.COMException e)
                {
                    Console.Error.WriteLine(e.Message); // TODO: Log

                    return null;
                }
            }

            if (Clipboard.ContainsFileDropList())
            {
                var files = Clipboard.GetFileDropList().Cast<string>().ToArray();
                var filesText = string.Join(Environment.NewLine, files);
                var tryToParseUri = files.Length == 1;

                return Make(filesText, tryToParseUri);
            }

            return null;
        }

        private static Item Make(string text, bool tryToParseUri = true)
        {
            if (tryToParseUri && Uri.TryCreate(text, UriKind.Absolute, out Uri uri))
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

        public virtual BitmapSource? GetPreviewImage() => null;

        public virtual string? GetPreviewText() => null;

        public virtual bool SupportsPreview => false;

        public abstract void PerformAction(IActionPerformer actionPerformer);
    }
}
