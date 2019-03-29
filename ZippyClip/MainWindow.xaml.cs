#nullable enable

namespace ZippyClip
{
    using Items;
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using ZippyClip.Hotkeys;
    using Screen = System.Windows.Forms.Screen;

    using static Windows.WinApi;

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

            for (int i = 0; i < ClipboardItems.Items.Count; ++i)
            {
                ClipboardItems.Items[i].ListIndex = (i < 9) ? (i + 1) : (int?)null;
            }
        }

        private ClipboardItemCollection ClipboardItems = new ClipboardItemCollection();

        public ObservableCollection<Item> ClipboardHistory =>
            ClipboardItems.Items;

        public static readonly DependencyProperty HistoryIsEmptyProperty =
            DependencyProperty.Register(nameof(HistoryIsEmpty), typeof(bool), typeof(MainWindow), new PropertyMetadata(true));

        public bool HistoryIsEmpty
        {
            get => (bool)GetValue(HistoryIsEmptyProperty);
            set => SetValue(HistoryIsEmptyProperty, value);
        }

        public Item? SelectedItem { get; set; }

        private void ListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            CopySelectedItemToClipboard();
        }

        private void CopySelectedItemToClipboard()
        {
            if (SelectedItem == null)
                return;

            Console.WriteLine("Copied to clipboard: " + SelectedItem);

            SelectedItem.CopyToClipboard();
        }

        private void HideAndPaste()
        {
            Hide();

            ThreadPool.QueueUserWorkItem(sleepAndPaste);

            void sleepAndPaste(object userState)
            {
                Thread.Sleep(100);
                System.Windows.Forms.SendKeys.SendWait("^v");
            }
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
            ClipboardHistory.CollectionChanged += delegate { HistoryIsEmpty = ClipboardHistory.Count == 0; };

            RegisterHotkeys();
            Hide();
        }

        private void RegisterHotkeys()
        {
            var hk = new HotkeyHandler(1, KeyModifiers.Alt | KeyModifiers.Control, System.Windows.Forms.Keys.V);

            hk.Pressed += delegate
            {
                WakeUp();
            };
        }

        public void WakeUp()
        {
            CenterWindow();
            Show();
            FocusOnList();
        }

        private void FocusOnList()
        {
            listClipboardItems.Focus();

            if (listClipboardItems.Items.Count > 0)
            {
                listClipboardItems.SelectedIndex = 0;
            }
        }

        private Screen GetScreenToAppearOn()
        {
            IntPtr handle = GetForegroundWindow();

            if (handle != IntPtr.Zero)
            {
                return Screen.FromHandle(handle);
            }
            else
            {
                return Screen.PrimaryScreen;
            }
        }

        private void CenterWindow()
        {
            Screen screen = GetScreenToAppearOn();            

            Left = screen.Bounds.Left + (screen.Bounds.Width - Width) / 2;
            Top = screen.Bounds.Top + (screen.Bounds.Height - Height) / 2;
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

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Return:
                    CopySelectedItemToClipboard();
                    HideAndPaste();
                    break;
                case Key.Escape:
                    Hide();
                    break;
                case Key.Q when Keyboard.Modifiers == ModifierKeys.Control:
                    Application.Current.Shutdown();
                    break;
                case Key k when k >= Key.D1 && k <= Key.D9:
                    if (SelectItem(k - Key.D1))
                    {
                        CopySelectedItemToClipboard();
                        HideAndPaste();
                    }
                    break;
                default:
                    break;
            }
        }

        private bool SelectItem(int index)
        {
            if (index < 0 || index >= listClipboardItems.Items.Count)
                return false;

            listClipboardItems.SelectedIndex = index;

            return true;
        }

        private void ListBoxItem_MouseEnter(object sender, MouseEventArgs e)
        {
            if ((sender as ListBoxItem)?.Content is Item item)

            ShowItemPreview(item);
        }

        private void ShowItemPreview(Item item)
        {
            if (!item.SupportsPreview)
                return;

            PreviewImage.Source = item.GetPreviewImage();
            PreviewText.Text = item.GetPreviewText();

            PreviewPopup.IsOpen = true;
        }

        private void HideItemPreview()
        {
            PreviewPopup.IsOpen = false;
        }

        private void Popup_MouseLeave(object sender, MouseEventArgs e)
        {
            HideItemPreview();
        }

        private void TheWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }
    }
}
