using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UFO : MonoBehaviour
{
    [SerializeField]
    private float standardAltitude = 5.0f;
    [SerializeField]
    private float speed = 0.2f;
    [SerializeField]
    private GameObject tractorBeam;

    public delegate void SwapIsOver();
    public static event SwapIsOver OnSwapIsOver;

    enum FloatMode
    {
        Directly,
        Infront,
        Behind
    };

    void OnEnable()
    {
        CubesHandler.OnSwap += Swap;
    }
    
    IEnumerator SwapAnimation(GameObject a, GameObject b)
    {
        
        Vector3 aPosition = a.transform.position;
        Vector3 bPosition = b.transform.position;

        //move above and lift first cube
        yield return MoveAbove (aPosition, FloatMode.Directly);
        yield return Lift();

        //drop it infront of the second cube
        yield return MoveAbove (bPosition, FloatMode.Infront);
        yield return Drop();

        //lift the second cube
        yield return MoveAbove(bPosition, FloatMode.Directly);
        yield return Lift();

        //drop it in the first cube's initial position
        yield return MoveAbove (aPosition, FloatMode.Directly);
        yield return Drop();

        //lift the first cube
        yield return MoveAbove(bPosition, FloatMode.Infront);
        yield return Lift();

        //drop in at the freed position of the second cube
        yield return MoveAbove(bPosition, FloatMode.Directly);
        yield return Drop();

        //fire the event communicating the end of the swap sequence
        OnSwapIsOver();
        yield return null;
    }

    IEnumerator MoveAbove(Vector3 targetPlace, FloatMode mode)
    {
        Vector3 offset = Vector3.zero;

        switch(mode)
        {
            case FloatMode.Directly:
                offset = new Vector3 (0, standardAltitude, 0);
                break;
            case FloatMode.Infront:
                offset = new Vector3 (0, standardAltitude, -2.0f);
                break;
            case FloatMode.Behind:
                offset = new Vector3 (0, standardAltitude, 2.0f);
                break;
        }
        Vector3 target = targetPlace + offset;
        yield return MoveObject(this.transform, target);
    }

    IEnumerator MoveObject(Transform obj, Vector3 target)
    {
        obj.transform.DOMove(target, speed);
        while(obj.transform.position != target)
        {
            yield return null;
        }
    }
    
    IEnumerator Lift()
    {
        GameObject cargo;
        Vector3 cargoPos = new Vector3 (transform.position.x, transform.position.y - 1.0f, transform.position.z);

        tractorBeam.SetActive(true);
        
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);
        
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
            {
                cargo = hit.collider.gameObject;
                yield return MoveObject(cargo.transform, cargoPos);
                cargo.transform.parent = gameObject.transform;
            }
        }
        AudioManager.PlaySound(AudioManager.Sound.LiftSound);
        yield return null;
    }

    IEnumerator Drop()
    {
        Transform child = GetChildrenByLayer(3);
        Vector3 dropPosition = new Vector3 (child.position.x, 0.5f, child.position.z);

        child.parent = null;
        
        yield return MoveObject(child.transform, dropPosition);
        tractorBeam.SetActive(false);
        AudioManager.PlaySound(AudioManager.Sound.LiftSound);

        yield return null;
    }

    Transform GetChildrenByLayer(int layerID)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            if(child.gameObject.layer == layerID)
            {
                return child;
            }
        }
        return null;  
    }

    void Swap(GameObject a, GameObject b)
    {
        StartCoroutine(SwapAnimation(a, b));
    }

    void OnDisable()
    {
        CubesHandler.OnSwap -= Swap;
    }
}
