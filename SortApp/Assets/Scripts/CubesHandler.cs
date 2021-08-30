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
    private int numOfCubes = 12;
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private Dropdown dropdown;

    bool sortIsNotRunning;
    private List<GameObject> cubes = new List<GameObject>();
    private List<Color> initialColors = new List<Color>();
    private int listCount;

    void OnEnable()
    {
        UFO.OnSwapIsOver += SortIsRunning;
    }

    void Awake()
    {
        SpawnCubes(numOfCubes);
        sortIsNotRunning = true;
    }

    Vector3 CubePositionOffset(int index)
    {
        return new Vector3 (1.5f * (index - numOfCubes / 2), 0f, 0f);
    }

    void SpawnCube(int index)
    {
        Color colour = new Color(0f,0f,0f,1f);

        Vector3 pos = Vector3.zero;
        pos = this.transform.position + CubePositionOffset(index);
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

    //choose sort algorithm
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
                StartCoroutine(OptimisedGnomeSort());
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
                    
                    yield return StartSwap(i, i+1);
                    Swap(i, i+1);
                }
            }
        }
        yield return new WaitForSeconds(1);
        OnSortToggle();
    }

    IEnumerator OptimisedGnomeSort()
    {
        OnSortToggle();

        for(int i = 0; i < listCount; i++)
        {
            yield return GnomeSort(i);
        }
        yield return new WaitForSeconds(1);
        OnSortToggle();
    }

    IEnumerator GnomeSort(int upperBound)
    {
        int pos = upperBound;

        while (pos > 0 && IsBrighter(cubes[pos - 1], cubes[pos]))
        {
            
            yield return StartSwap(pos, pos - 1);
            Swap(pos, pos - 1);
            pos--;
        }
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
                    
                    yield return StartSwap(i, i+1);
                    Swap(i, i+1);
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
                    yield return StartSwap(i, i+1);
                    Swap(i, i+1);
                    swapped = true;
                }
            }
        }
        yield return new WaitForSeconds(1);
        OnSortToggle();
    }

    IEnumerator StartSwap(int a, int b)
    {
        while(!sortIsNotRunning)
        {
            yield return null;
        }
        sortIsNotRunning = false;

        OnSwap(cubes[a], cubes[b]);
        yield return null;
    }

    void SortIsRunning()
    {
        sortIsNotRunning = true;
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

    void OnDisable()
    {
        UFO.OnSwapIsOver -= SortIsRunning;
    }
}
