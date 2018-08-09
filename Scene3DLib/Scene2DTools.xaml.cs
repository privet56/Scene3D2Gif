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

        public Scene2DTools()
        {
            Tools = new ObservableCollection<Scene3DViewModelLib.Scene2DButtonModel>();

            InitializeComponent();

            Scene2DButtonModel buttonModelGrid = new Scene2DButtonModel("pack://application:,,,/Scene3DRes;component/res/grid.png", "Grid On/Off", null, null);
            Tools.Add(buttonModelGrid);
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
    }
}
