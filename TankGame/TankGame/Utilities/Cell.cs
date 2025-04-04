using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankGame.Utilities
{
    internal struct Cell
    {
        public int X, Y;
        public Cell(int x, int y) { X = x; Y = y; }
        public static bool operator ==(Cell left, Cell right)
        {
            return left.X == right.X && left.Y == right.Y;
        }
        public static bool operator !=(Cell left, Cell right)
        {
            return left.X != right.X && left.Y != right.Y;
        }
        public Cell Sum(Cell cell) { return new Cell(X + cell.X , Y + cell.Y); }
        public Cell Sub(Cell cell) { return new Cell(X - cell.X, Y - cell.Y); }
        public bool InRange(int x, int y)
        {
            return X <= x && Y <= y && X >= 0 && Y >= 0;
        }
        public Cell DirToPoint(Cell point)
        {
            if(X == point.X)
            {
                int dir = Y < point.Y ? 1 : -1;
                return new Cell(0, dir);
            }
            else if(Y == point.Y)
            {
                int dir = X < point.X ? 1 : -1;
                return new Cell(dir, 0);
            }
            return new Cell(0, 0);
        }
        public override bool Equals(object obj)
        {
            return obj is Cell c && c.X == X && c.Y == Y;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}
