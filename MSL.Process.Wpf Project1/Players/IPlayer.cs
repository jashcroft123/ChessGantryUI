using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSL.Process.Wpf_Project1.Players
{
    public interface IPlayer
    {
        bool White { get; set; }
        Task FindMoves(string selectedSquare);

    }
}
