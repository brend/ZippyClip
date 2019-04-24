namespace ZippyClip.Commands
{
    using System;
    using System.Windows.Input;

    public class ShowMessageCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

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

        protected void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
