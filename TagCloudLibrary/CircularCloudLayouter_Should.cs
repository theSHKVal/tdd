using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagCloudLibrary
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        public static IEnumerable<TestCaseData> GetDataForCenterPointCheck
        {
            get
            {
                yield return new TestCaseData(new Point(-1, 1));
                yield return new TestCaseData(new Point(1, -1));
                yield return new TestCaseData(new Point(-1, -1));
            }
        }

        [TestCaseSource(nameof(GetDataForCenterPointCheck))]
        public static void ConstructorThrowsException_IfCenterPointIsIncorrect(Point center)
        {
            Action construct = () => new CircularCloudLayouter(center);
            construct.ShouldThrow<ArgumentException>();
        }

        public static IEnumerable<TestCaseData> GetDataForOutOfBoundsCheck
        {
            get
            {
                yield return new TestCaseData(new Point(1, 1), new Size(5, 5));
                yield return new TestCaseData(new Point(3, 3), new Size(8, 4));
                yield return new TestCaseData(new Point(4, 4), new Size(5, 10));
            }
        }

        [TestCaseSource(nameof(GetDataForOutOfBoundsCheck))]
        public void PutNextRectangleThrowsException_WhenOutOfBounds(Point centerPoint, Size size)
        {
            var cloud = new CircularCloudLayouter(centerPoint);
            cloud.Invoking(c => c.PutNextRectangle(size)).ShouldThrow<ArgumentOutOfRangeException>();
        }

        private readonly Point center = new Point(100, 100);
        private readonly List<Size> sizes = new List<Size>
        {
            new Size(30, 10), //center
            new Size(10, 5), new Size(10, 5), new Size(10, 5), new Size(5, 5), //top
            new Size(10, 5), new Size(5, 10), //right
            new Size(10, 10), new Size(10, 5), new Size(15, 5), //bottom
            new Size(10, 5), new Size(15, 15) // left
        };

        private void PutRectangles(ITagCloudLayouter cloud, int count)
        {
            for (var i = 0; i < count; i++)
                cloud.PutNextRectangle(sizes[i]);
        }

        [Test]
        public void PutNextRectangleAddsFirstRectanglesToCenter()
        {
            var cloud = new CircularCloudLayouter(center);
            var rectangle = cloud.PutNextRectangle(sizes.First());

            rectangle.Left.Should().Be(85);
            rectangle.Right.Should().Be(115);
            rectangle.Top.Should().Be(95);
            rectangle.Bottom.Should().Be(105);
        }

        [Test]
        public void PutNextRectangleAddsTwoRectanglesCorrectly()
        {
            var cloud = new CircularCloudLayouter(center);
            cloud.PutNextRectangle(sizes.First());
            var secondRectangle = cloud.PutNextRectangle(sizes.Skip(1).First());

            secondRectangle.Left.Should().Be(85);
            secondRectangle.Right.Should().Be(95);
            secondRectangle.Top.Should().Be(90);
            secondRectangle.Bottom.Should().Be(95);
        }

        [Test]
        public void PutNextRectangleFillsTopSideCorrectly()
        {
            var cloud = new CircularCloudLayouter(center);
            PutRectangles(cloud, 3);
            var lastTopRectangle = cloud.PutNextRectangle(new Size(10, 5));

            lastTopRectangle.Left.Should().Be(105);
            lastTopRectangle.Right.Should().Be(115);
            lastTopRectangle.Top.Should().Be(90);
            lastTopRectangle.Bottom.Should().Be(95);
        }

        [Test]
        public void PutNextRectangleFillsAllSidesCorrectly()
        {
            var cloud = new CircularCloudLayouter(center);
            PutRectangles(cloud, 11);
            var lastRectangle = cloud.PutNextRectangle(sizes.Last());

            lastRectangle.Left.Should().Be(70);
            lastRectangle.Right.Should().Be(85);
            lastRectangle.Top.Should().Be(85);
            lastRectangle.Bottom.Should().Be(100);
        }

        [Test]
        public void PutNextRectangleCorrectlyAddsFirstRectangle_AfterNewPivot()
        {
            var cloud = new CircularCloudLayouter(center);
            PutRectangles(cloud, 12);
            var lastRectangle = cloud.PutNextRectangle(new Size(10, 5));

            lastRectangle.Left.Should().Be(70);
            lastRectangle.Right.Should().Be(80);
            lastRectangle.Top.Should().Be(80);
            lastRectangle.Bottom.Should().Be(85);
        }
    }
}
