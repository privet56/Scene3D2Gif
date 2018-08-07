using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Scene3D2Gif
{
    public class ApplicationCloseCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return Application.Current != null && Application.Current.MainWindow != null;
        }

        public void Execute(object parameter)
        {
            Application.Current.MainWindow.Close();
        }
    }

    public static class AppCommands
    {
        private static readonly ICommand appCloseCmd = new ApplicationCloseCommand();
        public static ICommand ApplicationCloseCommand
        {
            get { return appCloseCmd; }
        }
    }

    public class ActionCommand : ICommand
    {
        private readonly Action<object> action;
        private readonly Predicate<object> canExecute;
        public ActionCommand(Action<object> action) : this(action, null)
        { }
        public ActionCommand(Action<object> action,
        Predicate<object> canExecute)
        {
            this.action = action;
            this.canExecute = canExecute;
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
    }
}
