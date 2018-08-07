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
using Scene3D2Gif;

using System.Diagnostics;

using HelixToolkit.Wpf.SharpDX;
using System.Windows.Media.Animation;

using Media3D = System.Windows.Media.Media3D;

namespace Scene3DLib
{
    public class Scene3D
    {
        public static Model3D getModel(string path)
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
                Debug.WriteLine("ERR importing '"+ path + "' : "+e.Message);
            }
            return device;
        }
        public static System.Windows.Media.Media3D.MeshGeometry3D get3DText(string text)
        {
            var builder = new HelixToolkit.Wpf.MeshBuilder(false, false);
            //Debug.WriteLine("<<<get3DText:" + text + ">>>");
            builder.ExtrudeText(
                text,
                "Arial",
                FontStyles.Normal,
                FontWeights.Bold,
                2,
                new Vector3D(-1, 0, 0), //text direction
                new Point3D(0, 0, 0),
                new Point3D(0, 0, 0.001));

            return builder.ToMesh(true);
        }
        public static Media3D.Transform3D CreateAnimatedTransform1(Media3D.Transform3DGroup transformGroup, Media3D.Vector3D center, Media3D.Vector3D axis, double speed = 4)
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
