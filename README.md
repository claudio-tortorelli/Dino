# Dino
A C# CLI tool for automatic GPX track classification against KML area polygons

----

Purposes
--------
This tool was developed to support Arezzo section (www.caiarezzo.it) of Italian Alpine Club (www.cai.it/) to classify thousand of GPX into a set of categories (eg. valleys and mountains of the region).
GPX were acquired with a Garmin GPS and KML area were drawn and exported with Google Earth.

<b>Warning</b>: input data and results were tested in this specific context only.

This work is dedicated to <b>Donato 'Dino' Ginepri</b>, a CAI member and a friend.

Features
------
- parsing of GPX tracks (GPS generated)
- parsing of KML area (Google Earth generated)
- classification results stored to CSV
- optional lat-lon wgs84 coordinates translation to mercator
- optional gpx decimation
- optional leaflet maps generation

Option file and usage
------
synopsis: Dino.exe <option full file path>
  
The option file is a standard txt with following entries
- trackFolder=[gpx folder path]  (*)
- areaFolder=[kml folder path] (*)
- csvPath=[csv output path] (*)
- multiarea=[true|false], default = true -> a track can be linked to more than one area
- threshold=[0,1], default=0.2 -> ratio of track points inside an area over the total, needed to link track and area
- projectionType=[mercator|wgs84], default = wgs84
- decimate=[true|false], default = true -> if the decimation gpx version must be generated
- maxPoint=<integer>, default = 100 -> max number of points of a decimated track
- verbose=[true|false], default = true, process output is printed to console
- buildMapArea=[true|false], default = true, build a leaflet map with all area's tracks
- mapFolder=[map folder path], the folder where are stored built maps
  
A sample option file is included into sources.

(*) required

Input/output Data
------
- Input: a folder with GPX files (tracks)
- Input: a folder with KML files (area)
- Output: a CSV classification file
- Output: a folder with decimated GPX files
- Output: a folder with leaflet area maps

Input data are never modified or deleted.
Output data are overwritten.

Binaries and samples
------
Under bin folder is included a binary built with Visual Studio 2012.
In the sample_data folder are stored some test gpx and kml.

A leaflet map folder deployed on my web server is visible at this link
https://www.claudiotortorelli.it/dino/pratomagno/

Credits
-----
- Ray Cast algorithm 
  https://www.codeproject.com/Tips/626992/Check-if-a-Point-is-Inside-the-Polygon-Using-Ray-C
- MercatorConverter is a C# porting of this python version 
  https://gis.stackexchange.com/questions/15269/how-to-convert-lat-long-to-meters-using-mercator-projection-in-c
- leaflet map library (https://leafletjs.com/); gpx plugins and samples
  https://github.com/StephSaephan/leaflet-map-example
  https://github.com/mpetazzoni/leaflet-gpx

Change Log
------
<b>1.1.1 2020-03-14</b>
- Added area layer to leaflet map

<b>1.1.0 2020-03-07</b>
- Added leaflet optional map building
- GPX decimation

<b>1.0.1 2020-02-18</b>
- Options parsing minor bugfix
- Added verbose output
- Added some sample data

<b>1.0.0 2020-02-17</b>
- First version
