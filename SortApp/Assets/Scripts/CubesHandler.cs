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

    bool swapIsNotRunning;
    private List<GameObject> cubes = new List<GameObject>();
    private int listCount;

    void OnEnable()
    {
        UFO.OnSwapIsOver += SwapIsNotRunning;
    }

    void Awake()
    {
        SpawnCubes(numOfCubes);
        swapIsNotRunning = true;
    }

    void SpawnCube(int index)
    {
        Color colour = new Color(0f,0f,0f,1f);

        Vector3 pos = Vector3.zero;
        pos = this.transform.position + Utils.CubePositionOffset(index, numOfCubes);

        GameObject nextCube = Instantiate(prefab, pos, Quaternion.identity);

        colour = Utils.GetRandomGrayscaleColor();
        nextCube.GetComponent<Renderer>().material.color = colour;
        nextCube.gameObject.layer = 3;

        cubes.Add(nextCube);
    }

    void SpawnCubes(int n)
    {
        for (int i = 0; i < n; i++)
        {
            SpawnCube(i);
        }

        listCount = cubes.Count;    
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
    IEnumerator BubbleSort()
    {
        OnSortToggle();

        for (int j = 0; j <= listCount - 2; j++)
        {
            for (int i = 0; i <= listCount - 2; i++)
            {
                if(Utils.IsBrighter(cubes[i], cubes[i+1]))
                {
                    yield return StartSwap(i, i+1);
                    Utils.Swap(i, i+1, cubes);
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

        while (pos > 0 && Utils.IsBrighter(cubes[pos - 1], cubes[pos]))
        {
            
            yield return StartSwap(pos, pos - 1);
            Utils.Swap(pos, pos - 1, cubes);
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
                if(Utils.IsBrighter(cubes[i], cubes[i+1]))
                {
                    
                    yield return StartSwap(i, i+1);
                    Utils.Swap(i, i+1, cubes);
                    swapped = true;
                }
            }

            if(swapped == false)
                break;

            end--;

            for (int i = end - 1; i >= start; i--)
            {
                if(Utils.IsBrighter(cubes[i], cubes[i+1]))
                {
                    yield return StartSwap(i, i+1);
                    Utils.Swap(i, i+1, cubes);
                    swapped = true;
                }
            }
        }
        yield return new WaitForSeconds(1);
        OnSortToggle();
    }

    IEnumerator StartSwap(int a, int b)
    {
        while(!swapIsNotRunning)
        {
            yield return null;
        }
        swapIsNotRunning = false;

        OnSwap(cubes[a], cubes[b]);
        yield return null;
    }

    void SwapIsNotRunning()
    {
        swapIsNotRunning = true;
    }

    public void Shuffle()
    {
        foreach (var cube in cubes)
        {
            cube.GetComponent<Renderer>().material.color = Utils.GetRandomGrayscaleColor();
        }
    }

    void OnDisable()
    {
        UFO.OnSwapIsOver -= SwapIsNotRunning;
    }
}
