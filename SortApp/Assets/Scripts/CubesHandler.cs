using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubesHandler : MonoBehaviour
{
    [SerializeField]
    private int numOfCubes = 10;

    [SerializeField]
    private GameObject prefab;


    private List<GameObject> cubes = new List<GameObject>();


    // Start is called before the first frame update
    void Awake()
    {
        SpawnCubes(numOfCubes);
    }

    void SpawnCubes(int n)
    {
        Vector3 pos = Vector3.zero;

        for (int i = 0; i < n; i++)
        {
            pos = this.transform.position + new Vector3 (1.5f * (i - numOfCubes / 2), 0.0f, 0.0f);
            GameObject nextCube = Instantiate(prefab, pos, Quaternion.identity);
            nextCube.GetComponent<Renderer>().material.color = GetRandomGrayscaleColor();
            cubes.Add(nextCube);
        }
    }

    Color GetRandomGrayscaleColor()
    {
        Color colour = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        float value = colour.grayscale;

        return new Color(value, value, value, 1.0f);
    }
}
