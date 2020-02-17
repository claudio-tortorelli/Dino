# Dino
A C# CLI tool for automatic GPX track classification against KML area polygons

----

Purposes
--------
This tool was developed to support Arezzo section (www.caiarezzo.it) of Italian Alpine Club (www.cai.it/) to classify thousand of GPX into a set of categories (eg. valleys and mountains of the region).
GPX were acquired with a Garmin GPS and KML area were drawn and exported with Google Earth.

Warning: input data and results were tested in this specific context only.

This work is dedicated to Donato 'Dino' Ginepri, a CAI member and a friend.

Features
------
- parsing of GPX tracks (GPS generated)
- parsing of KML area (Google Earth generated)
- lat-lon coordinate translation to mercator 
- classification results exported to CSV
- optional gpx decimation

Option file and usage
------
synopsis: Dino.exe <option file path>
  
The option file is a standard txt file with following entries
- trackFolder=[gpx folder path]  (*)
- areaFolder=[kml folder path] (*)
- csvPath=[csv output path] (*)
- multiarea=[true|false], default = true -> a track can be linked to more than one area
- threshold=[0,1], default=0.2 -> ratio of track points inside an area over the total, needed to link track and area
- projectionType=[mercator|none], default = mercator
- decimate=[true|false], default = true -> if the decimation gpx version must be generated
- maxPoint=<integer>, default = 100 -> max number of points of a decimated track
- verbose=[true|false], default = true, process output is printed to console
  
A sample option file is included into sources.
(*) required

Input/output Data
------
- Input: a folder with GPX files (tracks)
- Input: a folder with KML files (area)
- Output: a folder with decimated GPX files
- Output: a CSV classification file

Input data are never modified or deleted.
Output data are overwritten.

Credits
-----
- Ray Cast algorithm 
  https://www.codeproject.com/Tips/626992/Check-if-a-Point-is-Inside-the-Polygon-Using-Ray-C
- MercatorConverter is a C# porting of this python version 
  https://gis.stackexchange.com/questions/15269/how-to-convert-lat-long-to-meters-using-mercator-projection-in-c

Change Log
------
1.0.1 2020-02-18  Options parsing minor bugfix
                  Added verbose output
                  Added some sample data
1.0.0 2020-02-17  First version
