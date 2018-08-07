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

using HelixToolkit.Wpf.SharpDX;
using System.Windows.Media.Animation;

using Media3D = System.Windows.Media.Media3D;

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
         new UIPropertyMetadata(null));

        public static readonly DependencyProperty Scene3DObjProperty =
         DependencyProperty.Register("Scene3DObj",
         typeof(string),
         typeof(Scene3DButton),
         new UIPropertyMetadata(null));

        public static readonly DependencyProperty Scene3DTextGeometryProperty =
         DependencyProperty.Register("Scene3DTextGeometry",
         typeof(System.Windows.Media.Media3D.MeshGeometry3D),
         typeof(Scene3DButton),
         new UIPropertyMetadata(null));

        public static readonly DependencyProperty Scene3DCmdProperty =
         DependencyProperty.Register("Scene3DCmd",
         typeof(ICommand),
         typeof(Scene3DButton),
         new UIPropertyMetadata(null));

        public Transform3D ItemsModel3DTransform { private set; get; } = new Media3D.TranslateTransform3D(0, 0, 5);

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
        public string Scene3DObj
        {
            get
            {
                string v = (string)GetValue(Scene3DObjProperty);
                return v;
            }
            set
            {
                SetValue(Scene3DObjProperty, value);
            }
        }
        public System.Windows.Media.Media3D.MeshGeometry3D Scene3DTextGeometry
        {
            get => (System.Windows.Media.Media3D.MeshGeometry3D)GetValue(Scene3DTextGeometryProperty);
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
            if (String.IsNullOrWhiteSpace(this.Scene3DText)) return;

            var builder = new HelixToolkit.Wpf.MeshBuilder(false, false);
            Debug.WriteLine("<<<buildTextGeometry:" + this.Scene3DText + ">>>");
            builder.ExtrudeText(
                this.Scene3DText,
                "Arial",
                FontStyles.Normal,
                FontWeights.Bold,
                2,
                new Vector3D(-1, 0, 0), //text direction
                new Point3D(0, 0, 0),
                new Point3D(0, 0, 0.001));

            this.Scene3DTextGeometry = builder.ToMesh(true);
            PointCollection pc = this.Scene3DTextGeometry.TextureCoordinates;

            Point3D pos = this.viewport.Camera.Position;

            pos.X -= (this.Scene3DText.Length / 2);//=move cam to the right!
            //pos.Z -= 3;//move cam downwards
            pos.Z -= 5;
            pos.Y -= 3;
            viewport.Camera.Position = pos;

            {
                ItemsModel3DTransform = CreateAnimatedTransform1(new Media3D.Transform3DGroup(), new Vector3D(3, 0, 0), new Vector3D(0.1, 0, 0));
                //OnPropertyChanged(nameof(ItemsModel3DTransform));
            }
        }

        private static Media3D.Transform3D CreateAnimatedTransform1(Media3D.Transform3DGroup transformGroup, Media3D.Vector3D center, Media3D.Vector3D axis, double speed = 4)
        {
            var rotateAnimation = new Rotation3DAnimation
            {
                RepeatBehavior = RepeatBehavior.Forever,
                By = new Media3D.AxisAngleRotation3D(axis, 90),
                Duration = TimeSpan.FromSeconds(speed / 2),
                IsCumulative = true,
            };

            var rotateTransform = new Media3D.RotateTransform3D();
            rotateTransform.BeginAnimation(Media3D.RotateTransform3D.RotationProperty, rotateAnimation);

            transformGroup.Children.Add(rotateTransform);

            var rotateAnimation1 = new Rotation3DAnimation
            {
                RepeatBehavior = RepeatBehavior.Forever,
                By = new Media3D.AxisAngleRotation3D(axis, 240),
                Duration = TimeSpan.FromSeconds(speed / 4),
                IsCumulative = true,
            };

            var rotateTransform1 = new Media3D.RotateTransform3D();
            rotateTransform1.CenterX = center.X;
            rotateTransform1.CenterY = center.Y;
            rotateTransform1.CenterZ = center.Z;
            rotateTransform1.BeginAnimation(Media3D.RotateTransform3D.RotationProperty, rotateAnimation1);

            transformGroup.Children.Add(rotateTransform1);

            return transformGroup;
        }
    }
}
