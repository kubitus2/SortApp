using UnityEngine;
using UnityEngine.Audio;

public class SoundAssets : MonoBehaviour
{
    private static SoundAssets _Instance;

    public static SoundAssets Instance
    {
        get
        {
            if(_Instance == null)
                _Instance = Instantiate(Resources.Load<SoundAssets>("SoundAssets"));
        
            return _Instance;
        }
    }

    public GameObject ufo;
    public SoundAudioClip[] soundAudioClipArray;

    [System.Serializable]
    public class SoundAudioClip
    {
        public AudioManager.Sound sound;
        public AudioClip clip;
        public AudioMixerGroup targetAMGroup;
    }
}
