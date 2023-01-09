using HelixToolkit.Wpf.SharpDX;
using MSL.Core;
using MSL.Process.Wpf_Project1.Models;
using MSL.Process.Wpf_Project1.Models.Piece;
using MSL.Process.Wpf_Project1.Models.Piece.Moves;
using MSL.Process.Wpf_Project1.Process.Shared;
using Prism.Commands;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MSL.Process.Wpf_Project1.Game
{
    /// <summary>
    /// Hold all the game related things
    /// Eg piece and the board
    /// </summary>
    public class GameManager : MslBindableBase
    {
        #region Bindable Properties
        private Board _Board;
        public Board Board
        {
            get { return _Board; }
            set { SetProperty(ref _Board, value); }
        }

        private FEN _FEN;
        public FEN FEN
        {
            get { return _FEN; }
            set { SetProperty(ref _FEN, value); }
        }

        private bool _WhiteToMove;
        public bool WhiteToMove
        {
            get { return _WhiteToMove; }
            set { SetProperty(ref _WhiteToMove, value); }
        }

        private ObservableCollection<MoveBase> _CompletedMoves = new ObservableCollection<MoveBase>();
        public ObservableCollection<MoveBase> CompletedMoves
        {
            get { return _CompletedMoves; }
            set { SetProperty(ref _CompletedMoves, value); }
        }

        private ObservableCollection<MoveBase> _UndoedMoves = new ObservableCollection<MoveBase>();
        public ObservableCollection<MoveBase> UndoedMoves
        {
            get { return _UndoedMoves; }
            set { SetProperty(ref _UndoedMoves, value); }
        }
        #endregion

        #region Delegate Commands
        public DelegateCommand UndoCommand { get; private set; }
        public DelegateCommand RedoCommand { get; private set; }

        #endregion

        #region Public Properties

        #endregion

        #region Private Properties
        #endregion


        public GameManager(FEN fEN)
        {
            FEN = fEN;
            FEN.NewFENLoaded += NewBoardLoaded;
            Board = new Board();
            FEN.LoadNewFEN(SharedConstants.StartingFENString).Await();

            UndoCommand = new DelegateCommand(async () => await Undo(), () => { return CompletedMoves?.Count > 0; }).ObservesProperty(() => CompletedMoves.Count);
            RedoCommand = new DelegateCommand(async () => await Redo(), () => { return UndoedMoves?.Count > 0; }).ObservesProperty(() => UndoedMoves.Count);
        }

        #region Public Methods
        public async Task PieceSelected(PieceBase piece)
        {
            //we have a couple consideration that need adding.
            // if we split these into before and after considerations
            // BEFORE: Will that move put us in check
            // AFTER: Was the move a 'Special move' eg was it a king and does the rook also need to be moved
            int numOfSelected = FEN.Pieces.Count(x => x.IsSelected == true);
            DeselectedAll();

            if (piece.White == FEN.WhiteToMove)
            {
                piece.IsSelected = true;
                var sudoValidMoves = await piece.FindMoves(FEN.Pieces);
                List<MoveBase> validMoves = new List<MoveBase>();
                validMoves = sudoValidMoves;
                
                //foreach (var sudoValidMove in sudoValidMoves)
                //{
                //    //validate move won't result in a check
                //    bool moveIsValid = true;

                //    await sudoValidMove.Execute(FEN);

                //    //TODO: Add recursion to this to validate the validation moves
                //    var nextMovePieces = FEN.Pieces.Where(x => x.White == FEN.WhiteToMove);
                //    foreach (var nextMovePiece in nextMovePieces)
                //    {
                //        //for all the opponent pieces check all possible moves
                //        var sudoValidFutureMoves = await nextMovePiece.FindMoves(FEN.Pieces);
                //        foreach (var sudoValidFutureMove in sudoValidFutureMoves)
                //        {
                //            //check if all possible takes for piece is the King, this would mean check
                //            if (sudoValidFutureMove is Take)
                //            {
                //                var sudoValidFutureTake = (Take)sudoValidFutureMove;
                //                await sudoValidFutureTake.Execute(FEN);
                //                var takenPiece = sudoValidFutureTake.TakenPiece;
                //                await sudoValidFutureTake.Undo(FEN);

                //                if (takenPiece is King)
                //                {
                //                    moveIsValid = false;
                //                    break;
                //                }
                //            }
                //        }

                //        if (!moveIsValid)
                //        {
                //            break;
                //        }
                //    }
                //    await sudoValidMove.Undo(FEN);

                //    if (moveIsValid)
                //    {
                //        validMoves.Add(sudoValidMove);
                //    }
                //}

                piece.Moves = validMoves;

                //Highlight the valid move squares
                var moveSquares = validMoves.Select(x => x.Finish);
                Board.Values.Where(x => moveSquares.Contains(x.Square)).ToList().ForEach(x => x.MoveSquare = true);
            }

            return;
        }

        public async Task SquareSelected(BoardSquare moveToSquare)
        {
            //Check if selected square can be moved to
            if (moveToSquare.MoveSquare)
            {
                //find selected piece and move it to the selected square
                var selectedPiece = FEN.Pieces.First(x => x.IsSelected == true);
                var move = await selectedPiece.Move(moveToSquare.Square, FEN);
                CompletedMoves.Add(move);
                UndoedMoves = new ObservableCollection<MoveBase>();
                //CompletedMoves = CompletedMoves;
            }
            DeselectedAll();
        }
        #endregion

        #region Protected Methods

        #endregion

        #region Private Methods
        private async Task Undo()
        {
            DeselectedAll();
            var move = CompletedMoves.Last();
            CompletedMoves.RemoveAt(CompletedMoves.Count - 1);
            await move.Undo(FEN);
            UndoedMoves.Add(move);
        }
        private async Task Redo()
        {
            DeselectedAll();
            var move = UndoedMoves.Last();
            UndoedMoves.RemoveAt(UndoedMoves.Count - 1);
            await move.Execute(FEN);
            CompletedMoves.Add(move);
        }
        private void NewBoardLoaded(object sender, EventArgs e)
        {

        }
        private void DeselectedAll()
        {
            FEN.Pieces.Select(x => { x.IsSelected = false; return x; }).ToList();
            Board.Values.Select(x => { x.MoveSquare = false; return x; }).ToList();
        }
        #endregion
    }
}
