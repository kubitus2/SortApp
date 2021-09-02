using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static Vector3 CubePositionOffset(int index, int n)
    {
        return new Vector3 (1.5f * (index - n / 2), 0f, 0f);
    }

    public static Transform GetChildrenByLayer(Transform obj, int layerID)
    {
        for (int i = 0; i < obj.childCount; i++)
        {
            Transform child = obj.GetChild(i);

            if(child.gameObject.layer == layerID)
            {
                return child;
            }
        }
        return null;  
    }

    public static Color GetRandomGrayscaleColor()
    {
        Color colour = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        float value = colour.grayscale;

        return new Color(value, value, value, 1.0f);
    }

    public static float GetGrayscaleValue(GameObject obj)
    {
        Color col = obj.GetComponent<Renderer>().material.color;

        return col.grayscale;
    }

    public static bool IsBrighter(GameObject a, GameObject b)
    {
        return GetGrayscaleValue(a) > GetGrayscaleValue(b);
    }

    public static void Swap(int a, int b, List<GameObject> obj)
    {
        GameObject temp = obj[b];
        obj[b] = obj[a];
        obj[a] = temp;
    }

}
