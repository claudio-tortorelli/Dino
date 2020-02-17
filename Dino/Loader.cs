using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Dino
{
    class Loader
    {
        /// <summary>
        /// The marcator converter
        /// </summary>
        private static LatLngMercatorConverter _MercatorConverter = new LatLngMercatorConverter();

        /// <summary>
        /// Load all gpx inside the input folder and store their points
        /// </summary>
        public static SortedDictionary<string, MapPoint[]> Tracks(string trackFolder, string projectionType)
        {
            Console.WriteLine("----------------------");
            Console.WriteLine("[LOADING TRACKS]");

            SortedDictionary<string, MapPoint[]> outTracks = new SortedDictionary<string, MapPoint[]>();

            DirectoryInfo d = new DirectoryInfo(trackFolder);
            foreach (var file in d.GetFiles("*" + Constants.GPX))
            {
                string curFileName = file.Name.Replace(Constants.GPX, "");
                Console.Write("  " + curFileName);

                XmlDocument gpxDoc = new XmlDocument();
                gpxDoc.Load(file.FullName);

                XmlNamespaceManager nsmgr = new XmlNamespaceManager(gpxDoc.NameTable);
                nsmgr.AddNamespace("x", "http://www.topografix.com/GPX/1/1");
                XmlNodeList nl = gpxDoc.SelectNodes("//x:trkpt", nsmgr);

                MapPoint[] curTrack = new MapPoint[nl.Count];
                int iPt = 0;
                foreach (XmlNode xnode in nl)
                {
                    string ptContent = xnode.OuterXml;
                    ptContent = ptContent.Replace("<trkpt ", "");
                    ptContent = ptContent.Replace("xmlns=\"http://www.topografix.com/GPX/1/1\">", "");
                    ptContent = ptContent.Replace("</time>", "");
                    ptContent = ptContent.Replace("</trkpt>", "");
                    ptContent = ptContent.Replace("<time>", " ");
                    ptContent = ptContent.Replace("<time>", " ");
                    ptContent = ptContent.Replace("lat=\"", "");
                    ptContent = ptContent.Replace("lon=\"", "");
                    ptContent = ptContent.Replace("\"", "");
                    ptContent = ptContent.Replace("<ele>", "");
                    ptContent = ptContent.Replace("</ele>", "");
                    string[] values = ptContent.Split(' ');

                    try
                    {
                        double lat = double.Parse(values[0].Replace(',', '.'), CultureInfo.InvariantCulture);
                        double lon = double.Parse(values[1].Replace(',', '.'), CultureInfo.InvariantCulture);
                        double elev = double.Parse(values[2].Replace(',', '.'), CultureInfo.InvariantCulture);

                        if (projectionType.Equals("mercator")) 
                            curTrack[iPt++] = _MercatorConverter.ProjectPoint(new MapPoint(lat, lon));
                        else
                            curTrack[iPt++] = new MapPoint(lat, lon);                       
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine(" !! point parsing error: " + ex.Message);
                    }
                }
                Console.WriteLine(String.Format(" > extracted {0} points", nl.Count));
                outTracks.Add(curFileName, curTrack);                
            }
            Console.WriteLine("[DONE]");
            return outTracks;
        }

        /// <summary>
        /// Load all kml area inside the input folder and store their points
        /// </summary>
        public static SortedDictionary<string, MapPoint[]> Area(string areaFolder, string projectionType)
        {
            Console.WriteLine("----------------------");
            Console.WriteLine("[LOADING AREA]");

            SortedDictionary<string, MapPoint[]> outArea = new SortedDictionary<string, MapPoint[]>();

            string outFolderPath = areaFolder + Path.DirectorySeparatorChar + Constants.OUT_FOLD;
            Directory.CreateDirectory(outFolderPath);

            DirectoryInfo d = new DirectoryInfo(areaFolder);
            foreach (var file in d.GetFiles("*" + Constants.KML))
            {
                string curFileName = file.Name.Replace(Constants.KML, "");
                Console.Write("  " + curFileName);

                XmlDocument kmlDoc = new XmlDocument();
                kmlDoc.Load(file.FullName);

                XmlNamespaceManager nsmgr = new XmlNamespaceManager(kmlDoc.NameTable);
                nsmgr.AddNamespace("gx", "http://www.opengis.net/kml/2.2");
                XmlNodeList nl = kmlDoc.SelectNodes("//gx:coordinates", nsmgr);

                MapPoint[] curArea = null;
                int iPt = 0;

                int counter = 0;
                foreach (XmlNode xnode in nl)
                {
                    string content = xnode.OuterXml;
                    content = content.Replace("<coordinates xmlns=\"http://www.opengis.net/kml/2.2\">", "");
                    content = content.Replace("\n", "");
                    content = content.Replace("\t", "");
                    content = content.Replace("</coordinates>", "");
                    content = content.Replace(",0", "");
                    content = content.Trim();
                    string[] values = content.Split(' ');
                    curArea = new MapPoint[values.Length];
                    for (int i = 0; i < values.Length; i++)
                    {
                        try
                        {
                            if (values[i].Length == 0)
                                continue;
                            string[] lonLat = values[i].Split(',');
                            double lon = double.Parse(lonLat[0].Replace(',', '.'), CultureInfo.InvariantCulture);
                            double lat = double.Parse(lonLat[1].Replace(',', '.'), CultureInfo.InvariantCulture);

                            if (projectionType.Equals("mercator")) // Mercator
                                curArea[iPt++] = _MercatorConverter.ProjectPoint(new MapPoint(lat, lon));
                            else
                                curArea[iPt++] = new MapPoint(lat, lon);
                            counter++;
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine(" !! point parsing error: " + ex.Message);
                        }
                    }
                }
                outArea.Add(curFileName, curArea);
                Console.WriteLine(String.Format(" > extracted {0} point", counter));
            }
            Console.WriteLine("[DONE]");
            return outArea;
        }
    }
}
