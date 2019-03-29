#nullable enable

namespace ZippyClip.Actions
{
    using System.Diagnostics;
    using ZippyClip.Items;

    class AlternativeActionPerformer : IActionPerformer
    {
        public void ActivateHyperlink(UriItem item)
        {
            Windows.Infrastructure.Navigate(item.Uri);
        }

        public void ActivateText(TextItem item)
        {
        }

        public void ActivateImage(ImageItem item)
        {
        }
    }
}
