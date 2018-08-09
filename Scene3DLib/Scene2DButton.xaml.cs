using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace Scene3DLib
{
    /// <summary>
    /// Interaktionslogik für Scene2Dbutotn.xaml
    /// </summary>
    public partial class Scene2DButton : UserControl
    {
        public static readonly DependencyProperty ButtonModelProperty =
         DependencyProperty.Register("ButtonModel",
         typeof(Scene3DViewModelLib.Scene2DButtonModel),
         typeof(Scene2DButton),
         new UIPropertyMetadata(null));

        public static readonly DependencyProperty TooltipTextProperty =
         DependencyProperty.Register("TooltipText",
         typeof(string),
         typeof(Scene2DButton),
         new UIPropertyMetadata(null));


        public Scene2DButton()
        {
            InitializeComponent();
        }
        public void OnMouseEnter(object sender, MouseEventArgs e)
        {

        }
        public void OnMouseOut(object sender, MouseEventArgs e)
        {

        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //string imgsrc = "pack://application:,,,/Scene3DRes;component/res/grid.png";
            //this.img.Source = new BitmapImage(new Uri(this.img.Source.ToString()));
            Debug.WriteLine("OnApplyTemplate:" + TooltipText);
        }
        public Scene3DViewModelLib.Scene2DButtonModel ButtonModel
        {
            get
            {
                var v = (Scene3DViewModelLib.Scene2DButtonModel)GetValue(ButtonModelProperty);
                //Debug.WriteLine("SET:"+v.TooltipText);
                return v;
            }
            set
            {
                SetValue(ButtonModelProperty, value);
                //Debug.WriteLine("GET:" + value.TooltipText);
            }
        }
        public string TooltipText
        {
            get
            {
                var v = (string)GetValue(TooltipTextProperty);
                Debug.WriteLine("SET_Tooltip:"+v);
                return v;
            }
            set
            {
                SetValue(TooltipTextProperty, value);
                Debug.WriteLine("GET_Tooltip:" + value);
            }
        }
    }
}
