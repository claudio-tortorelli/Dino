using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dino
{
    /// <summary>
    /// Class to store and access all input process options
    /// </summary>
    class Options
    {
        /// <summary>
        /// path of input folder where gpx track are stored
        /// </summary>
        public static string _trackfolder;
        /// <summary>
        /// path of the input folder where kml area are stored
        /// </summary>
        public static string _areafolder;
        /// <summary>
        /// path of the output csv
        /// </summary>
        public static string _csvpath;
        /// <summary>
        /// true if a track can be classified in more than one area
        /// default = true
        /// </summary>
        public static bool _multiarea = true;
        /// <summary>
        /// threshold ration value to include a track in an area.
        /// ratio = track's points inside and area / total track's points
        /// if ratio > _threshold then the track is classified in the target area        /// 
        /// default = 0.2
        /// </summary>
        public static double _threshold = 0.2;
        /// <summary>
        /// kind of planar coordinates projection.
        /// Allowed values: mercator, none
        /// default = mercator
        /// </summary>
        public static string _projectionType = "mercator";
        /// <summary>
        /// true if a decimated copy of gpx must be generated
        /// default = true
        /// </summary>
        public static bool _decimate = true;
        /// <summary>
        /// maximum number of point inside a decimated gpx
        /// </summary>
        public static int _maxPoints = 100;


        /// <summary>
        /// Parse an input option file to related members
        /// </summary>
        public static void ParseOpts(string optionPath)
        {
            string[] lines = System.IO.File.ReadAllLines(@optionPath);

            Console.WriteLine("[PARSING OPTIONS]");
            foreach (string optLine in lines)
            {
                string line = optLine.ToLower();
                if (line.StartsWith("trackfolder="))
                {
                    _trackfolder = line.Replace("trackfolder=", "");
                    Console.WriteLine("trackfolder=" + _trackfolder);
                }
                else if (line.StartsWith("areafolder="))
                {
                    _areafolder = line.Replace("areafolder=", "");
                    Console.WriteLine("areafolder=" + _areafolder);
                }             
                else if (line.StartsWith("multiarea="))
                {
                    string multiarea = line.Replace("multiarea=", "");
                    Console.WriteLine("multiarea=" + multiarea);
                    if (multiarea.Equals("no"))
                        _multiarea = false;
                }
                else if (line.StartsWith("threshold="))
                {
                    _threshold = double.Parse(line.Replace("threshold=", "").Replace(',', '.'), CultureInfo.InvariantCulture);
                    Console.WriteLine("threshold=" + _threshold);
                }
                else if (line.StartsWith("csvpath="))
                {
                    _csvpath = line.Replace("csvpath=", "");
                    Console.WriteLine("csvpath=" + _csvpath);
                }
                else if (line.StartsWith("projectiontype="))
                {
                    _projectionType = line.Replace("projectiontype=", "");
                    Console.WriteLine("projectiontype=" + _projectionType);
                }
                else if (line.StartsWith("decimate="))
                {
                    string decimate = line.Replace("decimate=", "");
                    Console.WriteLine("decimate=" + _decimate);
                    if (decimate.Equals("no"))
                        _decimate = false;
                }
                else if (line.StartsWith("maxpoint="))
                {
                    _maxPoints = int.Parse(line.Replace("maxpoint=", ""));
                    Console.WriteLine("maxpoint=" + _maxPoints);
                }
            }

            if (_trackfolder.Length == 0)
            {
                throw new Exception("Invalid TrackFolder");
            }
            if (_areafolder.Length == 0)
            {
                throw new Exception("Invalid AreaFolder");
            }
            if (_csvpath.Length == 0)
            {
                throw new Exception("Invalid Output path");
            }
            Console.WriteLine("[DONE]");
        }
    }
}
