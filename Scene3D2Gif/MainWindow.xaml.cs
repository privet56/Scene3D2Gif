using HelixToolkit.Wpf;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Scene3D2Gif
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ModelVisual3D device = new ModelVisual3D();
            //string path = @"..\..\..\..\_obj\taeyeon_a.obj";                  //not visible...
            //string path = @"d:\projects\Scene3D2Gif\_obj\bunny\bunny.obj";    //works
            string path = @"d:\projects\Scene3D2Gif\_obj\lea\lea.obj";          //works
            //string path = @"..\..\..\..\_obj\bunny\taeyeon_test.obj";
            device.Content = getModel(path);
            this.helixViewport3D.Children.Add(device);
        }

        public Model3D getModel(string path)
        {
            Model3D device = null;
            try
            {
                //viewport3D.RotateGesture = new MouseGesture(MouseAction.LeftClick);
                ModelImporter import = new ModelImporter();
                device = import.Load(path);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return device;
        }
    }
}
