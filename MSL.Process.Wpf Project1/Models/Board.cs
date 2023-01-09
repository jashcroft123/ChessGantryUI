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
    public class Board : Dictionary<Square, BoardSquare>
    {
        public BillboardText3D LettersBillboard { get; set; } = new BillboardText3D();


        public Board()
        {
            for (int x = 0; x < 8; x++)
            {
                for (int z = 0; z < 8; z++)
                {
                    bool white = (x + z) % 2 == 1;
                    Square square = (x, z);
                    base.Add(square, new BoardSquare(white, square));

                    if (x == 0)
                        LettersBillboard.TextInfo.Add(new TextInfo($"{Char.ConvertFromUtf32(z + 65)}",
                                                      new Vector3(BoardSquare.SquareCentreX(x) - SharedConstants.SquareSize, 0, BoardSquare.SquareCentreZ(z))));

                    if (z == 0)
                        LettersBillboard.TextInfo.Add(new TextInfo($"{x + 1}",
                                                      new Vector3(BoardSquare.SquareCentreX(x), 0, BoardSquare.SquareCentreZ(z) - SharedConstants.SquareSize)));
                }
            }

            //    new Vector3(-(float)Math.Cos(Math.PI / 6) * offset, -(float)Math.Sin(Math.PI / 6) * offset, 0))
            //{
            //    Foreground = Color.White,
            //    Scale = scale
            //});
        }

        private void notify(object Sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsSelected")
            {

            }
        }
    }
}
