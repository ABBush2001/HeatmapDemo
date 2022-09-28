using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValuesHolder : MonoBehaviour
{
    public float nw_x;
    public float nw_y;
    public float se_x;
    public float se_y;

    public void Start()
    {
        //Set positions of anchors
        GameObject.Find("Anchor1").GetComponent<LocationMarker>().LatLon.x = nw_x;
        GameObject.Find("Anchor1").GetComponent<LocationMarker>().LatLon.y = nw_y;

        GameObject.Find("Anchor2").GetComponent<LocationMarker>().LatLon.x = se_x;
        GameObject.Find("Anchor2").GetComponent<LocationMarker>().LatLon.y = se_y;

    }
}
