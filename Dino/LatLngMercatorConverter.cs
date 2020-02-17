using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dino
{
    
    /// <summary>
    /// Simplified converter from lat lon coordinates (angular) to marcator planar x,y coordinates
    /// ---> credits to https://gis.stackexchange.com/questions/15269/how-to-convert-lat-long-to-meters-using-mercator-projection-in-c
    /// </summary>
    class LatLngMercatorConverter
    {
        public MapPoint ProjectPoint(MapPoint PointToReproject)
        {
            const double RadiansPerDegree = Math.PI / 180;

            double Rad = PointToReproject.Y() * RadiansPerDegree;
            double FSin = Math.Sin(Rad);
            double DegreeEqualsRadians = 0.017453292519943;
            double EarthsRadius = 6378137;

            double y = EarthsRadius / 2.0 * Math.Log((1.0 + FSin) / (1.0 - FSin));
            double x = PointToReproject.X() * DegreeEqualsRadians * EarthsRadius;

            return new MapPoint(x, y);
        }
    }
}
