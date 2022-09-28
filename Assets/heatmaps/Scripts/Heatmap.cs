/*NOTE: This code is adopted from a heatmap designed 
 * by Alan Zucconi, whose personal blog can be found 
 * at www.alanzucconi.com*/
using UnityEngine;
using System.Collections;

public class Heatmap : MonoBehaviour
{
    public Vector4[] positions;
    public Vector4[] properties;

    public Material material;

    public TextAsset jsonFile;

    void Start()
    {
        positions = new Vector4[50];
        properties = new Vector4[50];

        int i = 0;

        //takes in a list of (x, y) coordinates, converts them to Unity world positions based on anchors, then assigns a radius and a value for temp based on input data

        Temps tempsInJson = JsonUtility.FromJson<Temps>(jsonFile.text);

        foreach (Temp temp in tempsInJson.temps)
        {
            Vector3 pos = LatLon.ConvertCoordToPos(GameObject.Find("Anchor1").GetComponent<LocationMarker>().LatLon.x, GameObject.Find("Anchor1").GetComponent<LocationMarker>().LatLon.y, temp.x, temp.y);
            positions[i] = new Vector4(pos.x, pos.y, 0, 0);
            properties[i] = new Vector4(50, temp.tmp / 100, 0, 0);
            i++;
        }

        material.SetInt("_Points_Length", 50);
        material.SetVectorArray("_Points", positions);
        material.SetVectorArray("_Properties", properties);

    }

}