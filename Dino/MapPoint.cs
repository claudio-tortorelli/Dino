using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dino
{
    /// <summary>
    /// A simple planar projected point
    /// A set of MapPoint represents a track or an area
    /// </summary>
    class MapPoint
    {
        /// <summary>
        /// x coord
        /// </summary>
        private double _X;
        /// <summary>
        /// y coord
        /// </summary>
        private double _Y;
        /// <summary>
        /// z coord
        /// </summary>
        private double _Z;

        /// <summary>
        /// base constructor
        /// </summary>
        public MapPoint(double x, double y) : this(x, y, 0.0)
        {
            
        }

        /// <summary>
        /// this constructor handles the z too
        /// </summary>
        public MapPoint(double x, double y, double z)
        {
            this._X = x;
            this._Y = y;
            this._Z = z;
        }

        /// <summary>
        /// get x value
        /// </summary>
        public double X() { return _X; }
        /// <summary>
        /// get y value
        /// </summary>
        public double Y() { return _Y; }
        /// <summary>
        /// get z value
        /// </summary>
        public double Z() { return _Z; }
    }
}
