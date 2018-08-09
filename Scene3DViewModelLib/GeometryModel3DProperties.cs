
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace Scene3DViewModelLib
{
    public static class GeometryModel3DProperties
    {
        public static readonly DependencyProperty OrgMaterialProperty = DependencyProperty.RegisterAttached(
          "OrgMaterial",
          typeof(Material),
          typeof(GeometryModel3D)
        );
        public static readonly DependencyProperty OrgBackMaterialProperty = DependencyProperty.RegisterAttached(
          "OrgBackMaterial",
          typeof(Material),
          typeof(GeometryModel3D)
        );

        public static readonly DependencyProperty MaterialChangedProperty = DependencyProperty.RegisterAttached(
          "MaterialChanged",
          typeof(Boolean),
          typeof(GeometryModel3D)
        );

        public static void SetMaterialChanged(GeometryModel3D element, Boolean value)
        {
            element.SetValue(MaterialChangedProperty, value);
        }

        public static void SetOrgMaterial(GeometryModel3D element, Material value)
        {
            element.SetValue(OrgMaterialProperty, value);
        }
        public static Material OrgMaterial(GeometryModel3D element)
        {
            return (Material)element.GetValue(OrgMaterialProperty);
        }

        public static void SetOrgBackMaterial(GeometryModel3D element, Material value)
        {
            element.SetValue(OrgBackMaterialProperty, value);
        }
        public static Material OrgBackMaterial(GeometryModel3D element)
        {
            return (Material)element.GetValue(OrgBackMaterialProperty);
        }
    }
}
