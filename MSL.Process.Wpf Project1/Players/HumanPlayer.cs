using MSL.Process.Wpf_Project1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSL.Process.Wpf_Project1.Players
{
    /// <summary>
    /// This is a UI user and therefore we want to wait for an input
    /// </summary>
    public class HumanPlayer : PlayerBase
    {

        #region Bindable Properties

        #endregion

        #region Public Properties

        #endregion

        #region Private Properties

        #endregion





        public HumanPlayer(bool white) : base(white)
        {

        }

        public override async Task FindMoves(string selectedSquare)
        {

        }
    }
}
