using HelixToolkit.Wpf.SharpDX;
using MSL.Process.Wpf_Project1.Game;
using MSL.Process.Wpf_Project1.Models.Piece.Moves;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class Pawn : PieceBase
    {
        public MoveDirection MoveDirection { get; set; }

        public Pawn(bool white, Square square) : base(white, square)
        {
            //use move direction to calculate legal moves
            MoveDirection = White ? MoveDirection.N : MoveDirection.S;

            string notation = "";
            if (white)
            {
                notation.ToUpper();
            }
            Notation = notation;
        }

        #region Public Properties
        public override async Task<List<MoveBase>> FindMoves(Pieces pieces)
        {
            List<MoveBase> validMoves = new List<MoveBase>();

            char startRank = White ? '2' : '7';
            bool onStartRank = Square.StringValue[1] == startRank;

            //if we are in the starting square of the pawn the can move 2 spaces
            int NumberOfMovesForward = onStartRank ? 2 : 1;

            //Can move forward?
            validMoves.AddRange(await FindLegalMoves(pieces, MoveDirection, NumberOfMovesForward, false));

            validMoves.AddRange(await PawnTake(pieces));
            //need to add check if en passant just happened

            return Moves = validMoves;
        }
        #endregion

        #region Private Properties
        /// <summary>
        /// Requires special implementation because it's a bit of a strange set of rules
        /// </summary>
        /// <param name="pieces"></param>
        /// <returns></returns>
        private async Task<List<Take>> PawnTake(Pieces pieces)
        {
            //Can this be reworked to be a bit nicer?
            List<Take> moves = new List<Take>();

            int direction = 1;
            if (MoveDirection == MoveDirection.S)
                direction = -1;

            var testSquare = Square + (direction, 1);
            if (pieces.Any(x => x.Square == testSquare))
            {
                if (pieces.First(x => x.Square == testSquare).White != White)
                {
                    Take take = new Take();
                    take.Start = Square;
                    take.Finish = Square + (direction, 1);
                    moves.Add(take);
                }
            }

            testSquare = Square + (direction, -1);
            if (pieces.Any(x => x.Square == testSquare))
            {
                if (pieces.First(x => x.Square == testSquare).White != White)
                {
                    Take take = new Take();
                    take.Start = Square;
                    take.Finish = Square + (direction, -1);
                    moves.Add(take);
                }
            }

            //TODO need to check for enpassant take
            return moves;
        }
        #endregion
    }
}