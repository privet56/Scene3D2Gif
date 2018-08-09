using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelixToolkit.Wpf;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace Scene3DViewModelLib
{
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
            this.ChangeMaterial(this.selectedModels, null/*Materials.Blue/*null //TODO: reset Material instead of setting Blue! */);

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
                    if (material != null)
                    {
                        Material orgMaterial = geometryModel.Material;
                        Material orgBackMaterial = geometryModel.BackMaterial;

                        GeometryModel3DProperties.SetMaterialChanged(geometryModel, true);
                        GeometryModel3DProperties.SetOrgMaterial(geometryModel, orgMaterial);
                        GeometryModel3DProperties.SetOrgBackMaterial(geometryModel, orgBackMaterial);

                        geometryModel.Material = material;
                        geometryModel.BackMaterial = material;
                    }
                    else
                    {
                        if(GeometryModel3DProperties.OrgMaterial(geometryModel) != null) geometryModel.Material = GeometryModel3DProperties.OrgMaterial(geometryModel);
                        if(GeometryModel3DProperties.OrgBackMaterial(geometryModel) != null)geometryModel.BackMaterial = GeometryModel3DProperties.OrgBackMaterial(geometryModel);
                    }
                }
            }
        }
    }
}
