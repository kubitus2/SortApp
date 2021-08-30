using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    [SerializeField]
    private AudioClip blip;

    private AudioSource source;

    void OnEnable()
    {
        UFO.OnObjectMove += PlayBlip;
    }
    
    void Awake()
    {
        source = this.GetComponent<AudioSource>();
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
