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
            this.ImgSrc = imgSrc;
            this.TooltipText = tooltipText;
            this.CommandClick = commandClick;
            this.CommandMouseOver = commandMouseOver;
        }

        protected string m_imgSrc;
        protected string m_tooltipText;
        protected ActionCommand m_commandClick;
        protected ActionCommand m_commandMouseOver;

        public string ImgSrc { get => this.m_imgSrc; set { this.m_imgSrc = value; NotifyPropertyChanged(); } }
        public string TooltipText { get => this.m_tooltipText; set { this.m_tooltipText = value; NotifyPropertyChanged(); } }
        public ActionCommand CommandClick { get => this.m_commandClick; set { this.m_commandClick = value; NotifyPropertyChanged(); } }
        public ActionCommand CommandMouseOver { get => this.m_commandMouseOver; set { this.m_commandMouseOver = value; NotifyPropertyChanged(); } }

        public override string ToString()
        {
            return "Scene2DButtonModel {imgSrc} {tooltipText}";
        }
    }
}
