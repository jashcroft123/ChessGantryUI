using HelixToolkit.Wpf.SharpDX;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using MeshGeometry3D = HelixToolkit.Wpf.SharpDX.MeshGeometry3D;

namespace MSL.Process.Wpf_Project1.Models
{
    public class YGantry : ModelBase
    {
        public YGantry()
        {
            Material = PhongMaterials.Silver;
            Material.ReflectiveColor = SharpDX.Color.DarkGray;
            Material.RenderEnvironmentMap = true;

            var m1 = LoadObj("Models/Objs/Extrusion.obj"); //Load obj's into model group
            Geometry = m1[0].Geometry as MeshGeometry3D;

            Rotate(90, "x");
            RelativeTranslate(y: 200);
        }
    }
}
