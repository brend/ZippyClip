namespace ZippyClip.Commands
{
    using System;
    using System.Windows.Input;

    public sealed class ShowMessageCommand : ICommand
    {
        public void Execute(object parameter)
        {
            if (parameter is MainWindow window)
            {
                window.WakeUp();
            }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}
