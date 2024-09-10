using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;
using System;
public enum SoundType {
    BGM,
    UI_BUTTON_INTERACT,
    COLLECT,
    PLAYER_HURT,
    DESTROY_MINISTORM
}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private AudioSource audioSource;

    [SerializeField]
    private SoundList[] soundList;

    [SerializeField]
    private Slider m_Volume;
    [SerializeField]
    private Slider bgm_Volume;
    [SerializeField]
    private Slider sfx_Volume;

    private float masterVolume;
    private float bgmVolume;
    private float sfxVolume;

    void Awake()
    {
        if (instance == null) {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // For SFX on a trigger
    public static void PlayRandomSound(SoundType sound, float volume = 1) {
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        instance.audioSource.PlayOneShot(randomClip, volume);
    }

    // For SFX on a trigger
    public static void PlaySound(SoundType sound, float volume = 1) {
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        instance.audioSource.PlayOneShot(clips[0], volume);
    }
    // For SFX on a trigger
    public static void PlayCollectSound(SoundType sound, float volume = 1) 
    {
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;

    }

    public void masterVolumeChange() {
        instance.masterVolume = m_Volume.value;
        Debug.Log(instance.masterVolume);
    }

    public void bgmVolumeChange(float volume)
    {
        instance.bgmVolume = bgm_Volume.value;
        Debug.Log(instance.bgmVolume);
    }

    public void sfxVolumeChange(float volume) {
        instance.sfxVolume = sfx_Volume.value;
        Debug.Log(instance.sfxVolume);
    }

#if UNITY_EDITOR
    private void OnEnable() {
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref soundList, names.Length);
        for (int i = 0; i < soundList.Length; i++) {
            soundList[i].name = names[i];
        }
    }
#endif

}

[Serializable]
public struct SoundList 
{
    public AudioClip[] Sounds { get => sounds; }
    [HideInInspector] public string name;
    [SerializeField] private AudioClip[] sounds;
}