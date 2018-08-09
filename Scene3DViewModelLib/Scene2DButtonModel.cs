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
    public class Scene2DButtonModel : BaseViewModel
    {
        public Scene2DButtonModel()
        {

        }
        public Scene2DButtonModel(string imgSrc, string tooltipText, ActionCommand commandClick, ActionCommand commandMouseOver)
        {
            this.imgSrc = imgSrc;
            this.tooltipText = tooltipText;
            this.commandClick = commandClick;
            this.commandMouseOver = commandMouseOver;
        }

        protected string imgSrc { get => this.imgSrc; set { this.imgSrc = value; NotifyPropertyChanged(); } }
        protected string tooltipText { get => this.tooltipText; set { this.tooltipText = value; NotifyPropertyChanged(); } }
        protected ActionCommand commandClick { get => this.commandClick; set { this.commandClick = value; NotifyPropertyChanged(); } }
        protected ActionCommand commandMouseOver { get => this.commandMouseOver; set { this.commandMouseOver = value; NotifyPropertyChanged(); } }
    }
}
