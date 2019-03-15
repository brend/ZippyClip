namespace ZippyClip.Items
{
    using System.Windows;
    using System.Windows.Media.Imaging;

    class ImageItem : Item
    {
        public ImageItem(BitmapSource image)
        {
            this.Image = image ?? throw new System.ArgumentNullException(nameof(image));
        }
        
        public BitmapSource Image { get; }

        protected override void CopyContentsToClipboard()
        {
            Clipboard.SetImage(Image);
        }

        public override string ToString()
        {
            return "<Image data>";
        }
    }
}
