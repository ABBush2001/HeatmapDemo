# HeatmapDemo

This small Unity demo takes in a json file where each entry in the file contains
- (x,y) coordinates
- A temperature (in fahrenheit)
- A date

And converts those values into data points that can then be mapped onto a 3D object in the form of a heatmap. This is acheived by applying a material to an object using the heatmap shader (created by [Alan Zucconi](https://www.alanzucconi.com/2016/01/27/arrays-shaders-heatmaps-in-unity3d/)), and then placing that object directly on top of the object that one wants to display the map on. In this example, One mesh is placed directly on top of another mesh.

You'll also need to set the anchors (the most upper-left coordinate possible and the most lower-right coordinate possible), as these points will be necessary for converting real world coordinates into Unity world positions.


## Other Notes

- Heatmap.cs : Reads in JSON file w/data on (x,y) positions and temperatures, puts that data into arrays and passes it to the heatmap shader
- LatLon.cs : Converts (x,y) coordinates into Unity World positions based on anchors
- LocationMarker.cs : Attached to the anchors; holds the anchor's (x,y) position
- Temp.cs : Used for parsing JSON
- Temps.cs : Used for parsing JSON
