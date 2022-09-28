using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LatLon
{
    //takes in (x,y) coordinates and converts them to a Unity world position based on anchors
    public static Vector3 ConvertCoordToPos(float northwest_x, float northwest_y, float x, float y)
    {
        float worldX = (northwest_x - x) * 100;
        float worldY = (northwest_y - y) * 100;

        Vector3 pos = new Vector3(worldX, worldY);

        return pos;
    }
}
