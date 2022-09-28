# HeatmapDemo

- Heatmap.cs : Reads in JSON file w/data on (x,y) positions and temperatures, puts that data into arrays and passes it to the heatmap shader
- LatLon.cs : Converts (x,y) coordinates into Unity World positions based on anchors
- LocationMarker.cs : Attached to the anchors; holds the anchor's (x,y) position
- Temp.cs : Used for parsing JSON
- Temps.cs : Used for parsing JSON
- ValuesHolder.cs : Used to set values of anchor at start 
