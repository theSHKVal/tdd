using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagCloudLibrary
{
    internal class PivotRectangle
    {
        public Rectangle Rectangle { get; private set; }
        public bool AllSidesAreFilled { get; private set; }
        public int LengthToFill { get; private set; }
        public Side CurrentSide { get; private set; }

        public void CalculateNewPivot(List<Rectangle> cloud)
        {
            var left = cloud.Select(r => r.Left).Min();
            var right = cloud.Select(r => r.Right).Max();
            var top = cloud.Select(r => r.Top).Min();
            var bottom = cloud.Select(r => r.Bottom).Max();

            Rectangle = new Rectangle(new Point(left, top), new Size(right - left, bottom - top));
            AllSidesAreFilled = false;
            CurrentSide = Side.Top;
            LengthToFill = Rectangle.Width;
        }

        public void FillSide(int length)
        {
            LengthToFill -= length;
            if (LengthToFill < 0)
            {
                switch (CurrentSide)
                {
                    case Side.Top:
                    case Side.Bottom:
                        LengthToFill = Rectangle.Height;
                        break;
                    case Side.Right:
                        LengthToFill = Rectangle.Width;
                        break;
                    case Side.Left:
                        AllSidesAreFilled = true;
                        return;
                }
                CurrentSide = GetNextSideToFill();
            }
        }

        private Side GetNextSideToFill()
        {
            switch (CurrentSide)
            {
                case Side.Top: return Side.Right;
                case Side.Right: return Side.Bottom;
                case Side.Bottom: return Side.Left;
                default: throw new InvalidOperationException();
            }
        }
    }
}
