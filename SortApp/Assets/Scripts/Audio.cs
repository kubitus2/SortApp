using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    [SerializeField]
    private AudioClip blip;

    private AudioSource source {get {return GetComponent<AudioSource>();}}

    void OnEnable()
    {
        UFO.OnObjectMove += PlayBlip;
    }

    void PlayBlip()
    {
        source.PlayOneShot(blip);
    }

    void OnDisable()
    {
        UFO.OnObjectMove -= PlayBlip;
    }
}
