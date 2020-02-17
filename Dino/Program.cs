using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Dino
{
    class Program
    {
        /// <summary>
        /// map of input tracks
        /// </summary>
        private static SortedDictionary<string, MapPoint[]> _tracks = null;
        /// <summary>
        /// map of input area
        /// </summary>
        private static SortedDictionary<string, MapPoint[]> _area = null;
        
        static void Main(string[] args)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("Dino v. {0} - CaiArezzo.it - Claudio Tortorelli", version);
            Console.WriteLine("--------------------------------------------------\n");

            if (args.Length != 1)
            {
                Console.WriteLine("Help: options file path is needed as argument");
                return;
            }
            if (!File.Exists(args[0]))
            {
                Console.Error.WriteLine("Options file not found");
                return;
            }
            // parsing options from file
            Options.ParseOpts(args[0]);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // parse tracks and area
            _tracks = Loader.Tracks(Options._trackfolder, Options._projectionType);
            _area = Loader.Area(Options._areafolder, Options._projectionType);

            // create a decimated gpx copy
            if (Options._decimate)
                Decimator.Decimate(Options._trackfolder, Options._maxPoints);
            
            // classify tracks on area
            using (StreamWriter outputFile = new StreamWriter(Options._csvpath, false))
            {
                ClassifyTracks(outputFile);
            }

            stopwatch.Stop();
            Console.WriteLine("---------------------------");
            Console.WriteLine(_tracks.Count + " tracks processed against " + _area.Count + " areas");
            Console.WriteLine("Process done in {0} seconds", stopwatch.ElapsedMilliseconds / 1000);
            Console.WriteLine("---------------------------");
            //Console.ReadKey();
        }

        /// <summary>
        /// Use point in polygon algorithm to classify input tracks over against the area
        /// </summary>
        private static void ClassifyTracks(StreamWriter outFile)
        {
            Console.WriteLine("----------------------");
            Console.WriteLine("[TRACKS CLASSIFICATION]");

            foreach (KeyValuePair<string, MapPoint[]> entryTrack in _tracks)
            {
                Console.WriteLine(entryTrack.Key);
                string insideArea = "";
                int totPts = entryTrack.Value.Length;
                foreach (KeyValuePair<string, MapPoint[]> entryArea in _area)
                {
                    Console.Write(" | " + entryArea.Key);
                    int insidePts = 0;
                    for (int iPt = 0; iPt < totPts; iPt++)
                    {
                        MapPoint curPt = entryTrack.Value[iPt];
                        if (PIP.RayCast(entryArea.Value, curPt))
                            insidePts++;
                    }

                    if (((double)insidePts / totPts) > Options._threshold)
                    {
                        // add to area
                        insideArea += entryArea.Key + Constants.CSV_SEP;
                        Console.WriteLine(" > YES");

                        if (!Options._multiarea)
                            break;                        
                    }
                    else
                        Console.WriteLine(" > NO");                    

                }
                string result = "";
                if (insideArea.Length > 0)
                    result = (entryTrack.Key + Constants.CSV_SEP + insideArea).Trim();
                else
                    result = (entryTrack.Key + Constants.CSV_SEP).Trim();
                outFile.WriteLine(result);
                Console.WriteLine(" --> " + result);
            }

            Console.WriteLine("[DONE]");
        }
    }
}
