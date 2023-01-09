using HelixToolkit.Wpf.SharpDX;
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
    public class Knight : PieceBase
    {
        public Knight(bool white, Square square) : base(white, square)
        {
            string notation = "n";
            if (white)
            {
                notation.ToUpper();
            }
            else
            {
                Rotate(180);
            }
            Notation = notation;
        }

        public override async Task<List<MoveBase>> FindMoves(Pieces pieces)
        {
            List<MoveBase> validMoves = new List<MoveBase>();

            List<Square> testSquares = new List<Square>()
            {
                Square + (1,2),
                Square + (2,1),
                Square + (-1,2),
                Square + (-2,1),
                Square + (1,-2),
                Square + (2,-1),
                Square + (-1,-2),
                Square + (-2,-1)
            };

            foreach (Square testSquare in testSquares)
            {
                if (testSquare.X < 0 || testSquare.X > 7 || testSquare.Z < 0 || testSquare.Z > 7)
                {
                    continue;
                }

                if (!pieces.Any(x => x.Square == testSquare))
                {
                    //no piece in square can move there add it as a move
                    Move move = new Move();
                    move.Start = Square;
                    move.Finish = testSquare;
                    validMoves.Add(move);
                }

                else if (pieces.First(x => x.Square == testSquare).White != White)
                {
                    //Taking at the end of the move
                    Take take = new Take();
                    take.Start = Square;
                    take.Finish = testSquare;
                    validMoves.Add(take);
                }
            }
            return Moves = validMoves;
        }
    }
}