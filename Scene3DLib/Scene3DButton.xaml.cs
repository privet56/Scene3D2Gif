using HelixToolkit.Wpf;
using Scene3D2Gif;
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

using System.Diagnostics;

namespace Scene3DLib
{
    /// <summary>
    /// Interaktionslogik für Scene3DButton.xaml
    /// </summary>
    public partial class Scene3DButton : UserControl
    {
        public static readonly DependencyProperty Scene3DTextProperty =
         DependencyProperty.Register("Scene3DText",
         typeof(string),
         typeof(Scene3DButton),
         new UIPropertyMetadata(" "));

        public static readonly DependencyProperty Scene3DTextGeometryProperty =
         DependencyProperty.Register("Scene3DTextGeometry",
         typeof(MeshGeometry3D),
         typeof(Scene3DButton),
         new UIPropertyMetadata(null));

        public static readonly DependencyProperty Scene3DCmdProperty =
         DependencyProperty.Register("Scene3DCmd",
         typeof(ICommand),
         typeof(Scene3DButton),
         new UIPropertyMetadata(null));

        static Scene3DButton()
        {
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(Scene3DButton), new FrameworkPropertyMetadata(typeof(Scene3DButton)));
        }

        public Scene3DButton()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        public string Scene3DText
        {
            get
            {
                string v = (string)GetValue(Scene3DTextProperty);
                return v;
            }
            set
            {
                SetValue(Scene3DTextProperty, value);
                this.buildTextGeometry();
            }
        }
        public MeshGeometry3D Scene3DTextGeometry
        {
            get => (MeshGeometry3D)GetValue(Scene3DTextGeometryProperty);
            set
            {
                SetValue(Scene3DTextGeometryProperty, value);
            }
        }
        public ICommand Scene3DCmd
        {
            get => (ICommand)GetValue(Scene3DCmdProperty);
            set
            {
                SetValue(Scene3DCmdProperty, value);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            string v = (string)GetValue(Scene3DTextProperty);
            //Debug.WriteLine("<<<OnApplyTemplate:" + v + ">>>");
            this.buildTextGeometry();
        }

        protected void buildTextGeometry()
        {
            var builder = new MeshBuilder(false, false);
            Debug.WriteLine("<<<buildTextGeometry:" + this.Scene3DText + ">>>");
            builder.ExtrudeText(
                this.Scene3DText,
                "Arial",
                FontStyles.Normal,
                FontWeights.Bold,
                2,
                new Vector3D(-1, 0, 0), //text direction
                new Point3D(0, 0, 0),
                new Point3D(0, 0, 1));

            this.Scene3DTextGeometry = builder.ToMesh(true);
            PointCollection pc = this.Scene3DTextGeometry.TextureCoordinates;
        }
    }
}
