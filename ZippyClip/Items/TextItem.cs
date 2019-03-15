namespace ZippyClip.Items
{
    using System.Windows;

    class TextItem : Item
    {
        public TextItem(string text)
        {
            Text = text ?? throw new System.ArgumentNullException(nameof(text));
        }

        public string Text { get; set; }

        protected override void CopyContentsToClipboard()
        {
            Clipboard.SetText(Text);
        }

        public override string ToString()
        {
            return Text ?? "";
        }
    }
}
