using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ZippyClip.Commands
{
    public class DeleteItemCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public DeleteItemCommand(MainWindow parent)
        {
            if (parent == null) throw new ArgumentNullException(nameof(parent));

            Parent = parent;
        }

        protected MainWindow Parent { get; }

        public bool CanExecute(object parameter)
        {
            return parameter is Items.Item;
        }

        public void Execute(object parameter)
        {
            if (parameter is Items.Item item)
            {
                Parent.ClipboardHistory.Remove(item);
            }
        }

        protected void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
