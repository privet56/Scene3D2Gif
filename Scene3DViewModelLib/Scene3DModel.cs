using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Scene3DViewModelLib
{
    public class Scene3DModel : BaseViewModel
    {
        public Scene3DModel()
        {

        }
        public Scene3DModel(string scene3DObj, ICommand command)
        {
            this.Scene3DObj = scene3DObj;
            this.Command = command;
        }

        protected string m_Scene3DObj;
        protected ICommand m_Command;

        public string Scene3DObj
        {
            get => this.m_Scene3DObj;
            set
            {
                this.m_Scene3DObj = value;
                NotifyPropertyChanged();
            }
        }
        public ICommand Command
        {
            get => this.m_Command;
            set
            {
                this.m_Command = value;
                NotifyPropertyChanged();
            }
        }
    }
}
