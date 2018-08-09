using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace Scene3DViewModelLib
{
    public class Scene3DModel : BaseViewModel
    {
        public Scene3DModel()
        {

        }
        public Scene3DModel(string scene3DObj, ICommand command, ModelVisual3D scene3DModelVisual)
        {
            this.Scene3DObj = scene3DObj;
            this.Command = command;
            this.Scene3DModelVisual = scene3DModelVisual;
        }

        protected string m_Scene3DObj;
        protected ICommand m_Command;
        protected ModelVisual3D m_Scene3DModelVisual;

        public string Scene3DObj
        {
            get => this.m_Scene3DObj;
            set
            {
                this.m_Scene3DObj = value;
                NotifyPropertyChanged();
            }
        }
        public ModelVisual3D Scene3DModelVisual
        {
            get => this.m_Scene3DModelVisual;
            set
            {
                this.m_Scene3DModelVisual = value;
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
