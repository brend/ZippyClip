#nullable enable

namespace ZippyClip
{
    using Items;
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using ZippyClip.Actions;
    using ZippyClip.Hotkeys;
    using static Windows.WinApi;
    using Screen = System.Windows.Forms.Screen;

    public partial class MainWindow : Window, INotifyPropertyChanging, INotifyPropertyChanged
    {
        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;
            DeleteItemCommand = new Commands.DeleteItemCommand(this);
            PauseCommand = new Commands.PauseCommand();
        }

        public IActionPerformer CopyToClipboardAction { get; } = new CopyToClipboardAction();

        public IActionPerformer AlternativeActionPerformer { get; } = new AlternativeActionPerformer();

        public ICommand DeleteItemCommand { get; } 

        public ICommand PauseCommand { get; }

        private void ClipboardNotification_ClipboardUpdate(object sender, EventArgs e)
        {
            PutClipboardContentsInList();
        }

        private void PutClipboardContentsInList()
        {
            if (Paused)
                return;

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

        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CopySelectedItemToClipboard();
        }

        private void CopySelectedItemToClipboard()
        {
            SelectedItem?.PerformAction(CopyToClipboardAction);
        }

        private void PerformAlternativeActionOnSelectedItem()
        {
            SelectedItem?.PerformAction(AlternativeActionPerformer);
        }

        private void DeleteSelectedItem()
        {
            if (SelectedItem != null)
                DeleteItemCommand.Execute(SelectedItem); 
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

        private Rect GetResoultionSafeWorkingArea(Screen screen)
        {
            PresentationSource presentationSource = PresentationSource.FromVisual(Application.Current.MainWindow);
            Matrix matrix = presentationSource.CompositionTarget.TransformToDevice;
            double widthFactor = matrix.M22,
                heightFactor = matrix.M11;
            System.Drawing.Rectangle workingArea = screen.WorkingArea;

            return new Rect(
                workingArea.Left / widthFactor, 
                workingArea.Top / heightFactor,
                workingArea.Width / widthFactor,
                workingArea.Height / heightFactor);
        }

        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);

            Console.WriteLine("X = {0}, Y = {1}", Left, Top);
        }

        private void CenterWindow()
        {
            Screen screen = GetScreenToAppearOn();
            Rect bounds = GetResoultionSafeWorkingArea(screen);

            Left = bounds.Left + (bounds.Width - Width) / 2;
            Top = bounds.Top + (bounds.Height - Height) / 2;
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Windows.Infrastructure.Navigate(e.Uri);
            Hide();
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
                case Key.Enter when Keyboard.Modifiers == ModifierKeys.Control:
                    PerformAlternativeActionOnSelectedItem();
                    break;

                case Key.Enter:
                    CopySelectedItemToClipboard();
                    HideAndPaste();
                    break;

                case Key.Escape:
                    Hide();
                    break;

                case Key.Delete:
                    DeleteSelectedItem();
                    break;

                case Key.C when Keyboard.Modifiers == ModifierKeys.Control:
                    CopySelectedItemToClipboard();
                    break;

                case Key.Q when Keyboard.Modifiers == ModifierKeys.Control:
                    QuitApplication();
                    break;

                case Key k when k >= Key.D1 && k <= Key.D9
                             || k >= Key.NumPad1 && k <= Key.NumPad9:   // Issue 20: Support for numeric keypad
                    SelectItemAndCopyToClipboard(k - (k >= Key.NumPad1 && k <= Key.NumPad9 ? Key.NumPad1 : Key.D1));
                    HideAndPaste();
                    break;

                default:
                    break;
            }
        }

        private void SelectItemAndCopyToClipboard(int itemIndex)
        {
            if (SelectItem(itemIndex))
            {
                CopySelectedItemToClipboard();
            }
        }

        private static void QuitApplication()
        {
            Windows.Infrastructure.QuitApplication();
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
            {
                ShowItemPreview(item);
            }
        }

        private void ListBoxItem_MouseLeave(object sender, MouseEventArgs e)
        {
            HideItemPreview();
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

        private void TheWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private bool Paused { get; set; }

        public void TogglePaused()
        {
            Paused = !Paused;
            PauseCommandHeader = Paused ? "Unpause" : "Pause";
        }

        private string pauseCommandHeader = "Pause";
        public string PauseCommandHeader
        {
            get => pauseCommandHeader;
            set
            {
                OnPropertyChanging(new PropertyChangingEventArgs(nameof(PauseCommandHeader)));

                pauseCommandHeader = value;

                OnPropertyChanged(new PropertyChangedEventArgs(nameof(PauseCommandHeader)));
            }
        }

        protected void OnPropertyChanging(PropertyChangingEventArgs e)
        {
            PropertyChanging?.Invoke(this, e);
        }

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        private void PauseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            PauseCommand.Execute(this);
        }

        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CopySelectedItemToClipboard();
            HideAndPaste();
        }
    }
}
