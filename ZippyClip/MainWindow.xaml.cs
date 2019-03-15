namespace ZippyClip
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;
    using Items;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

            ClipboardNotification.ClipboardUpdate += ClipboardNotification_ClipboardUpdate;
        }

        private void ClipboardNotification_ClipboardUpdate(object sender, EventArgs e)
        {
            PutClipboardContentsInList();
        }

        private void PutClipboardContentsInList()
        {
            if (Item.MakeFromClipboard() is Item item)
            {
                ClipboardHistory.Add(item);
            }
        }

        public ObservableCollection<Item> ClipboardHistory { get; } = new ObservableCollection<Item>(new List<Item>());

        public Item SelectedItem { get; set; }

        private void ListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (SelectedItem == null)
                return;

            SelectedItem.CopyToClipboard();
        }

        private void ButtonUri_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
