#nullable enable

namespace ZippyClip.Actions
{
    using ZippyClip.Items;

    public interface IActionPerformer
    {
        void ActivateHyperlink(UriItem item);
        void ActivateImage(ImageItem item);
        void ActivateText(TextItem item);
    }
}