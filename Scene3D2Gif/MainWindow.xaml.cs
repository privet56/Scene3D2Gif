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
        private MeshGeometry3D textGeometry;

        public MainWindow()
        {
            InitializeComponent();

            ModelVisual3D device = new ModelVisual3D();
            //string path = @"..\..\..\..\_obj\taeyeon_a.obj";                  //not visible...
            //string path = @"d:\projects\Scene3D2Gif\_obj\bunny\bunny.obj";    //works
            //string path = @"d:\projects\Scene3D2Gif\_obj\lea\lea.obj";          //works
            //string path = @"..\..\..\..\_obj\bunny\taeyeon_test.obj";
            string path = @"res/DinoRider.3ds";
            device.Content = Scene3DLib.Scene3D.getModel(path);
            this.helixViewport3D.Children.Add(device);

            {
                var builder = new MeshBuilder(false, false);
                builder.ExtrudeText(
                    "Helix Toolkit",
                    "Arial",
                    FontStyles.Normal,
                    FontWeights.Bold,
                    2,
                    new Vector3D(-1, 0, 0), //text direction
                    new Point3D(0, 0, 0),
                    new Point3D(0, 0, 1));

                this.textGeometry = builder.ToMesh(true);
                PointCollection pc = this.textGeometry.TextureCoordinates;
                this.DataContext = this;
            }
        }
        public MeshGeometry3D TextGeometry
        {
            get
            {
                return this.textGeometry;
            }
        }

    }
}
