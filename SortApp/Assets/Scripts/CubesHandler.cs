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
    bool sortIsRunning;

    private List<GameObject> cubes = new List<GameObject>();

    void OnEnable()
    {
        UFO.OnSwapIsOver += SortIsRunning;
    }
    void Start()
    {
        SpawnCubes(numOfCubes);
        sortIsRunning = true;
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

    float GetGrayscaleValue(GameObject obj)
    {
        Color col = obj.GetComponent<Renderer>().material.color;

        return col.grayscale;
    }

    public void Sort()
    {
        StartCoroutine(BubbleSort());
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
        return GetGrayscaleValue(a) > GetGrayscaleValue(b);
    }

    
    IEnumerator BubbleSort()
    {
        for (int j = 0; j <= cubes.Count - 2; j++)
        {
            for (int i = 0; i <= cubes.Count - 2; i++)
            {
                if(IsBrighter(cubes[i], cubes[i+1]))
                {
                    Swap(i, i+1);
                    yield return StartSwap(i, i+1);
                }

            }
        }
        yield return null;
    }


    IEnumerator StartSwap(int a, int b)
    {
        while(!sortIsRunning)
        {
            yield return null;
        }
        sortIsRunning = false;

        OnSwap(cubes[a], cubes[b]);
        yield return null;
    }

    void SortIsRunning()
    {
        Debug.Log("Sort is over");
        sortIsRunning = true;
    }

    void OnDisable()
    {
        UFO.OnSwapIsOver -= SortIsRunning;
    }



}
