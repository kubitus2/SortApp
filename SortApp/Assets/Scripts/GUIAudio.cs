using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class GUIAudio : MonoBehaviour
{
    [SerializeField]
    private AudioClip sound;

    private Button button {get {return GetComponent<Button>();}}
    private AudioSource source {get {return GetComponent<AudioSource>();}}

    void Awake()
    {
        gameObject.AddComponent<AudioSource>();
        source.clip = sound;
        source.playOnAwake = false;

        button.onClick.AddListener(() => PlaySound());
    }

    void PlaySound()
    {
        source.PlayOneShot(sound);
    }
}
