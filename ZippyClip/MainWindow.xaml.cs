#nullable enable

namespace ZippyClip
{
    using Items;
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using ZippyClip.Hotkeys;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;
        }

        private void ClipboardNotification_ClipboardUpdate(object sender, EventArgs e)
        {
            PutClipboardContentsInList();
        }

        private void PutClipboardContentsInList()
        {
            ClipboardItems.PushClipboardContents();
        }

        private ClipboardItemCollection ClipboardItems = new ClipboardItemCollection();

        public ObservableCollection<Item> ClipboardHistory =>
            ClipboardItems.Items;

        public Item? SelectedItem { get; set; }

        private void ListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            CopySelectedItemToClipboard();
        }

        private void CopySelectedItemToClipboard()
        {
            if (SelectedItem == null)
                return;

            SelectedItem.CopyToClipboard();
        }

        private void CopyItemToClipboard(int index)
        {
            if (index < 0 || index >= ClipboardHistory.Count)
                return;

            var item = ClipboardHistory[index];

            item.CopyToClipboard();
        }

        private void ButtonUri_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ClipboardNotification.ClipboardUpdate += ClipboardNotification_ClipboardUpdate;

            RegisterHotkeys();            
        }

        private void RegisterHotkeys()
        {
            foreach (int i in Enumerable.Range(0, 3))
            {
                var hk = new HotkeyHandler(i, KeyModifiers.Shift, System.Windows.Forms.Keys.D1 + i);

                hk.Pressed += delegate 
                {
                    System.Threading.Thread.Sleep(10);
                    CopyItemToClipboard(i);
                    System.Windows.Forms.SendKeys.SendWait("^v");
                };
            }
        }

        private void ButtonCopyItem_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            if (button.DataContext is Item item)
            {
                item.CopyToClipboard();
            }
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void ButtonPreviewItem_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            if (button.DataContext is Item item)
            {
                PreviewItem(item);
            }
        }

        private void PreviewItem(Item item)
        {
            if (item == null)
                return;

            Console.WriteLine("Preview " + item);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;

            Hide();
        }
    }
}
