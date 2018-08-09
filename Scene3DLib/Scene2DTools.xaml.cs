using HelixToolkit.Wpf;
using Scene3DViewModelLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaktionslogik für Scene2DTools.xaml
    /// </summary>
    public partial class Scene2DTools : UserControl
    {
        public static readonly DependencyProperty ToolsProperty =
         DependencyProperty.Register("Tools",
         typeof(ObservableCollection<Scene3DViewModelLib.Scene2DButtonModel>),
         typeof(Scene2DTools),
         new UIPropertyMetadata(null));

        public static readonly DependencyProperty currentScene3DProperty =
         DependencyProperty.Register("currentScene3D",
         typeof(Scene3D),
         typeof(Scene2DTools),
         new UIPropertyMetadata(null));

        public static readonly DependencyProperty OnFocusActionProperty =
         DependencyProperty.Register("OnFocusAction",
         typeof(Action<string>),
         typeof(Scene2DTools),
         new UIPropertyMetadata(null));

        public Scene2DTools()
        {
            Tools = new ObservableCollection<Scene3DViewModelLib.Scene2DButtonModel>();

            InitializeComponent();

            //TODO: Actions in extra new Classes incl impl!

            {
                string txt = "Grid On/Off";
                ActionCommand cmd = new ActionCommand(action => OnGridToogle(), canExecute => true, (b) => {
                    OnFocus(b ? txt : null);
                });
                Scene2DButtonModel buttonModelGrid = new Scene2DButtonModel("pack://application:,,,/Scene3DRes;component/res/grid.png", txt, cmd, Tools.Count);
                Tools.Add(buttonModelGrid);
            }
            {
                string txt = "Camera Info On/Off";
                ActionCommand cmd = new ActionCommand(action => OnCameraInfoToogle(), canExecute => true, (b) => {
                    OnFocus(b ? txt : null);
                });
                Scene2DButtonModel buttonModelGrid = new Scene2DButtonModel("pack://application:,,,/Scene3DRes;component/res/cameraInfo.png", txt, cmd, Tools.Count);
                Tools.Add(buttonModelGrid);
            }
            {
                string txt = "Show/Hide View Cube";
                ActionCommand cmd = new ActionCommand(action => OnViewCubeToogle(), canExecute => true, (b) => {
                    OnFocus(b ? txt : null);
                });
                Scene2DButtonModel buttonModelGrid = new Scene2DButtonModel("pack://application:,,,/Scene3DRes;component/res/viewCube.png", txt, cmd, Tools.Count);
                Tools.Add(buttonModelGrid);
            }
            {
                string txt = "Show/Hide SkyBox";
                ActionCommand cmd = new ActionCommand(action => OnViewSkyBoxToogle(), canExecute => true, (b) => {
                    OnFocus(b ? txt : null);
                });
                Scene2DButtonModel buttonModelGrid = new Scene2DButtonModel("pack://application:,,,/Scene3DRes;component/res/skybox.png", txt, cmd, Tools.Count);
                Tools.Add(buttonModelGrid);
            }
        }

        double panoramaCube3D_OrgSize = -99.9;
        private void OnViewSkyBoxToogle()
        {
            //this.currentScene3D.panoramaCube3D.AnimateOpacity(0.0, 3.3);
            if (panoramaCube3D_OrgSize < -9)
                panoramaCube3D_OrgSize = this.currentScene3D.panoramaCube3D.Size;

            this.currentScene3D.panoramaCube3D.Size = (this.currentScene3D.panoramaCube3D.Size < 0.1) ? panoramaCube3D_OrgSize : 0.0001;
        }
        private void OnCameraInfoToogle()
        {
            this.currentScene3D.viewport.ShowCameraInfo = this.currentScene3D.viewport.ShowCameraInfo ? false : true;
        }
        private void OnViewCubeToogle()
        {
            this.currentScene3D.viewport.ShowViewCube = this.currentScene3D.viewport.ShowViewCube ? false : true;
        }
        private void OnGridToogle()
        {
            this.currentScene3D.grid.Visible = this.currentScene3D.grid.Visible ? false : true;
        }

        private void OnFocus(string txt)
        {
            this.OnFocusAction.Invoke(txt);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //MessageBox.Show(this.Viewport.ToString());
            //Debug.WriteLine("this.Viewport:");
            //Debug.WriteLine(this.Viewport);
        }

        public ObservableCollection<Scene3DViewModelLib.Scene2DButtonModel> Tools
        {
            get
            {
                return (ObservableCollection<Scene3DViewModelLib.Scene2DButtonModel>)GetValue(ToolsProperty);
            }
            set
            {
                SetValue(ToolsProperty, value);
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
        public Action<string> OnFocusAction
        {
            get
            {
                return (Action<string>)GetValue(OnFocusActionProperty);
            }
            set
            {
                SetValue(OnFocusActionProperty, value);
            }
        }
    }
}
