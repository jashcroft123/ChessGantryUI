using MSL.Process.Wpf_Project1.Game;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MSL.Process.Wpf_Project1.Models.Piece.Moves
{
    public class Take : MoveBase
    {
        public PieceBase TakenPiece;

        public override async Task Execute(FEN fen)
        {
            PieceBase takenPiece = fen.Pieces.First(x => x.Square == Finish);
            fen.Pieces.Remove(takenPiece);
            TakenPiece = takenPiece;

            PieceBase piece = fen.Pieces.First(x => x.Square == Start);
            fen.Pieces.Remove(piece);
            piece.Square = Finish;
            fen.Pieces.Add(piece);
            await base.MoveEnd(fen);
            PGN = $"{Start.StringValue[0]}{piece.Notation}x{Finish.StringValue}";
        }

        public override async Task Undo(FEN fen)
        {
            PieceBase piece = fen.Pieces.First(x => x.Square == Finish);
            fen.Pieces.Remove(piece);
            piece.Square = Start;
            fen.Pieces.Add(piece);
            fen.Pieces.Add(TakenPiece);
            await base.MoveEnd(fen);
        }
    }
}
