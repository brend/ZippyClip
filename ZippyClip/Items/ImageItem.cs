#nullable enable

namespace ZippyClip.Items
{
    using System.Windows;
    using System.Windows.Media.Imaging;
    using ZippyClip.Actions;

    public class ImageItem : Item
    {
        public ImageItem(BitmapSource image)
        {
            this.Image = image ?? throw new System.ArgumentNullException(nameof(image));
        }
        
        public BitmapSource Image { get; }

        protected override void CopyContentsToClipboard(IDataObject data)
        {
            data.SetData(DataFormats.Bitmap, Image);
        }

        public override string ToString()
        {
            return "<Image data>";
        }

        public override int GetHashCode()
        {
            return Image.GetHashCode();
        }

        public override bool Equals(Item other)
        {
            return other is ImageItem i && Image.Equals(i.Image);
        }

        public override BitmapSource? GetPreviewImage() => Image;

        public override void PerformAction(IActionPerformer actionPerformer) => actionPerformer.ActivateImage(this);

        public override bool SupportsPreview => true;
    }
}
