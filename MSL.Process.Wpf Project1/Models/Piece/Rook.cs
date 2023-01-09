﻿using HelixToolkit.Wpf.SharpDX;
using MSL.Process.Wpf_Project1.Models.Piece.Moves;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using MeshGeometry3D = HelixToolkit.Wpf.SharpDX.MeshGeometry3D;
using Quaternion = System.Windows.Media.Media3D.Quaternion;

namespace MSL.Process.Wpf_Project1.Models.Piece
{
    public class Rook : PieceBase
    {
        protected override List<MoveDirection> _Directions { get; set; } = new List<MoveDirection>
        {
            MoveDirection.N,
            MoveDirection.S,
            MoveDirection.E,
            MoveDirection.W
        };

        public Rook(bool white, Square square) : base(white, square)
        {
            string notation = "r";
            if (white)
            {
                notation.ToUpper();
            }
            Notation = notation;
        }
    }
}