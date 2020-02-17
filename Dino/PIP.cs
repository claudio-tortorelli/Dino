using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dino
{
    class PIP
    {        
        /// <summary>
        /// Determines if the given point is inside the polygon using the ray cast algorithm
        /// </summary>
        /// <param name="pointArray">the vertices of polygon</param>
        /// <param name="testPoint">the given point</param>
        /// <returns>true if the point is inside the polygon; otherwise, false</returns>
        /// ---> credits to https://www.codeproject.com/Tips/626992/Check-if-a-Point-is-Inside-the-Polygon-Using-Ray-C
        public static bool RayCast(MapPoint[] pointArray, MapPoint testPoint)
        {
            //Ray-cast algorithm is here onward
            int k, j = pointArray.Length - 1;
            bool oddNodes = false; //to check whether number of intersections is odd
            for (k = 0; k < pointArray.Length; k++)
            {
                //fetch adjucent points of the polygon
                MapPoint polyK = pointArray[k];
                MapPoint polyJ = pointArray[j];

                //check the intersections
                if (((polyK.Y() > testPoint.Y()) != (polyJ.Y() > testPoint.Y())) &&
                 (testPoint.X() < (polyJ.X() - polyK.X()) * (testPoint.Y() - polyK.Y()) / (polyJ.Y() - polyK.Y()) + polyK.X()))
                    oddNodes = !oddNodes; //switch between odd and even
                j = k;
            }

            if (oddNodes) //if odd number of intersections
                return true; // the point is inside
            return false; // is outside otherwise
        }

        /// <summary>
        /// Determines if the given point is inside the polygon (slower implementation than ray cast)
        /// </summary>
        /// <param name="polygon">the vertices of polygon</param>
        /// <param name="testPoint">the given point</param>
        /// <returns>true if the point is inside the polygon; otherwise, false</returns>
        /// ---> credits to https://stackoverflow.com/questions/4243042/c-sharp-point-in-polygon        
        public static bool IsPointInPolygon(MapPoint[] polygon, MapPoint testPoint)
        {
            bool result = false;
            int j = polygon.Count() - 1;
            for (int i = 0; i < polygon.Count(); i++)
            {
                if (polygon[i].Y() < testPoint.Y() && polygon[j].Y() >= testPoint.Y() || 
                    polygon[j].Y() < testPoint.Y() && polygon[i].Y() >= testPoint.Y())
                {
                    if (polygon[i].X() + (testPoint.Y() - polygon[i].Y()) / (polygon[j].Y() - polygon[i].Y()) * (polygon[j].X() - polygon[i].X()) < testPoint.X())
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }
    }
}
