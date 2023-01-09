using Assimp;
using HelixToolkit.Wpf.SharpDX;
using HelixToolkit.Wpf.SharpDX.Assimp;
using Microsoft.Xaml.Behaviors;
using MSL.Core;
using MSL.Process.Wpf_Project1.Models;
using MSL.Process.Wpf_Project1.Models.Piece;
using MSL.Process.Wpf_Project1.Players;
using Prism.Ioc;
using Prism.Modularity;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MSL.Process.Wpf_Project1.Game
{
    public class FEN : MslBindableBase
    {
        #region Bindable Properties
        private bool _WhiteToMove;
        public bool WhiteToMove
        {
            get { return _WhiteToMove; }
            set { SetProperty(ref _WhiteToMove, value); }
        }

        private Pieces _Pieces;
        public Pieces Pieces
        {
            get { return _Pieces; }
            set { SetProperty(ref _Pieces, value); }
        }
        #endregion

        #region Public Properties

        public bool WhiteCastleQueenSide;
        public bool WhiteCastleKingSide;
        public bool BlackCastleQueenSide;
        public bool BlackCastleKingSide;

        public EventHandler NewFENLoaded { get; set; }
        #endregion

        #region Private Properties

        #endregion

        public async Task LoadNewFEN(string fenString)
        {
            List<string> fenList = fenString.Split(' ').ToList();
            List<string> piecesString = fenList[0].Split('/').Reverse().ToList();

            Pieces = await ImportPieces(piecesString);
            WhiteToMove = fenList[1].Contains("w");

            WhiteCastleQueenSide = fenList[2].Contains('K');
            WhiteCastleKingSide = fenList[2].Contains('Q');
            BlackCastleQueenSide = fenList[2].Contains('k');
            BlackCastleKingSide = fenList[2].Contains('q');

            NewFENLoaded.Invoke(this, new EventArgs());
        }

        private async Task<Pieces> ImportPieces(List<string> piecesString)
        {
            Pieces pieces = new Pieces();
            for (int x = 0; x < 8; x++)
            {
                int z = 0;
                foreach (char pieceString in piecesString[x].ToCharArray())
                {
                    if (!char.IsDigit(pieceString))
                    {
                        bool white = char.IsUpper(pieceString);
                        char pieceChar = char.ToLower(pieceString);

                        Square square = (x, z);
                        pieces.Add( await LoadPiece(pieceChar, white, square));
                    }
                    z++;
                }
            }
            return pieces;
        }

        private async Task<PieceBase> LoadPiece(char pieceString, bool white, Square square)
        {
            //have to use switch case as expressions aren't avaliable in #7.3 :(
            switch (pieceString)
            {
                case 'p':
                    return ContainerLocator.Current.Resolve<Pawn>((typeof(bool), white), (typeof(Square), square));

                case 'r':
                    return ContainerLocator.Current.Resolve<Rook>((typeof(bool), white), (typeof(Square), square));

                case 'n':
                    return ContainerLocator.Current.Resolve<Knight>((typeof(bool), white), (typeof(Square), square));

                case 'b':
                    return ContainerLocator.Current.Resolve<Bishop>((typeof(bool), white), (typeof(Square), square));

                case 'q':
                    return ContainerLocator.Current.Resolve<Queen>((typeof(bool), white), (typeof(Square), square));

                case 'k':
                    return ContainerLocator.Current.Resolve<King>((typeof(bool), white), (typeof(Square), square));

                default:
                    throw new Exception($"Unknown {pieceString} piece to import");
            }
        }
    }
}
