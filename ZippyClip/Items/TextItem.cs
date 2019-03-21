#nullable enable

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

        protected override void CopyContentsToClipboard(IDataObject data)
        {
            data.SetData(DataFormats.Text, Text);
        }

        public override string ToString()
        {
            return Text ?? "";
        }

        public override int GetHashCode()
        {
            return Text.GetHashCode();
        }

        public override bool Equals(Item other)
        {
            return other is TextItem t && Text.Equals(t.Text);
        }
    }
}
