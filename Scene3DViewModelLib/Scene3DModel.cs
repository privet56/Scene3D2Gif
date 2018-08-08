using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace Scene3DViewModelLib
{
    public class Scene3DModel : BaseViewModel
    {
        public Scene3DModel()
        {

        }
        public Scene3DModel(string scene3DObj, ICommand command)
        {
            this.Scene3DObj = scene3DObj;
            this.Command = command;
        }

        protected string m_Scene3DObj;
        protected ICommand m_Command;

        public string Scene3DObj
        {
            get => this.m_Scene3DObj;
            set
            {
                this.m_Scene3DObj = value;
                NotifyPropertyChanged();
            }
        }
        public ICommand Command
        {
            get => this.m_Command;
            set
            {
                this.m_Command = value;
                NotifyPropertyChanged();
            }
        }
    }

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private IList<Model3D> selectedModels;
        private IList<Visual3D> selectedVisuals;

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public MainWindowViewModel(Viewport3D viewport)
        {
            this.RectangleSelectionCommand = new RectangleSelectionCommand(viewport, this.HandleSelectionModelsEvent, this.HandleSelectionVisualsEvent);
            this.PointSelectionCommand = new PointSelectionCommand(viewport, this.HandleSelectionModelsEvent, this.HandleSelectionVisualsEvent);
        }

        public RectangleSelectionCommand RectangleSelectionCommand { get; private set; }

        public PointSelectionCommand PointSelectionCommand { get; private set; }

        public SelectionHitMode SelectionMode
        {
            get
            {
                return this.RectangleSelectionCommand.SelectionHitMode;
            }

            set
            {
                this.RectangleSelectionCommand.SelectionHitMode = value;
            }
        }

        public IEnumerable<SelectionHitMode> SelectionModes
        {
            get
            {
                return Enum.GetValues(typeof(SelectionHitMode)).Cast<SelectionHitMode>();
            }
        }

        public string SelectedVisuals
        {
            get
            {
                return selectedVisuals == null ? "" : string.Join("; ", selectedVisuals.Select(x => x.GetType().Name));
            }
        }

        private void HandleSelectionVisualsEvent(object sender, VisualsSelectedEventArgs args)
        {
            this.selectedVisuals = args.SelectedVisuals;
            RaisePropertyChanged(nameof(SelectedVisuals));
        }
        private void HandleSelectionModelsEvent(object sender, ModelsSelectedEventArgs args)
        {
            this.ChangeMaterial(this.selectedModels, Materials.Blue/*null //TODO: reset Material instead of setting Blue! */);

            this.selectedModels = args.SelectedModels;
            var rectangleSelectionArgs = args as ModelsSelectedByRectangleEventArgs;
            if (rectangleSelectionArgs != null)
            {
                this.ChangeMaterial(
                    this.selectedModels,
                    rectangleSelectionArgs.Rectangle.Size != default(Size) ? Materials.Red : Materials.Green);
            }
            else
            {
                this.ChangeMaterial(this.selectedModels, Materials.Orange);
            }
        }

        private void ChangeMaterial(IEnumerable<Model3D> models, Material material)
        {
            if (models == null)
            {
                return;
            }

            foreach (var model in models)
            {
                var geometryModel = model as GeometryModel3D;
                if (geometryModel != null)
                {
                    geometryModel.Material = material;
                    geometryModel.BackMaterial = material;
                }
            }
        }
    }
}
