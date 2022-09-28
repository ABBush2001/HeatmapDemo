// Alan Zucconi
// www.alanzucconi.com
using UnityEngine;
using System.Collections;

public class Heatmap : MonoBehaviour
{
    public Vector4[] positions;
    public Vector4[] properties;

    public Material material;

    public int count = 50;

    void Start()
    {
        positions = new Vector4[count];
        properties = new Vector4[count];

        //Place the position of Sanel Valley
        Vector2 SanelValley = new Vector2(38.9866f, 123.1064f);
        Vector3 Coords = LatLon.GetUnityPosition(SanelValley, GameObject.FindGameObjectWithTag("Ferndale").GetComponent<LocationMarker>().LatLon, GameObject.FindGameObjectWithTag("Sacramento").GetComponent<LocationMarker>().LatLon, GameObject.FindGameObjectWithTag("Ferndale").transform.position, GameObject.FindGameObjectWithTag("Sacramento").transform.position);

        positions[0] = new Vector4(Coords.x, Coords.y, 0, 0);
        properties[0] = new Vector4(100, 1, 0, 0);

        /*for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = new Vector4(Random.Range(-11.25f, +11.25f), Random.Range(-11.25f, +11.25f), 0, 0);
            properties[i] = new Vector4(Random.Range(0f, 5f), Random.Range(-0.25f, 1f), 0, 0);
        }*/

        material.SetInt("_Points_Length", count);
        material.SetVectorArray("_Points", positions);
        material.SetVectorArray("_Properties", properties);

    }

}