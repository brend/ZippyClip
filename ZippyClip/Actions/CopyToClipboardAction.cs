using ZippyClip.Items;

namespace ZippyClip.Actions
{
    public class CopyToClipboardAction : IActionPerformer
    {
        public void ActivateHyperlink(UriItem item)
        {
            CopyToClipboard(item);
        }

        public void ActivateImage(ImageItem item)
        {
            CopyToClipboard(item);
        }

        public void ActivateText(TextItem item)
        {
            CopyToClipboard(item);
        }

        private void CopyToClipboard(Item item)
        {
            item.CopyToClipboard();
        }
    }
}
