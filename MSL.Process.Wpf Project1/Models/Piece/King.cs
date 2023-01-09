using HelixToolkit.Wpf.SharpDX;
using MSL.Process.Wpf_Project1.Game;
using MSL.Process.Wpf_Project1.Models.Piece.Moves;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using MeshGeometry3D = HelixToolkit.Wpf.SharpDX.MeshGeometry3D;
using Quaternion = System.Windows.Media.Media3D.Quaternion;

namespace MSL.Process.Wpf_Project1.Models.Piece
{
    public class King : PieceBase
    {
        FEN FEN;

        protected override List<MoveDirection> _Directions { get; set; } = new List<MoveDirection>
        {
            MoveDirection.N,
            MoveDirection.S,
            MoveDirection.E,
            MoveDirection.W,
            MoveDirection.NW,
            MoveDirection.NE,
            MoveDirection.SW,
            MoveDirection.SE
        };

        public King(bool white, Square square, FEN fen) : base(white, square)
        {
            FEN = fen;
            Rotate(90);


            string notation = "k";
            if (white)
            {
                notation.ToUpper();
            }
            Notation = notation;
        }

        public override async Task<List<MoveBase>> FindMoves(Pieces pieces)
        {
            List<MoveBase> validMoves = new List<MoveBase>();

            _ = FEN.BlackCastleKingSide;
            //Can move forward?
            validMoves.AddRange(await FindLegalMoves(pieces, _Directions, 1, true));

            return Moves = validMoves;
        }
    }
}