using HelixToolkit.Wpf.SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models.Gantry
{
    public class XAxis : AxisBase
    {
        public XAxis()
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
