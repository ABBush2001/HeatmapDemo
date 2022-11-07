/*NOTE: This code is adopted from a heatmap designed 
 * by Alan Zucconi, whose personal blog can be found 
 * at www.alanzucconi.com*/
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class Heatmap : MonoBehaviour
{
    public Vector4[] positions;
    public Vector4[] properties;

    public Material material;

    public TextAsset jsonFile;

    public DateTime convertedString;
    public List<DateTime> dates = new List<DateTime>();
    public int numDates;

    public Slider dateSlider;
    public int curSliderVal = -1;

    public Vector3 anchor;

    void Start()
    {
        anchor = GameObject.Find("Anchor1").GetComponent<LocationMarker>().LatLon;

        positions = new Vector4[100];
        properties = new Vector4[100];

        for(int k = 0; k < positions.Length; k++)
        {
            positions[k] = new Vector4(0, 0, 0, 0);
            properties[k] = new Vector4(0, 0, 0, 0);
        }

        GetUniqueDates();

        dateSlider.minValue = 0;
        dateSlider.maxValue = numDates - 1;

        int i = 0;

        //takes in a list of (x, y) coordinates, converts them to Unity world positions based on anchors, then assigns a radius and a value for temp based on input data

        Temps tempsInJson = JsonUtility.FromJson<Temps>(jsonFile.text);

        foreach (Temp temp in tempsInJson.temps)
        {
            
            if (i == 0)
            {
                convertedString = DateTime.Parse(temp.date);
                
                Vector3 pos = LatLon.ConvertCoordToPos(anchor.x, anchor.y, temp.x, temp.y);
                positions[i] = new Vector4(pos.x, pos.y, 0, 0);
                properties[i] = new Vector4(50, temp.tmp / 100, 0, 0);
            }
            else
            {
                if(DateTime.Parse(temp.date).Equals(convertedString))
                {
                    Vector3 pos = LatLon.ConvertCoordToPos(anchor.x, anchor.y, temp.x, temp.y);
                    positions[i] = new Vector4(pos.x, pos.y, 0, 0);
                    properties[i] = new Vector4(50, temp.tmp / 100, 0, 0);
                }
            }

            i++;

        }

        material.SetInt("_Points_Length", 50);
        material.SetVectorArray("_Points", positions);
        material.SetVectorArray("_Properties", properties);

    }

    void Update()
    {
       //check if slider value is a whole number - if so, update it to the appropriate date
       if(dateSlider.value % 1 == 0 && dateSlider.value > curSliderVal)
        {
            Temps tempsInJson = JsonUtility.FromJson<Temps>(jsonFile.text);
            int i = 0;

            foreach(Temp temp in tempsInJson.temps)
            {
                if (DateTime.Parse(temp.date).Equals(dates[(int)dateSlider.value]))
                {

                    for(int j = 0; j < positions.Length; j++)
                    {
                        Vector3 coords = LatLon.ConvertCoordToPos(anchor.x, anchor.y, temp.x, temp.y);
                        
                        if(positions[j].x == coords.x && positions[j].y == coords.y)
                        {
                            Vector3 pos = LatLon.ConvertCoordToPos(anchor.x, anchor.y, temp.x, temp.y);
                            properties[j] = new Vector4(50, temp.tmp / 100, 0, 0);
                        }
                    }
                    

                    material.SetInt("_Points_Length", 50);
                    material.SetVectorArray("_Points", positions);
                    material.SetVectorArray("_Properties", properties);
                }

                i++;
            }

            curSliderVal = (int)dateSlider.value;
        }
       else if(dateSlider.value % 1 == 0 && dateSlider.value < curSliderVal)
        {
            Temps tempsInJson = JsonUtility.FromJson<Temps>(jsonFile.text);
            int i = 0;

            foreach (Temp temp in tempsInJson.temps)
            {
                if (DateTime.Parse(temp.date).Equals(dates[(int)dateSlider.value]))
                {
                    for (int j = 0; j < positions.Length; j++)
                    {
                        Vector3 coords = LatLon.ConvertCoordToPos(anchor.x, anchor.y, temp.x, temp.y);

                        if (positions[j].x == coords.x && positions[j].y == coords.y)
                        {
                            Vector3 pos = LatLon.ConvertCoordToPos(anchor.x, anchor.y, temp.x, temp.y);
                            properties[j] = new Vector4(50, temp.tmp / 100, 0, 0);
                        }
                    }

                    material.SetInt("_Points_Length", 50);
                    material.SetVectorArray("_Points", positions);
                    material.SetVectorArray("_Properties", properties);
                }

                i++;
            }

            curSliderVal = (int)dateSlider.value;
        }

    }

    private void GetUniqueDates()
    {
        DateTime prevDate = new DateTime();
        bool firstLoop = true;

        Temps tempsInJson = JsonUtility.FromJson<Temps>(jsonFile.text);

        foreach (Temp temp in tempsInJson.temps)
        {
            if(firstLoop)
            {
                numDates++;
                firstLoop = false;
                prevDate = DateTime.Parse(temp.date);
                dates.Add(prevDate);
            }
            else
            {
                if (DateTime.Parse(temp.date).Equals(prevDate))
                {
                }
                else
                {
                    numDates++;
                    dates.Add(DateTime.Parse(temp.date));
                }

                prevDate = DateTime.Parse(temp.date);
            }
        }

    }

}