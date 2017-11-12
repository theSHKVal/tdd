using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagCloudLibrary
{
    [TestFixture]
    public class PivotRectangle_Should
    {
        private PivotRectangle pivot;

        [SetUp]
        public void SetUp()
        {
            pivot = new PivotRectangle();
        }

        [Test]
        public void CalculateNewPivotMakesTheSameRectangle_IfItIsTheOnlyOne()
        {
            var cloud = new List<Rectangle> {new Rectangle(new Point(100, 100), new Size(50, 50))};
            pivot.CalculateNewPivot(cloud);

            pivot.Rectangle.Top.Should().Be(100);
            pivot.Rectangle.Bottom.Should().Be(150);
            pivot.Rectangle.Right.Should().Be(150);
            pivot.Rectangle.Left.Should().Be(100);
        }

        [Test]
        public void CalculateNewPivotMakesCorrectRectangleOverCloud()
        {
            var cloud = new List<Rectangle>
            {
                new Rectangle(new Point(90, 100), new Size(50, 50)),
                new Rectangle(new Point(150, 100), new Size(70, 50)),
                new Rectangle(new Point(100, 150), new Size(50, 50)),
                new Rectangle(new Point(150, 150), new Size(50, 60))
            };
            pivot.CalculateNewPivot(cloud);

            pivot.Rectangle.Top.Should().Be(100);
            pivot.Rectangle.Bottom.Should().Be(210);
            pivot.Rectangle.Right.Should().Be(220);
            pivot.Rectangle.Left.Should().Be(90);
        }

        [Test]
        public void FillSideWorksCorrectly()
        {
            var cloud = new List<Rectangle> { new Rectangle(new Point(100, 100), new Size(50, 50)) };
            pivot.CalculateNewPivot(cloud);
            pivot.FillSide(pivot.Rectangle.Width + 1);
            pivot.FillSide(pivot.Rectangle.Height + 1);
            pivot.FillSide(pivot.Rectangle.Width + 1);

            pivot.CurrentSide.Should().Be(Side.Left);
        }
    }
}