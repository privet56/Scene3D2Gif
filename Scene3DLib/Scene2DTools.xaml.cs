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

        public static readonly DependencyProperty ViewportProperty =
         DependencyProperty.Register("Viewport",
         typeof(HelixViewport3D),
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

            {
                string txt = "Grid On/Off";
                ActionCommand cmd = new ActionCommand(action => OnGridToogle(), canExecute => true, (b) => {
                    OnFocus(b ? txt : null);
                });
                Scene2DButtonModel buttonModelGrid = new Scene2DButtonModel("pack://application:,,,/Scene3DRes;component/res/grid.png", txt, cmd);
                Tools.Add(buttonModelGrid);
            }
        }

        private void OnGridToogle()
        {
            
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
        public HelixViewport3D Viewport
        {
            get
            {
                return (HelixViewport3D)GetValue(ViewportProperty);
            }
            set
            {
                SetValue(ViewportProperty, value);
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
