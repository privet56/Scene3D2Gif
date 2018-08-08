using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Scene3D2Lib
{
    public class ActionCommand : ICommand
    {
        protected readonly Action<object> action;
        protected readonly Predicate<object> canExecute;
        protected Action<bool> onFocus;

        public ActionCommand(Action<object> action) : this(action, null, null)
        {

        }
        public ActionCommand(Action<object> action, Predicate<object> canExecute) : this(action, canExecute, null)
        {

        }
        public ActionCommand(Action<object> action, Predicate<object> canExecute, Action<bool> onFocus)
        {
            this.action = action;
            this.canExecute = canExecute;
            this.onFocus = onFocus;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public bool CanExecute(object parameter)
        {
            return canExecute == null ? true : canExecute(parameter);
        }
        public void Execute(object parameter)
        {
            action(parameter);
        }
        public void setOnFocusAction(Action<bool> onFocus)
        {
            this.onFocus = onFocus;
        }
        public void OnFocus(bool focus)
        {
            this.onFocus?.Invoke(focus);
        }
    }
}
