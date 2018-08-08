using HelixToolkit.Wpf;
using Microsoft.Win32;
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
using Media3D = System.Windows.Media.Media3D;
using System.Diagnostics;
using System.Collections.ObjectModel;
using Scene3D2Lib;

namespace Scene3D2Gif
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly DependencyProperty Scene3DElesProperty =
         DependencyProperty.Register("Scene3DEles",
         typeof(ObservableCollection<Scene3DViewModelLib.Scene3DModel>),
         typeof(MainWindow),
         new UIPropertyMetadata(null));


        public MainWindow()
        {
            Scene3DEles = new ObservableCollection<Scene3DViewModelLib.Scene3DModel>();
            InitializeComponent();
            this.DataContext = this;
        }
        public ObservableCollection<Scene3DViewModelLib.Scene3DModel> Scene3DEles
        {
            get
            {
                return (ObservableCollection<Scene3DViewModelLib.Scene3DModel>)GetValue(Scene3DElesProperty);
            }
            set
            {
                SetValue(Scene3DElesProperty, value);
            }
        }
        public ICommand InsertCommand
        {
            get
            {
                return new ActionCommand(action => OnInsert(), canExecute => true, (b) => {
                    On3DControlFocus(b ? "Insert a 3D Object" : null);
                });
            }
        }
        public void On3DControlFocus(string mouseOverText)
        {
            statBarText.Text = mouseOverText == null ? "Ready..." : mouseOverText;
        }
        public ICommand ScreenshotCommand
        {
            get
            {
                return new ActionCommand(action => OnScreenshot(), canExecute => true, (b) => {
                    On3DControlFocus(b ? "Put Screenshot into Clipboard" : null);
                });
            }
        }

        public void OnInsertAgain(string scene3DModelObj)
        {
            var _previousCursor = Mouse.OverrideCursor;
            Mouse.OverrideCursor = Cursors.Wait;

            ModelVisual3D device = new ModelVisual3D();
            device.Content = Scene3DLib.Scene3D.getModel(scene3DModelObj);
            this.helixViewport3D.Children.Add(device);
            Scene3DViewModelLib.Scene3DModel scene3DModel = new Scene3DViewModelLib.Scene3DModel(scene3DModelObj, new ActionCommand(action => OnInsertAgain(scene3DModelObj), canExecute => true, (b) => {
                On3DControlFocus(b ? "Insert Again" : null);
            }));
            Scene3DEles.Add(scene3DModel);

            {
                //< ht:BindableTranslateManipulator Direction = "1 0 0"  Length = "5" Diameter = "1" Color = "Black" Value = "{Binding TranslateValue}" TargetTransform = "{Binding ElementName=sphere1, Path=Transform}" />
                TranslateManipulator manipulator = new TranslateManipulator();
                manipulator.Bind(device);
                manipulator.Color = Colors.Red;
                manipulator.Direction = new Vector3D(1, 0, 0);
                manipulator.Diameter = 0.1;
                manipulator.Length = 9;//TODO: len = obj-len * 2
                this.helixViewport3D.Children.Add(manipulator);
            }

            Mouse.OverrideCursor = _previousCursor;
        }
        public void OnScreenshot()
        {
            //TODO: remove grid before screenshot
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)this.helixViewport3D.ActualWidth, (int)this.helixViewport3D.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(this.helixViewport3D);
            Clipboard.SetImage(rtb);
        }
        public void OnInsert()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = Scene3DLib.Scene3D.AssemblyDirectory;
            openFileDialog.Filter = "obj files (*.obj)|*.obj|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            Nullable<bool> r = openFileDialog.ShowDialog();

            if (r != true)
            {
                Debug.WriteLine("!openFileDialog.ShowDialog()");
                return;
            }

            OnInsertAgain(openFileDialog.FileName);
        }
    }
}
