﻿using HelixToolkit.Wpf;
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
using PropertyTools.Wpf;
using Scene3DViewModelLib;
using Scene3DLib;
using System.Reflection;
using System.CodeDom.Compiler;
using Microsoft.JScript;
using System.Threading;
using System.Windows.Threading;

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

        public static readonly DependencyProperty currentScene3DProperty =
         DependencyProperty.Register("currentScene3D",
         typeof(Scene3D),
         typeof(MainWindow),
         new UIPropertyMetadata(null));

        public MainWindow()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            Scene3DEles = new ObservableCollection<Scene3DViewModelLib.Scene3DModel>();
            InitializeComponent();

            //this.DataContext = this;//needed for button-list        //TODO: don't do this!!!
            /* 
             * 1) destroys manipulator
             * 2) sets blue on deselect (->solved with attached prop!)

            var vm = new MainWindowViewModel(this.helixViewport3D.Viewport);
            this.helixViewport3D.InputBindings.Add(new MouseBinding(vm.RectangleSelectionCommand, new MouseGesture(MouseAction.LeftClick)));
            this.helixViewport3D.InputBindings.Add(new MouseBinding(vm.PointSelectionCommand, new MouseGesture(MouseAction.LeftClick, ModifierKeys.Control)));
            */

            this.currentScene3D = new Scene3D(this.helixViewport3D, this.gridLinesVisual3D, this.lights, this.panoramaCube3D);

            Task.Delay(999).ContinueWith(t => {

                {
                    AppJSAutomator appJSAutomator = new Scene3DLib.AppJSAutomator(this);
                    string scene3DObjectLoaded = appJSAutomator.onStartUp(@"res\startup_automation.js");
                }
                {   //show hint on startup!
                    CalloutWindow.showHint("Tools allow to show/hide the controls of the 3D Viewport.", 5000, this);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
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
        public Scene3D currentScene3D
        {
            get
            {
                return (Scene3D)GetValue(currentScene3DProperty);
            }
            set
            {
                SetValue(currentScene3DProperty, value);
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
        public Action<string> OnFocusAction
        {
            get
            {
                return (s) => {
                    On3DControlFocus(s);
                };
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

        public async void rezip(string inDir, string tempDir, string outDir)
        {
            new Zipper().Rezip(inDir, tempDir, outDir);
        }

        public async void OnInsertAgain(string scene3DModelObj)
        {
            var _previousCursor = Mouse.OverrideCursor;
            Mouse.OverrideCursor = Cursors.Wait;

            //sync code
            //ModelVisual3D device = new ModelVisual3D();
            //device.Content = Scene3DLib.Scene3D.getModel(scene3DModelObj);
            //OnInsertAgain(scene3DModelObj, device, _previousCursor);

            //ATTENTION: Viewport3D seems to have a problem with non-ui-thread-loaded 3D objects !!!
            //This API was accessed with arguments from the wrong context.
            //Der aufrufende Thread kann nicht auf dieses Objekt zugreifen, da sich das Objekt im Besitz eines anderen Threads befindet.
            //https://www.google.ca/search?q=This%20API%20was%20accessed%20with%20arguments%20from%20the%20wrong%20context.&cad=h
            //solution: load Model3D & Freeze it!

            TaskScheduler uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            Dispatcher uiDispatcher = Application.Current.Dispatcher;

            var syncContext = SynchronizationContext.Current;
            Model3D model3D = await Scene3DObjLoader.GetInstance().LoadObjectWithTaskAsync(scene3DModelObj);
            await syncContext;
            ModelVisual3D device = new ModelVisual3D();
            device.Content = model3D;
            OnInsertAgain(scene3DModelObj, device, _previousCursor);

            /*
            Model3D model3D = await Scene3DObjLoader.GetInstance().LoadObjectWithTaskAsync(scene3DModelObj).;

            Task.Factory.StartNew(() =>
            {
                ModelVisual3D device1 = Scene3DObjLoader.GetInstance().LoadOjbect(scene3DModelObj);
                return device1;
            }).ContinueWith((device2) =>
            {
                OnInsertAgain(scene3DModelObj, device2.Result, _previousCursor);

            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler);
            * /
            /*
            var syncContext = SynchronizationContext.Current;
            ModelVisual3D device = await Scene3DObjLoader.GetInstance().LoadObjectWithTaskAsync(scene3DModelObj);
            await syncContext;
            * /
            var syncContext = SynchronizationContext.Current;
            ModelVisual3D device = null;
            await Task.Factory.StartNew(() => {
                device = Scene3DObjLoader.GetInstance().LoadOjbect(scene3DModelObj);

            }, TaskCreationOptions.LongRunning);
            await syncContext;  //This API was accessed with arguments from the wrong context.

            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {
                //uiDispatcher.Invoke(new OnInsertAgainDelegate(OnInsertAgain), scene3DModelObj, device, _previousCursor));
                uiDispatcher.Invoke(new Action(() => OnInsertAgain(scene3DModelObj, device, _previousCursor)));
            }));
            */
        }

        public delegate void OnInsertAgainDelegate(string scene3DModelObj, ModelVisual3D device, Cursor _previousCursor);

        public void OnInsertAgain(string scene3DModelObj, ModelVisual3D device, Cursor _previousCursor)
        {
            Debug.WriteLine("CALLING async is DONE ...curthread:" + Application.Current.Dispatcher.ToString());
            this.helixViewport3D.Children.Add(device);
            Scene3DViewModelLib.Scene3DModel scene3DModel = new Scene3DViewModelLib.Scene3DModel(scene3DModelObj, new ActionCommand(action => OnInsertAgain(scene3DModelObj), canExecute => true, (b) =>
            {
                On3DControlFocus(b ? "Insert Again" : null);
            }), device);

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

            Debug.WriteLine("CALLED  async...");
        }
        public void OnScreenshot()
        {
            //TODO: remove grid before screenshot
            /*
            bool showCamInfo = this.helixViewport3D.ShowCameraInfo;
            bool showViewCube = this.helixViewport3D.ShowViewCube;
            this.helixViewport3D.ShowCameraInfo = false;
            this.helixViewport3D.ShowViewCube = false;
            this.gridLinesVisual3D.Visible = false;
            */
            //RenderTargetBitmap b = new RenderTargetBitmap((int)this.helixViewport3D.ActualWidth, (int)this.helixViewport3D.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            //b.Render(this.helixViewport3D);

            //alternative: export to file
            //this.helixViewport3D.Export(fn);

            Task.Delay(100).ContinueWith(t => {

                BitmapSource bs = this.CaptureScreen(this.helixViewport3D, 96, 96);
                Clipboard.SetImage(bs);
                /*
                this.helixViewport3D.ShowCameraInfo = showCamInfo;
                this.helixViewport3D.ShowViewCube = showViewCube;
                this.gridLinesVisual3D.Visible = true;
                */
                this.On3DControlFocus("DONE. Screenhshot is in the Clipboard.");

            }, TaskScheduler.FromCurrentSynchronizationContext());

        }
        private BitmapSource CaptureScreen(Visual target, double dpiX, double dpiY)
        {
            if (target == null)
            {
                return null;
            }
            Rect bounds = VisualTreeHelper.GetDescendantBounds(target);
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)(bounds.Width * dpiX / 96.0),
                                                            (int)(bounds.Height * dpiY / 96.0),
                                                            dpiX,
                                                            dpiY,
                                                            PixelFormats.Pbgra32);
            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext ctx = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(target);
                ctx.DrawRectangle(vb, null, new Rect(new Point(), bounds.Size));
            }
            rtb.Render(dv);
            return rtb;
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
