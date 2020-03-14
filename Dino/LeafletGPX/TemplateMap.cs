using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dino
{
    class TemplateMap
    {
        public static string leaflet = @"
<!DOCTYPE html>
<html>
<head>
    <title>[TITLE]</title>
    <meta charset=""utf-8"" />
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
	<link rel=""stylesheet"" href=""leaflet.css""/>
 	<script src=""leaflet.js""></script>	
	<script src=""leaflet-providers.js""></script>
	<script src=""gpx.js""></script>
	
    <link rel=""stylesheet"" href=""leaflet.groupedlayercontrol.min.css"">
    <script src=""leaflet.groupedlayercontrol.min.js"" type=""text/javascript""></script>

    <link rel=""stylesheet"" href=""Control.MiniMap.css"">
    <script src=""Control.MiniMap.min.js"" type=""text/javascript""></script>

    <style>
        body {
            padding: 0;
            margin: 0;
        }
        html,
        body,
        #map {
            height: 100%;
        }
    </style>
</head>

<body>
    <div id=""map""></div>
    <script>
       var map = L.map('map', {
            zoom: 15
        });
        [BOUNDS]      
        var defaultBase = L.tileLayer.provider('Esri.WorldImagery').addTo(map);
        var baseLayers = {
            'ESRI Imagery': defaultBase,
            'OSM Topo': L.tileLayer.provider('OpenTopoMap')
        };
        //Overlay grouped layers    
        var groupOverLays = {
            
        };
        //add layer switch control
        L.control.groupedLayers(baseLayers, groupOverLays).addTo(map);

        //add scale bar to map
        L.control.scale({
            position: 'bottomleft'
        }).addTo(map);

        // Overview mini map
        var Esri_WorldTopoMap = L.tileLayer('https://server.arcgisonline.com/ArcGIS/rest/services/World_Topo_Map/MapServer/tile/{z}/{y}/{x}', {
            attribution: '&copy; Esri &mdash; Esri, DeLorme, NAVTEQ, TomTom, Intermap, iPC, USGS, FAO, NPS, NRCAN, GeoBase, Kadaster NL, Ordnance Survey, Esri Japan, METI, Esri China (Hong Kong), and the GIS User Community'
        });
 
        var miniMap = new L.Control.MiniMap(Esri_WorldTopoMap, {
            toggleDisplay: true,
            minimized: false,
            position: 'bottomleft'
        }).addTo(map);
        
        [GPX]		

        [AREA]

    </script>
</body>
</html>
";
    }
}
