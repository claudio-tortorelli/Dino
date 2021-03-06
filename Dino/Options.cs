﻿using System;
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
        public static string _trackfolder = "";
        /// <summary>
        /// path of the input folder where kml area are stored
        /// </summary>
        public static string _areafolder = "";
        /// <summary>
        /// path of the output csv
        /// </summary>
        public static string _csvpath = "";
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
        /// kind of planar coordinates converter.
        /// Allowed values: mercator, wgs84
        /// default = mercator
        /// </summary>
        public static string _projectionType = "wgs84";
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
        /// true if the output leaflet maps must be built
        /// </summary>
        public static bool _buildMapArea = true;
        /// <summary>
        /// the map root folder
        /// </summary>
        public static string _mapFolder = "";
        /// <summary>
        /// the output is printed to console
        /// </summary>
        public static bool _verbose = true;
        /// <summary>
        /// area polygon show on map
        /// </summary>
        public static bool _showAreaOnMap = true;
        /// <summary>
        /// gpx markers show on map
        /// </summary>
        public static bool _showMarkers = true;

        


        /// <summary>
        /// Parse an input option file to related members
        /// </summary>
        public static void ParseOpts(string optionPath)
        {
            string[] lines = System.IO.File.ReadAllLines(@optionPath);

            Program.Log("[PARSING OPTIONS]");
            foreach (string optLine in lines)
            {
                string line = optLine.ToLower();
                if (line.StartsWith("trackfolder="))
                {
                    _trackfolder = line.Replace("trackfolder=", "");
                    Program.Log("trackfolder=" + _trackfolder);
                }
                else if (line.StartsWith("areafolder="))
                {
                    _areafolder = line.Replace("areafolder=", "");
                    Program.Log("areafolder=" + _areafolder);
                }             
                else if (line.StartsWith("multiarea="))
                {
                    string multiarea = line.Replace("multiarea=", "");
                    Program.Log("multiarea=" + multiarea);
                    if (multiarea.Equals("false"))
                        _multiarea = false;
                }
                else if (line.StartsWith("threshold="))
                {
                    _threshold = double.Parse(line.Replace("threshold=", "").Replace(',', '.'), CultureInfo.InvariantCulture);
                    Program.Log("threshold=" + _threshold);
                }
                else if (line.StartsWith("csvpath="))
                {
                    _csvpath = line.Replace("csvpath=", "");
                    Program.Log("csvpath=" + _csvpath);
                }
                else if (line.StartsWith("projectiontype="))
                {
                    _projectionType = line.Replace("projectiontype=", "");
                    Program.Log("projectiontype=" + _projectionType);
                }
                else if (line.StartsWith("decimate="))
                {
                    string decimate = line.Replace("decimate=", "");
                    Program.Log("decimate=" + decimate);
                    if (decimate.Equals("false"))
                        _decimate = false;
                }
                else if (line.StartsWith("maxpoint="))
                {
                    _maxPoints = int.Parse(line.Replace("maxpoint=", ""));
                    Program.Log("maxpoint=" + _maxPoints);
                }
                else if (line.StartsWith("verbose="))
                {
                    string verbose = line.Replace("verbose=", "");
                    Program.Log("verbose=" + verbose);
                    if (verbose.Equals("false"))
                        _verbose = false;
                }
                else if (line.StartsWith("buildmaparea="))
                {
                    string buildMapArea = line.Replace("buildmaparea=", "");
                    Program.Log("buildMapArea=" + buildMapArea);
                    if (buildMapArea.Equals("false"))
                        _buildMapArea = false;
                }
                else if (line.StartsWith("mapfolder="))
                {
                    _mapFolder = line.Replace("mapfolder=", "");
                    Program.Log("mapFolder=" + _mapFolder);
                }
                else if (line.StartsWith("showareaonmap="))
                {
                    string showareaonmap = line.Replace("showareaonmap=", "");
                    Program.Log("showareaonmap=" + showareaonmap);
                    if (showareaonmap.Equals("false"))
                        _showAreaOnMap= false;
                }
                else if (line.StartsWith("showmarkers="))
                {
                    string showmarkers = line.Replace("showmarkers=", "");
                    Program.Log("showmarkers=" + showmarkers);
                    if (showmarkers.Equals("false"))
                        _showMarkers = false;
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
            if (_buildMapArea && _mapFolder.Length == 0)
            {
                throw new Exception("Invalid MapFolder path");
            }
            Program.Log("[DONE]");
        }
    }
}
