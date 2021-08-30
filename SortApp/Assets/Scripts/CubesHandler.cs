using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubesHandler : MonoBehaviour
{
    public delegate void SwapAction(GameObject a, GameObject b);
    public static event SwapAction OnSwap;

    public delegate void SortToggle();
    public static event SortToggle OnSortToggle;

    [SerializeField]
    [Range(5,20)]
    private int numOfCubes = 10;
    [SerializeField]
    private GameObject prefab;
    bool sortIsRunning;

    private List<GameObject> cubes = new List<GameObject>();
    private List<Color> initialColors = new List<Color>();

    private int listCount;

    [SerializeField]
    private Dropdown dropdown;

    void OnEnable()
    {
        UFO.OnSwapIsOver += SortIsRunning;
    }
    void Start()
    {
        SpawnCubes(numOfCubes);
        sortIsRunning = true;
    }

    void SpawnCube(int index)
    {
            Color colour = new Color(0f,0f,0f,1f);

            Vector3 pos = Vector3.zero;
            pos = this.transform.position + new Vector3 (1.5f * (index - numOfCubes / 2), 0.0f, 0.0f);
            GameObject nextCube = Instantiate(prefab, pos, Quaternion.identity);
            colour = GetRandomGrayscaleColor();
            nextCube.GetComponent<Renderer>().material.color = colour;
            nextCube.gameObject.layer = 3;
            cubes.Add(nextCube);
            initialColors.Add(colour);
    }
    void SpawnCubes(int n)
    {
        

        for (int i = 0; i < n; i++)
        {
            SpawnCube(i);
        }
        listCount = cubes.Count;
        
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
        switch(dropdown.value)
        {
            case 0:
                StartCoroutine(BubbleSort());
                break;
            case 1:
                StartCoroutine(CocktailSort());
                break;
            case 2:
                StartCoroutine(InsertionSort());
                break;
        }
    }

    //SORTING ALGORITHMS

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
        OnSortToggle();
        for (int j = 0; j <= listCount - 2; j++)
        {
            for (int i = 0; i <= listCount - 2; i++)
            {
                if(IsBrighter(cubes[i], cubes[i+1]))
                {
                    Swap(i, i+1);
                    yield return StartSwap(i, i+1);
                }

            }
        }
        OnSortToggle();
        yield return null;
    }

    IEnumerator InsertionSort()
    {
        OnSortToggle();
        for(int i = 1; i < listCount; i++)
        {
            int j = i;

            while(j > 0 && IsBrighter(cubes[j-1], cubes[j]))
            {
                Swap(j, j-1);
                yield return StartSwap(j, j - 1);

                j--;
            }
        }
        OnSortToggle();
    }

    IEnumerator CocktailSort()
    {
        OnSortToggle();

        bool swapped = true;
        int start = 0;
        int end = listCount;

        while(swapped)
        {
            swapped = false;

            for(int i = start; i < end - 1; ++i)
            {
                if(IsBrighter(cubes[i], cubes[i+1]))
                {
                    Swap(i, i+1);
                    yield return StartSwap(i, i+1);
                    swapped = true;
                }
            }

            if(swapped == false)
                break;

            end--;

            for (int i = end - 1; i >= start; i--)
            {
                if(IsBrighter(cubes[i], cubes[i+1]))
                {
                    Swap(i, i+1);
                    yield return StartSwap(i, i+1);
                    swapped = true;
                }
            }

            start++;
        }

        OnSortToggle();
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
        sortIsRunning = true;
    }

    void OnDisable()
    {
        UFO.OnSwapIsOver -= SortIsRunning;
    }

    public void Shuffle()
    {
        foreach (var cube in cubes)
        {
            cube.GetComponent<Renderer>().material.color = GetRandomGrayscaleColor();
        }
    }

    public void Revert()
    {
        for(int i=0; i < listCount; i++)
        {
            cubes[i].GetComponent<Renderer>().material.color = initialColors[i];
        }
    }



}
