using HelixToolkit.Wpf.SharpDX;
using MSL.Core;
using MSL.Process.Wpf_Project1.Process.Shared;
using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using MeshGeometry3D = HelixToolkit.Wpf.SharpDX.MeshGeometry3D;

namespace MSL.Process.Wpf_Project1.Models
{
    public class BoardSquare : ModelBase
    {

        #region Bindable Properties
        private Square _Square;
        public Square Square
        {
            get { return _Square; }
            set { SetProperty(ref _Square, value); }
        }

        private bool _MoveSquare;
        public bool MoveSquare
        {
            get { return _MoveSquare; }
            set
            {
                SetProperty(ref _MoveSquare, value);
                if (value)
                {
                    Material = SelctionColour;
                }
                else
                {
                    Material = InitialMaterial;
                }

            }
        }
        #endregion

        #region Public Properties
        public override PhongMaterial SelctionColour { get; set; } = PhongMaterials.PolishedGold;
        #endregion

        #region Private Properties

        #endregion

        public BoardSquare(bool white, Square square)
        {
            if (white)
                Material = InitialMaterial = PhongMaterials.White;
            else
                Material = InitialMaterial = PhongMaterials.Black;

            int squareCentreX = SquareCentreX(square.X);
            int squareCentreZ = SquareCentreZ(square.Z);

            var b2 = new MeshBuilder(true, true, true);
            b2.AddBox(new Vector3(squareCentreX, -0.5f, squareCentreZ), 30, 1, 30, BoxFaces.All);
            Geometry = b2.ToMeshGeometry3D();

            Square = square;
        }

        public static int SquareCentreX (int x)
        {
            return SharedConstants.SquareSize / 2 + SharedConstants.SquareSize * x;
        }
        public static int SquareCentreZ(int z)
        {
            return SharedConstants.SquareSize / 2 + SharedConstants.SquareSize * z;
        }

        private void notify(object Sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsSelected")
            {

            }

        }
    }
}
