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

        public Scene2DButtonModel(string imgSrc, string tooltipText, ActionCommand command, int index)
        {
            this.ImgSrc = imgSrc;
            this.TooltipText = tooltipText;
            this.Command = command;
            this.Index = index;
        }

        protected int m_index;
        protected string m_imgSrc;
        protected string m_tooltipText;
        protected ActionCommand m_command;

        public int Index { get => this.m_index; set { this.m_index = value; NotifyPropertyChanged(); } }
        public string ImgSrc { get => this.m_imgSrc; set { this.m_imgSrc = value; NotifyPropertyChanged(); } }
        public string TooltipText { get => this.m_tooltipText; set { this.m_tooltipText = value; NotifyPropertyChanged(); } }
        public ActionCommand Command { get => this.m_command; set { this.m_command = value; NotifyPropertyChanged(); } }

        public override string ToString()
        {
            return "Scene2DButtonModel {imgSrc} {tooltipText}";
        }
    }
}
