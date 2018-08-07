using HelixToolkit.Wpf;
using Scene3D2Gif;
using System;
using System.IO;
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
using System.ComponentModel;

namespace Scene3DLib
{
    /// <summary>
    /// Interaktionslogik für Scene3DButton.xaml
    /// </summary>
    public partial class Scene3DButton : UserControl
    {
        public static readonly DependencyProperty Scene3DTooltipProperty =
         DependencyProperty.Register("Scene3DTooltip",
         typeof(string),
         typeof(Scene3DButton),
         new UIPropertyMetadata(null));

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
                this.Scene3DTooltip = value;
                this.buildTextGeometry();
            }
        }
        public string Scene3DTooltip
        {
            get
            {
                string v = (string)GetValue(Scene3DTooltipProperty);
                return v;
            }
            set
            {
                SetValue(Scene3DTooltipProperty, value);
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

            //string v = (string)GetValue(Scene3DTextProperty);
            //Debug.WriteLine("<<<OnApplyTemplate:" + v + ">>>");

            this.buildTextGeometry();
            this.buildObjGeometry();

            this.Scene3DTooltip = this.Scene3DText;
            if(string.IsNullOrWhiteSpace(this.Scene3DTooltip) && !string.IsNullOrWhiteSpace(this.Scene3DObj))
            {
                this.Scene3DTooltip = System.IO.Path.GetFileNameWithoutExtension(this.Scene3DObj);
            }

            /*{
                ColorAnimation animation;
                animation = new ColorAnimation();
                animation.From = Colors.Orange;
                animation.To = Colors.Gray;
                animation.Duration = new Duration(TimeSpan.FromSeconds(1));
                animation.RepeatBehavior = RepeatBehavior.Forever;
                animation.AutoReverse = true;
                this.scene3DButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            }*/
        }
        protected void buildObjGeometry()
        {
            if (String.IsNullOrWhiteSpace(this.Scene3DObj)) return;

            ModelVisual3D device = new ModelVisual3D();
            device.Content = Scene3DLib.Scene3D.getModel(this.Scene3DObj);
            this.viewport.Children.Add(device);
            device.Transform = Scene3D.CreateAnimatedTransform1(new Media3D.Transform3DGroup(), new Vector3D(3, 0, 0), new Vector3D(0.1, 0.1, 0));
        }
        protected void buildTextGeometry()
        {
            if (String.IsNullOrWhiteSpace(this.Scene3DText)) return;

            this.Scene3DTextGeometry = Scene3D.get3DText(this.Scene3DText);

            //PointCollection pc = this.Scene3DTextGeometry.TextureCoordinates;

            {
                Point3D pos = this.viewport.Camera.Position;
                pos.Z -= 99;//move cam downwards
                viewport.Camera.Position = pos;
                /*
                Point3D pos = this.viewport.Camera.Position;
                pos.X -= (this.Scene3DText.Length / 2);//=move cam to the right!
                //pos.Z -= 3;//move cam downwards
                pos.Z -= 5;
                pos.Y -= 3;
                viewport.Camera.Position = pos;
                */
            }
            {
                ItemsModel3DTransform = Scene3D.CreateAnimatedTransform1(new Media3D.Transform3DGroup(), new Vector3D(3, 0, 0), new Vector3D(0.1, 0, 0));
                //OnPropertyChanged(nameof(ItemsModel3DTransform));
            }
        }
    }
}
