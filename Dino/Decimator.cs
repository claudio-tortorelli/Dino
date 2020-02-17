using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Dino
{    
    class Decimator
    {
        /// <summary>
        /// Regularly decimate gpx track points using a maximum point number value
        /// </summary>
        public static void Decimate(string trackFolder, double maxPoints)
        {
            Program.Log("----------------------");
            Program.Log("[APPLYING POINT DECIMATION");

            // create output folder
            string outFolderPath = trackFolder + Path.DirectorySeparatorChar + Constants.DECIMATION_FOLD;
            Directory.CreateDirectory(outFolderPath);

            // scan input folder
            DirectoryInfo d = new DirectoryInfo(trackFolder);
            foreach (var file in d.GetFiles("*" + Constants.GPX))
            {
                string curFileName = file.Name.Replace(Constants.GPX, "");
                Program.Log("  " + curFileName, false);

                using (StreamWriter outputFile = new StreamWriter(outFolderPath + Path.DirectorySeparatorChar + file.Name, false))
                {
                    // load gpx
                    XmlDocument gpxDoc = new XmlDocument();
                    gpxDoc.Load(file.FullName);

                    XmlNamespaceManager nsmgr = new XmlNamespaceManager(gpxDoc.NameTable);
                    nsmgr.AddNamespace("x", "http://www.topografix.com/GPX/1/1");
                    XmlNodeList nl = gpxDoc.SelectNodes("//x:trkpt", nsmgr);

                    // evaluate the decimation ratio
                    int pointModule = Convert.ToInt32((double)nl.Count / maxPoints) + 1;
                    int residualPts = 0;
                    for (int iPt = nl.Count - 1; iPt >= 0; iPt--)
                    {
                        if (iPt % pointModule == 0) 
                        {
                            // save this point
                            residualPts++;
                            continue;
                        }
                        nl[iPt].ParentNode.RemoveChild(nl[iPt]); // skip others
                    }
                    gpxDoc.Save(outputFile);

                    Program.Log(String.Format(" > decimated from {0} points to {1} points", nl.Count, residualPts));
                }
            }

            Program.Log("[DONE]");
        }
    }
}
