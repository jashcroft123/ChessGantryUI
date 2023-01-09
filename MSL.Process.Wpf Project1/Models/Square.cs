using MSL.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSL.Process.Wpf_Project1.Models
{
    /// <summary>
    /// Holds the location in both string and X Z coordinates
    /// see https://stackoverflow.com/questions/2582210/making-your-own-int-or-string-class
    /// </summary>
    public class Square : MslBindableBase
    {
        #region Bindable Properties
        private string _StringValue;
        /// <summary>
        /// Square name in string form. Updating this will set the integer values for X and Z
        /// </summary>
        public string StringValue
        {
            get { return _StringValue; }
            private set
            {
                SetProperty(ref _StringValue, value.ToUpper());
                var result = SquareToXZ(value);
                if(X != result.x)
                    X = result.x;
                if (Z != result.z)
                    Z = result.z;
            }
        }
        #endregion

        #region Public Properties
        private int _X;
        /// <summary>
        /// 0 referenced array for the letter of the square eg '1' == 0
        /// </summary>
        public int X
        {
            get { return _X; }
            private set 
            { 
                SetProperty(ref _X, value);
                var result = SquareToXZ(StringValue);
                if (result.x != value)
                    XZToSquare((value, Z));
            }
        }

        private int _Z;
        /// <summary>
        /// 0 referenced array for the letter of the square eg A == 0
        /// </summary>
        public int Z
        {
            get { return _Z; }
            private set
            {
                SetProperty(ref _Z, value);
                var result = SquareToXZ(StringValue);
                if (result.z != value)
                    XZToSquare((value, X));
            }
        }
        #endregion

        #region Private Properties

        #endregion

        #region Ctor
        private Square(string stringValue)
        {
            this.StringValue = stringValue;
        }
        private Square(int x, int z)
        {
            this.StringValue = XZToSquare((x, z));
        }
        #endregion

        #region Public Methods
        public static implicit operator Square(string value)
        {
            return new Square(value);
        }
        public static implicit operator Square((int x, int z) xz)
        {
            return new Square(xz.x, xz.z);
        }

        public static bool operator ==(Square a, Square b)
        {
            return Equals(a._StringValue, b._StringValue);
        }
        public static bool operator !=(Square a, Square b)
        {
            return !Equals(a._StringValue, b._StringValue);
        }

        public static bool operator ==(Square a, string b)
        {
            return Equals(a._StringValue, b.ToUpper());
        }
        public static bool operator !=(Square a, string b)
        {
            return !Equals(a._StringValue, b.ToUpper());
        }

        public static Square operator +(Square square, (int x, int z) add)
        {
            var xz = SquareToXZ((string)square);
            xz.x += add.x;
            xz.z += add.z;
            return XZToSquare(xz);
        }

        public override bool Equals(object a)
        {
            Square square = a as Square;
            return square.StringValue == StringValue;
        }
        /// <summary>
        /// Let compare using the stringvalue as seen at
        /// https://stackoverflow.com/questions/2389110/cannot-find-key-in-generic-dictionary
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return StringComparer.InvariantCulture.GetHashCode(StringValue) +
                   StringComparer.InvariantCulture.GetHashCode(StringValue);
        }
        public static explicit operator string(Square value)
        {
            return value.StringValue;
        }
        #endregion

        #region Protected Methods
        protected static string XZToSquare((int x, int z) xz)
        {
            return $"{Char.ConvertFromUtf32(xz.z + 65)}{(xz.x + 1)}";
        }

        protected static (int x, int z) SquareToXZ(string square)
        {
            var z = Encoding.UTF32.GetBytes(square.Substring(0))[0] - 65;
            var x = int.Parse(square.Substring(1)) - 1;
            return (x, z);
        }
        #endregion

        #region Private Methods
        #endregion
    }
}
