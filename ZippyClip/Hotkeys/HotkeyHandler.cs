namespace ZippyClip.Hotkeys
{
    using System;
    using System.Windows.Forms;

    public class HotkeyHandler
    {
        public HotkeyHandler(int id, KeyModifiers modifiers, Keys key)
        {
            Id = new HotkeyId(id);
            Modifiers = modifiers;
            Key = key;

            ClipboardNotification.RegisterHotkeyHandler(this);
        }

        public event EventHandler Pressed;

        public HotkeyId Id { get; }

        public KeyModifiers Modifiers { get; }

        public Keys Key { get; }

        internal void HotkeyPressed()
        {
            OnPressed();
        }

        private void OnPressed()
        {
            Pressed?.Invoke(this, EventArgs.Empty);
        }
    }
}
