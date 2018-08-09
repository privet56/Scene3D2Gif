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
        }
    }
}
