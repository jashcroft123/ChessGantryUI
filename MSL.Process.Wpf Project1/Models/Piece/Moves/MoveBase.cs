using MSL.Process.Wpf_Project1.Game;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSL.Process.Wpf_Project1.Models.Piece.Moves
{
    public abstract class MoveBase : BindableBase
    {
        public Square Start { get; set; }
        public Square Finish { get; set; }
        public bool White { get; set; }

        public string PGN { get; set; }

        public abstract Task Execute(FEN fen);

        public abstract Task Undo(FEN fen);

        /// <summary>
        /// Common functionality at the end of any move type
        /// </summary>
        /// <param name="fEN"></param>
        /// <returns></returns>
        protected async Task MoveEnd(FEN fEN)
        {
            fEN.WhiteToMove = !fEN.WhiteToMove;
        }
    }
}
