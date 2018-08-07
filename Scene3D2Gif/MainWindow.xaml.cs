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

namespace Scene3D2Gif
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //public ObservableCollection<string> insertedElements = new ObservableCollection<string>();

        public static readonly DependencyProperty Scene3DElesProperty =
         DependencyProperty.Register("Scene3DEles",
         typeof(ObservableCollection<string>),
         typeof(MainWindow),
         new UIPropertyMetadata(null));


        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            Scene3DEles = new ObservableCollection<string>();
        }
        public ObservableCollection<string> Scene3DEles
        {
            get
            {
                return (ObservableCollection<string>)GetValue(Scene3DElesProperty);
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
                return new ActionCommand(action => OnInsert(), canExecute => true);
            }
        }
        public ICommand InsertAgainCommand
        {
            get
            {
                return new ActionCommand(action => OnInsertAgain(), canExecute => true);
            }
        }
        public void OnInsertAgain()
        {
            MessageBox.Show("oops I did it again!");
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
            var _previousCursor = Mouse.OverrideCursor;
            Mouse.OverrideCursor = Cursors.Wait;

            ModelVisual3D device = new ModelVisual3D();
            device.Content = Scene3DLib.Scene3D.getModel(openFileDialog.FileName);
            this.helixViewport3D.Children.Add(device);
            Scene3DEles.Add(openFileDialog.FileName);
            Mouse.OverrideCursor = _previousCursor;
        }
    }
}
