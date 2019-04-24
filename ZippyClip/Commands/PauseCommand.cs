#nullable enable

using System;
using System.Windows.Input;

namespace ZippyClip.Commands
{
    public class PauseCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => (parameter as MainWindow)?.TogglePaused();

        protected void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
