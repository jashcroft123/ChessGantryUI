using HelixToolkit.Wpf.SharpDX;
using MSL.Process.Wpf_Project1.Game;
using MSL.Process.Wpf_Project1.Models.Piece.Moves;
using MSL.Process.Wpf_Project1.Process.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSL.Process.Wpf_Project1.Models.Piece
{
    public abstract class PieceBase : ModelBase, IPiece
    {

        #region Bindable Properties
        private Square _Square;
        public Square Square
        {
            get { return _Square; }
            set { SetProperty(ref _Square, value); }
        }

        private bool _IsSelected;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                SetProperty(ref _IsSelected, value);
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
        public bool White { get; set; }
        public List<MoveBase> Moves { get; set; }
        public string Notation { get; set; }
        #endregion

        #region Protected Properties
        protected virtual List<MoveDirection> _Directions { get; set; }
        #endregion

        #region Private Properties

        #endregion


        public PieceBase(bool white, Square square)
        {
            base.PropertyChanged += PropChange;

            Square = square;

            White = white;
            if (White)
                Material = InitialMaterial = PhongMaterials.White;
            else
                Material = InitialMaterial = PhongMaterials.BlackPlastic;

            var model = LoadObj($"Models/STLs/{this.GetType().Name}.stl"); //Load obj's into model group
            Geometry = model[0].Geometry as MeshGeometry3D;

            Rotate(-90, "x"); //rotate model so it sits flat
        }

        #region Public Methods
        public virtual async Task<List<MoveBase>> FindMoves(Pieces pieces)
        {
            List<MoveBase> validMoves = new List<MoveBase>();

            validMoves.AddRange(await FindLegalMoves(pieces, _Directions));

            return Moves = validMoves;
        }

        public virtual async Task<MoveBase> Move(Square finish, FEN fen)
        {
            MoveBase move = Moves.First(x => x.Finish == finish);
            await move.Execute(fen);
            return move;
        }
        #endregion

        #region Protected Methods

        protected async Task<List<MoveBase>> FindLegalMoves(Pieces pieces, List<MoveDirection> directions, int moveLimit = 8, bool canTakeAtEnd = true)
        {
            List<MoveBase> validMoves = new List<MoveBase>();
            foreach (MoveDirection direction in directions)
            {
                validMoves.AddRange(await _LegalMovesInDirection(pieces, direction, moveLimit, canTakeAtEnd));
            }
            return validMoves;
        }
        protected async Task<List<MoveBase>> FindLegalMoves(Pieces pieces, MoveDirection direction, int moveLimit = 8, bool canTakeAtEnd = true)
        {
            List<MoveBase> validMoves = await _LegalMovesInDirection(pieces, direction, moveLimit, canTakeAtEnd);
            return validMoves;
        }
        #endregion

        #region Private Methods
        private async void PropChange(object Sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Square")
            {
                int squareCentreX = SharedConstants.SquareSize / 2 + SharedConstants.SquareSize * Square.X;
                int squareCentreZ = SharedConstants.SquareSize / 2 + SharedConstants.SquareSize * Square.Z;
                await AbsoluteTranslate(squareCentreX, 0, squareCentreZ);
            }
        }

        private async Task<List<MoveBase>> _LegalMovesInDirection(Pieces pieces, MoveDirection direction, int moveLimit, bool canTakeAtEnd)
        {
            List<MoveBase> validMoves = new List<MoveBase>();

            for (int i = 1; i <= moveLimit; i++)
            {
                var valueToMove = await DirectionToAdd(direction, i);
                Square testSquare = Square + valueToMove;
                if (testSquare.X < 0 || testSquare.X > 7 || testSquare.Z < 0 || testSquare.Z > 7)
                    break;

                if (!pieces.Any(x => x.Square == testSquare))
                {
                    //no piece in square can move there add it as a move
                    Move move = new Move();
                    move.Start = Square;
                    move.Finish = testSquare;
                    validMoves.Add(move);
                }
                else if (canTakeAtEnd)
                {
                    if (pieces.First(x => x.Square == testSquare).White != White)
                    {
                        //Taking at the end of the move
                        Take take = new Take();
                        take.Start = Square;
                        take.Finish = testSquare;
                        validMoves.Add(take);
                    }
                    break;
                }
                else
                {
                    break;
                }
            }
            return validMoves;
        }

        private async Task<(int x, int z)> DirectionToAdd(MoveDirection direction, int i)
        {
            #region Helper diagram
            /*
                 |
             +-  |  ++
                 |
            ------------
                 |
             --  |  -+
                 |
            */
            #endregion

            (int x, int z) addValue = (0, 0);
            switch (direction)
            {
                case MoveDirection.N:
                    addValue = (i, 0);
                    break;
                case MoveDirection.E:
                    addValue = (0, i);
                    break;
                case MoveDirection.S:
                    addValue = (-i, 0);
                    break;
                case MoveDirection.W:
                    addValue = (0, -i);
                    break;
                case MoveDirection.NE:
                    addValue = (i, i);
                    break;
                case MoveDirection.SE:
                    addValue = (-i, i);
                    break;
                case MoveDirection.SW:
                    addValue = (-i, -i);
                    break;
                case MoveDirection.NW:
                    addValue = (i, -i);
                    break;
            }
            return addValue;
        }
        #endregion
    }
}
