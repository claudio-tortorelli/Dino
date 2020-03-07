using Dino.LeafletGPX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dino
{
    /// <summary>
    /// Helper for build a area folder with html files and resources
    /// Each folder includes a leaflet template map fitted on current area.
    /// 
    /// credits to: 
    /// https://github.com/StephSaephan/leaflet-map-example
    /// https://github.com/mpetazzoni/leaflet-gpx
    /// </summary>
    class MapBuilder
    {
        private string _areaName;
        private List<string> _tracks;
        private MapPoint[] _area;
        private Random _rnd = new Random();

        public MapBuilder(string areaName, MapPoint[] area)
        {
            _rnd = new Random();
            _areaName = areaName;
            _area = area;
            _tracks = new List<string>();
        }

        public void AddTrack(string trackName)
        {
            _tracks.Add(trackName);
        }

        public void Build()
        {
            // create area folder
            if (!Directory.Exists(Options._mapFolder))
            {
                Directory.CreateDirectory(Options._mapFolder);
            }
            string areaFolder = Options._mapFolder + Path.DirectorySeparatorChar + _areaName.Replace(" ", "-").Replace("'", "");
            areaFolder = areaFolder.ToLower();
            if (!Directory.Exists(areaFolder))
            {
                Directory.CreateDirectory(areaFolder);
            }

            // extract embedded resources
            ExctractResourcesToAreaFolder(areaFolder);

            // copy all decimated track to folder
            ImportGPX(areaFolder);

            // write track markers
            BuildMarkers(areaFolder);        
            
            // write template
            string templatePath = areaFolder + Path.DirectorySeparatorChar + "index.html";
            string template = TemplateMap.leaflet.Replace("[TITLE]", _areaName).Replace("[GPX]", GetGPX()).Replace("[BOUNDS]", GetBounds());
            File.WriteAllText(templatePath, template);
            Program.Log(" --> " + _areaName + " built");
        }

        private void ExctractResourcesToAreaFolder(string areaFolderPath)
        {
            foreach (string currentResource in System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames())
            {
                if (currentResource.EndsWith(".resources"))
                    continue;

                string strFile = areaFolderPath + "\\" + currentResource.Replace("Dino.Resources.", "");
                string path = Path.GetDirectoryName(strFile);
                string rootName = Path.GetFileNameWithoutExtension(strFile);
                string destFile = path + Path.DirectorySeparatorChar + rootName + System.IO.Path.GetExtension(currentResource);

                if (currentResource.EndsWith(Constants.PNG))
                {
                    using (Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(currentResource))
                    {
                        try
                        {
                            Image image = Image.FromStream(stream);
                            image.Save(destFile);
                            image.Dispose();
                        }
                        catch (ArgumentException)
                        {
                            stream.Position = 0;
                        }
                    }
                    continue;
                }

                System.IO.Stream fs = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(currentResource);

                string scriptContents = new StreamReader(fs).ReadToEnd();
                File.WriteAllText(destFile, scriptContents); 
            }
        }

        private void ImportGPX(string areaFolderPath)
        {
            string decFolderPath = Options._trackfolder + Path.DirectorySeparatorChar + Constants.DECIMATION_FOLD;

            foreach (string track in _tracks)
            {
                string source = decFolderPath + Path.DirectorySeparatorChar + track + Constants.GPX;
                string dest = areaFolderPath + Path.DirectorySeparatorChar + track + Constants.GPX;
                File.Copy(source, dest, true);
            }
        }

        private void BuildMarkers(string areaFolderPath)
        {
            foreach (string track in _tracks)
            {
                Bitmap bitmap = new Bitmap(areaFolderPath + Path.DirectorySeparatorChar + "pin.png");

                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    using (Font arialFont = new Font(FontFamily.GenericSansSerif, 7.0f, FontStyle.Bold))
                    {
                        graphics.DrawString(track, arialFont, Brushes.White, new PointF(3f, 3f));
                        graphics.DrawString(track, arialFont, Brushes.Green, new PointF(5f, 5f));
                    }
                }

                bitmap.Save(areaFolderPath + Path.DirectorySeparatorChar + track + "_start" + Constants.PNG);
                bitmap.Dispose();

                bitmap = new Bitmap(areaFolderPath + Path.DirectorySeparatorChar + "pin.png");

                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    using (Font arialFont = new Font(FontFamily.GenericSansSerif, 7.0f, FontStyle.Bold))
                    {
                        graphics.DrawString(track, arialFont, Brushes.White, new PointF(3f, 3f));
                        graphics.DrawString(track, arialFont, Brushes.Red, new PointF(5f, 5f));
                    }
                }

                bitmap.Save(areaFolderPath + Path.DirectorySeparatorChar + track + "_end" + Constants.PNG);
                bitmap.Dispose();
            }
            File.Delete(areaFolderPath + Path.DirectorySeparatorChar + "pin.png");
        }

        private string GetGPX()
        {
            string tag = "";
            foreach (string track in _tracks)
            {
                tag += TemplateGPX.tag.Replace("[FILE_NAME]", track + Constants.GPX).Replace("[ICON_START]", track + "_start" + Constants.PNG).Replace("[ICON_END]", track + "_end" + Constants.PNG);

                int red = _rnd.Next(10, 100);
                int green = _rnd.Next(10, 10);
                int blue = _rnd.Next(10, 256);

                Color rndColor = Color.FromArgb(red, green, blue);
                string hexColor = rndColor.R.ToString("X2") + rndColor.G.ToString("X2") + rndColor.B.ToString("X2");

                //int rndWeight = rnd.Next(3, 6);
                int rndWeight = 3;

                tag = tag.Replace("[COLOR]", hexColor).Replace("[WEIGHT]", string.Format("{0}",rndWeight));
                tag += "\n";
            }
            return tag;
        }

        private string GetBounds()
        {
            double[] tl = new double[2];
            tl[0] = 1000.0;
            tl[1] = -1000.0;
            double[] br = new double[2];
            br[0] = -1000.0;
            br[1] = 1000.0;
            foreach (MapPoint curPt in _area)
            {
                if (curPt.X() < tl[0])
                    tl[0] = curPt.X();
                if (curPt.Y() > tl[1])
                    tl[1] = curPt.Y();
                if (curPt.X() > br[0])
                    br[0] = curPt.X();
                if (curPt.Y() < br[1])
                    br[1] = curPt.Y();
            }

            string left = String.Format("{0:0.000}", tl[0]).Replace(",", ".");
            string top = String.Format("{0:0.000}", tl[1]).Replace(",", ".");
            string right = String.Format("{0:0.000}", br[0]).Replace(",", ".");
            string bottom = String.Format("{0:0.000}", br[1]).Replace(",", ".");

            string bounds = String.Format("[{0},{1}], [{2}, {3}]", left, top, right, bottom);
            return TemplateBounds.bounds.Replace("[BOUNDS]", bounds);
        }
    }
}
