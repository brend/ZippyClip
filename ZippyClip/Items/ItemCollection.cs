#nullable enable

namespace ZippyClip.Items
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;

    class ClipboardItemCollection
    {
        public ObservableCollection<Item> Items { get; } = 
            new ObservableCollection<Item>(new List<Item>());

        private HashSet<Item> HashedItems { get; } = new HashSet<Item>();

        public void Push(Item item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (Contains(item))
                return;

            Items.Insert(0, item);
        }

        public bool Contains(Item item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            var hashCode = item.GetHashCode();

            return Items.Any(k => k.GetHashCode() == hashCode);
        }

        public void PushClipboardContents()
        {
            if (ClipboardContentsHasBeenGeneratedByThisApp())
                return;

            if (Item.MakeFromClipboard() is Item item)
            {
                Push(item);
            }
        }

        private bool ClipboardContentsHasBeenGeneratedByThisApp()
        {
            return Clipboard.ContainsData(ClipboardNotification.ClipboardIgnoreFormat);
        }
    }
}
