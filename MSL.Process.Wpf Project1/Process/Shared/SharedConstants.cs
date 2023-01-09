using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace MSL.Process.Wpf_Project1.Process.Shared
{
    public static class SharedConstants
    {
        public const int SquareSize = 30;
        public const string StartingFENString = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

        public const double BoardCentre = SquareSize * 4;

        public static Point3D BoardCentreVector = new Point3D(BoardCentre, 0, BoardCentre);

        public static SynchronizationContext UIThreadContext { get; set; }
    }
}
