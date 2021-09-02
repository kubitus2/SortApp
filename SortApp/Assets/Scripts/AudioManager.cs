using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public static class AudioManager
{
    private static GameObject oneShotGameObject;
    private static AudioSource oneShotAudioSource;
    private static List<Button> btns = new List<Button>();

    public enum Sound
    {
        LiftSound,
        ClickSound
    }
    
    public static void PlaySound(Sound sound)
    {
        if(oneShotGameObject == null)
        {
            oneShotGameObject = new  GameObject("One shot sound");
            oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();
        }

        AudioClip clip = GetAudioClip(sound);
        oneShotGameObject.GetComponent<AudioSource>().outputAudioMixerGroup = GetMixerGroup(sound);
        oneShotAudioSource.PlayOneShot(clip);
    }

    private static AudioClip GetAudioClip(Sound sound)
    {
        AudioClip clip = null;

        foreach (SoundAssets.SoundAudioClip audioClip in SoundAssets.Instance.soundAudioClipArray)
        {
            if(audioClip.sound == sound)
            {
                clip = audioClip.clip;
            }
        }

        return clip;
    }

    private static AudioMixerGroup GetMixerGroup(Sound sound)
    {
        AudioMixerGroup audioMixerGroup = null;

        foreach (SoundAssets.SoundAudioClip audioClip in SoundAssets.Instance.soundAudioClipArray)
        {
            if(audioClip.sound == sound)
            {
                audioMixerGroup = audioClip.targetAMGroup;
            }
        }

        return audioMixerGroup;
    }
}
