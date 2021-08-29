using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubesHandler : MonoBehaviour
{
    public delegate void SwapAction(GameObject a, GameObject b);
    public static event SwapAction OnSwap;

    [SerializeField]
    private int numOfCubes = 10;
    [SerializeField]
    private GameObject prefab;

    private bool isSortPlaying;


    private List<GameObject> cubes = new List<GameObject>();


    void Start()
    {
        SpawnCubes(numOfCubes);
        isSortPlaying = false;
    }

    void SpawnCubes(int n)
    {
        Vector3 pos = Vector3.zero;

        for (int i = 0; i < n; i++)
        {
            pos = this.transform.position + new Vector3 (1.5f * (i - numOfCubes / 2), 0.0f, 0.0f);
            GameObject nextCube = Instantiate(prefab, pos, Quaternion.identity);
            nextCube.GetComponent<Renderer>().material.color = GetRandomGrayscaleColor();
            nextCube.gameObject.layer = 3;
            cubes.Add(nextCube);
        }
    }

    Color GetRandomGrayscaleColor()
    {
        Color colour = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        float value = colour.grayscale;

        return new Color(value, value, value, 1.0f);
    }

    public void Sort()
    {
        //BubbleSort();


    }

    //SwapING ALGORITHMS

    void Swap(int a, int b)
    {
        GameObject temp = cubes[b];
        cubes[b] = cubes[a];
        cubes[a] = temp;


    }

    bool IsBrighter(GameObject a, GameObject b)
    {
        return a.GetComponent<Renderer>().material.color.grayscale > b.GetComponent<Renderer>().material.color.grayscale;
    }

    
    void BubbleSort()
    {
        for (int j = 0; j <= cubes.Count - 2; j++)
        {
            for (int i = 0; i <= cubes.Count - 2; i++)
            {
                if(IsBrighter(cubes[i], cubes[i+1]))
                {
                    Swap(i, i+1);
                    if(OnSwap != null)
                        OnSwap(cubes[i], cubes[i+1]);
                }

            }
        }
    }

    public void Test()
    {
        OnSwap(cubes[2], cubes[3]);
        OnSwap(cubes[5], cubes[4]);
        Debug.Log("Test");

    }



}
