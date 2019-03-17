namespace ZippyClip
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ClipboardNotification.ClipboardUpdate += ClipboardNotification_ClipboardUpdate;

            RegisterHotkeys();            
        }

        HotKeyRegister hk1;

        private void RegisterHotkeys()
        {
            //hk1 = new HotKeyRegister(ClipboardNotification.NotificationWindowHandle, 1, KeyModifiers.Windows | KeyModifiers.Control, System.Windows.Forms.Keys.D1);

            //hk1.HotKeyPressed += delegate { MessageBox.Show("hk 1 has been pressed"); };
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
    }
}
