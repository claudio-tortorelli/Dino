# Dino
A C# tool for automatic GPX track classification against KML area polygons

----

Purposes
--------
This tool was developed to support Arezzo section (www.caiarezzo.it) of Italian Alpine Club (www.cai.it/) to classify thousand of GPX in a set of categories (eg. valleys and mountains of the region).
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
  
The option file is a standard txt file with following options
  trackFolder=<gpx folder path>
  areaFolder=<kml folder path>
  csvPath=<csv output path>
  multiarea=[yes|no], default = yes -> a track can be linked to more than one area
  threshold=0.2, 
  projectionType=[mercator|none], default = mercator
  decimate=[yes|no], default = yes -> if the decimation gpx version must be generated
  maxPoint=100

Input/output Data
------
- Input: a folder with GPX files (tracks)
- Input: a folder with KML files (area)
- Output: a folder with decimated GPX files
- Output: a CSV classification file

Input data are never modified or deleted.
Output data are overwritten.

Change Log
------
1.0.0 2020-02-17 First version

