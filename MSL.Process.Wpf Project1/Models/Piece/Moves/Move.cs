using MSL.Process.Wpf_Project1.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSL.Process.Wpf_Project1.Models.Piece.Moves
{
    public class Move : MoveBase
    {
        public override async Task Execute(FEN fen)
        {
            PieceBase piece = fen.Pieces.First(x=>x.Square == Start);
            fen.Pieces.Remove(piece);
            piece.Square = Finish;
            fen.Pieces.Add(piece);
            await base.MoveEnd(fen);
            PGN = $"{piece.Notation}{Finish.StringValue}";
        }

        public override async Task Undo(FEN fen)
        {
            PieceBase piece = fen.Pieces.First(x => x.Square == Finish);
            fen.Pieces.Remove(piece);
            piece.Square = Start;
            fen.Pieces.Add(piece);
            await base.MoveEnd(fen);
        }
    }
}
