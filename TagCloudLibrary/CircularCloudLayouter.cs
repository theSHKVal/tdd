using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagCloudLibrary
{
    public class CircularCloudLayouter : ITagCloudLayouter
    {
        private readonly List<Rectangle> cloud;
        private readonly Point center;
        private readonly PivotRectangle pivotRectangle;

        public CircularCloudLayouter(Point center)
        {
            cloud = new List<Rectangle>();
            pivotRectangle = new PivotRectangle();
            if (center.X < 0 || center.Y < 0)
            {
                throw new ArgumentException();
            }
            this.center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            Rectangle rectangle;
            if (!cloud.Any())
            {
                rectangle = PutFirstRectangle(rectangleSize);
                cloud.Add(rectangle);
                pivotRectangle.CalculateNewPivot(cloud);
                return rectangle;
            }

            if (pivotRectangle.AllSidesAreFilled) pivotRectangle.CalculateNewPivot(cloud);
            rectangle = PlaceOnSide(rectangleSize, pivotRectangle.CurrentSide);
            cloud.Add(rectangle);
            return rectangle;
        }

        private Rectangle PutFirstRectangle(Size size)
        {
            var rectangle = new Rectangle(new Point(center.X - size.Width / 2, center.Y - size.Height / 2), size);
            if (RectangleIsOutOfBounds(rectangle)) throw new ArgumentOutOfRangeException();
            return rectangle;
        }

        private Rectangle PlaceOnSide(Size size, Side side)
        {
            Rectangle rectangle;
            switch (side)
            {
                case Side.Top:
                    rectangle = new Rectangle(new Point(pivotRectangle.Rectangle.Right - pivotRectangle.LengthToFill,
                                                        pivotRectangle.Rectangle.Top - size.Height), size);
                    pivotRectangle.FillSide(size.Width);
                    break;
                case Side.Right:
                    rectangle = new Rectangle(new Point(pivotRectangle.Rectangle.Right,
                                                        pivotRectangle.Rectangle.Bottom - pivotRectangle.LengthToFill), size);
                    pivotRectangle.FillSide(size.Height);
                    break;
                case Side.Bottom:
                    rectangle = new Rectangle(new Point(pivotRectangle.Rectangle.Left + pivotRectangle.LengthToFill - size.Width,
                                                        pivotRectangle.Rectangle.Bottom), size);
                    pivotRectangle.FillSide(size.Width);
                    break;
                case Side.Left:
                    rectangle = new Rectangle(new Point(pivotRectangle.Rectangle.Left - size.Width,
                                                        pivotRectangle.Rectangle.Top + pivotRectangle.LengthToFill - size.Height), size);
                    pivotRectangle.FillSide(size.Height);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(side), side, "Invalid side value: " + side);
            }
            if (RectangleIsOutOfBounds(rectangle)) throw new ArgumentOutOfRangeException("Next object is out of bounds", null as Exception);
            return rectangle;
        }  

        private static bool RectangleIsOutOfBounds(Rectangle rectangle)
        {
            return rectangle.X < 0 || rectangle.Y < 0;
        }
    }
}
