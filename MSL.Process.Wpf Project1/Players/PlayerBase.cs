using MSL.Core;
using MSL.Process.Wpf_Project1.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSL.Process.Wpf_Project1.Players
{
    public abstract class PlayerBase : MslBindableBase
    {

        #region Bindable Properties
        private Pieces _Pieces;
        public Pieces Pieces
        {
            get { return _Pieces; }
            set { SetProperty(ref _Pieces, value); }
        }

        private ObservableCollection<string> _Moves;
        public ObservableCollection<string> Moves
        {
            get { return _Moves; }
            set { SetProperty(ref _Moves, value); }
        }
        #endregion

        #region Public Properties
            public bool White { get; set; }
        #endregion

        #region Protected Properties

        #endregion

        #region Private Properties

        #endregion






        public PlayerBase(bool white)
        {
            White = white;
        }

        public abstract Task FindMoves(string selectedSquare);
    }
}
