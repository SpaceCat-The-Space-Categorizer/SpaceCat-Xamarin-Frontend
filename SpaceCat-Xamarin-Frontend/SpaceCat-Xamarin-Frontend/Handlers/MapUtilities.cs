using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace SpaceCat_Xamarin_Frontend
{
    /// <summary>
    ///     Contains helper functions for creating floor maps, and their unit tests.
    /// </summary>
    public static class MapUtilities
    {
        
        /// <summary>
        ///     Determines if the provided point is contained within the bounds of the shape created by
        ///     the provided point collection.
        /// </summary>
        /// <param name="points">The points of a shape.</param>
        /// <param name="pt">The point to check if contained in shape.</param>
        /// <returns>Returns true if provided point is within the bounds of the outer points.</returns>
        public static bool Contains(PointCollection points, Point pt)
        {
            if (points.Count > 0)
            {
                double lowX = points[0].X;
                double lowY = points[0].Y;
                double highX = -1;
                double highY = -1;
                foreach (Point figPt in points)
                {
                    if (figPt.X < lowX) lowX = figPt.X;
                    if (figPt.Y < lowY) lowY = figPt.Y;
                    if (figPt.X > highX) highX = figPt.X;
                    if (figPt.Y > highY) highY = figPt.Y;
                }
                if (pt.X >= lowX && pt.X <= highX && pt.Y >= lowY && pt.Y <= highY)
                    return true;
                else return false;
            }
            else return false;
        }

        /// <summary>
        ///     Determines if the provided point is contained within the bounds of the provided image.
        /// </summary>
        /// <param name="img">The image to check for containment.</param>
        /// <param name="pt">The point to check if contained in image.</param>
        /// <returns>Returns true if provided point is within the bounds of the image.</returns>
        public static bool ShapeContains(FurnitureShape img, Point pt)
        {
            if (img != null)
            {
                double lowX = img.Bounds.X;
                double lowY = img.Bounds.Y;
                double highX = lowX + img.Bounds.Width;
                double highY = lowY + img.Bounds.Height;

                if (pt.X >= lowX && pt.X <= highX && pt.Y >= lowY && pt.Y <= highY)
                    return true;
                else return false;
            }
            else return false;
        }


        /// <summary>
        ///     Converts a point collection into 4 doubles representing the top left x and y coordinates of 
        ///     the rectangle, and the bottom right x and y coordinates of the rectangle.
        /// </summary>
        /// <param name="points">The point collection to be converted to a double array.</param>
        /// <returns>An array of 4 doubles: a smallest x, a smallest y, a largest x, and a largest y.</returns>
        public static double[] ParsePoints(PointCollection points)
        {
            double[] result = new double[4] { points[0].X, points[0].Y, points[0].X, points[0].Y };
            foreach (Point pt in points)
            {
                if (pt.X < result[0])
                    result[0] = pt.X;
                if (pt.Y < result[1])
                    result[1] = pt.Y;
                if (pt.X > result[2])
                    result[2] = pt.X;
                if (pt.Y > result[3])
                    result[3] = pt.Y;
            }

            return result;
        }




        // TEST METHOD FUNCTIONS

        /// <summary>
        ///     Runs all method tests when called. Any failures are printed to debug output.
        /// </summary>
        public static void RunUnitTesting()
        {
            // Method to call all unit tests

            Test_Contains();
            Test_ParsePoints();
        }

        /// <summary>
        ///     Tests the Contains method.
        /// </summary>
        /// <remarks>
        ///     To add tests: <br/>
        ///     Add to ptList (ptCollection index, expected result, and point to check for) <br/>
        ///     Add to ptCollections to test different point configurations.
        /// </remarks>
        private static void Test_Contains()
        {
            PointCollection[] ptCollections =
                {
                    new PointCollection { new Point(0,0), new Point(10,0), new Point(10,10), new Point(0,10) },
                    new PointCollection { new Point(0,10), new Point(0,0), new Point(10,0), new Point(10,10) },
                    new PointCollection { new Point(5,20), new Point(30,20), new Point(30,40), new Point(5,40) },
                    new PointCollection { new Point(30,20), new Point(5,40), new Point(5,20), new Point(30,40) },
                };

            List<(int, bool, Point)> ptList = new List<(int, bool, Point)>
                {
                    // TESTS 0-4
                    (0, true, new Point(5,5)),      // center of figure
                    (0, true, new Point(1,8)),      // near inner edge
                    (0, false, new Point(5,-2)),    // above figure, out of screen
                    (0, false, new Point(-2,5)),    // left of figure, out of screen
                    (0, false, new Point(-4,-4)),   // above and left of figure, out of screen on both
 
                    // TESTS 5-9
                    (1, true, new Point(5,5)),      // same as above, different point order
                    (1, true, new Point(1,8)),
                    (1, false, new Point(5,-2)),
                    (1, false, new Point(-2,5)),
                    (1, false, new Point(-4,-4)),

                    // TESTS 10-19
                    (2, true, new Point(5,20)),     // starting point
                    (2, true, new Point(30,40)),    // end point
                    (2, true, new Point(10,20)),    // top edge
                    (2, true, new Point(10,40)),    // bottom edge
                    (2, true, new Point(5,30)),     // left edge
                    (2, true, new Point(30,30)),    // right edge
                    (2, false, new Point(15,45)),   // below figure
                    (2, false, new Point(45,30)),   // right of figure
                    (2, false, new Point(15,15)),   // above figure
                    (2, false, new Point(3,30)),    // left of figure

                    // TESTS 20-29
                    (3, true, new Point(5,20)),     // same as above, different point order
                    (3, true, new Point(30,40)),
                    (3, true, new Point(10,20)),
                    (3, true, new Point(10,40)),
                    (3, true, new Point(5,30)),
                    (3, true, new Point(30,30)),
                    (3, false, new Point(15,45)),
                    (3, false, new Point(45,30)),
                    (3, false, new Point(15,15)),
                    (3, false, new Point(3,30)),
                };

            foreach ((int, bool, Point) test in ptList)
            {
                if (test.Item2 != Contains(ptCollections[test.Item1], test.Item3))
                    System.Diagnostics.Debug.WriteLine("Test_Contains: Test[" + ptList.IndexOf(test) + "] Failed!");
            }
            System.Diagnostics.Debug.WriteLine("Test_Contains: Complete");
        }

        /// <summary>
        ///     Tests the ParsePoints method.
        /// </summary>
        /// <remarks>
        ///     To add tests: <br/>
        ///     Add a PointCollection to ptCollections, and an array of 4 expected output doubles
        ///     to expectedResults.
        /// </remarks>
        private static void Test_ParsePoints()
        {
            PointCollection[] ptCollections =
                {
                    new PointCollection { new Point(0,0), new Point(10,0), new Point(10,10), new Point(0,10) },
                    new PointCollection { new Point(0,10), new Point(0,0), new Point(10,0), new Point(10,10) },
                    new PointCollection { new Point(5,20), new Point(30,20), new Point(30,40), new Point(5,40) },
                    new PointCollection { new Point(30,20), new Point(5,40), new Point(5,20), new Point(30,40) },
                };
            double[][] expectedResults =
                {
                    new double[] { 0, 0, 10, 10 },
                    new double[] { 0, 0, 10, 10 },
                    new double[] { 5, 20, 30, 40 },
                    new double[] { 5, 20, 30, 40 },
                };

            for (int i = 0; i < ptCollections.Length; i++)
            {
                if (ParsePoints(ptCollections[i]) == expectedResults[i])
                    System.Diagnostics.Debug.WriteLine("Test_ParsePoints: Test[" + i + "] Failed!");
            }
            System.Diagnostics.Debug.WriteLine("Test_ParsePoints: Complete");
        }

    }
}
