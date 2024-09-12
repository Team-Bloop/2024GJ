using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;
using System;
public enum SoundType {
    PEACEFUL_BGM,
    END_OF_ALL_BGM,
    GAME_OVER_BGM,
    UI_BUTTON_INTERACT,
    COLLECT,
    PLAYER_HURT,
    DESTROY_MINISTORM
}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField]
    private GameObject bgmAudioObject;

    private AudioSource sfxAudioSource;
    private AudioSource bgmAudioSource;

    [SerializeField]
    private Slider m_Volume;
    [SerializeField]
    private Slider bgm_Volume;
    [SerializeField]
    private Slider sfx_Volume;

    [SerializeField]
    private SoundList[] soundList;

    private float masterVolume = 0.5f;
    private float bgmVolume = 0.5f;
    private float sfxVolume = 0.5f;

    void Awake()
    {
        if (instance == null) {
            instance = this;
        }
        volumeSet();
    }

    // Start is called before the first frame update
    void Start()
    {
        sfxAudioSource = GetComponent<AudioSource>();
        bgmAudioSource = bgmAudioObject.GetComponent<AudioSource>();
        AudioManager.PlayBGM();
    }

    public void volumeSet() {
        masterVolume = m_Volume.value;
        bgmVolume = bgm_Volume.value;
        sfxVolume = sfx_Volume.value;
    }

    // If we want multiple types of sounds
    //public static void PlayRandomSound(SoundType sound, float volume = 1) {
    //    AudioClip[] clips = instance.soundList[(int)sound].Sounds;
    //    AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
    //    instance.audioSource.PlayOneShot(randomClip, volume);
    //}

    public static void PlayBGM() 
    {
        AudioClip[] clips = instance.soundList[(int)SoundType.PEACEFUL_BGM].Sounds;
        instance.bgmAudioSource.loop = true;
        instance.bgmAudioSource.clip = clips[0];
        instance.bgmAudioSource.volume = instance.masterVolume * instance.bgmVolume;
        instance.bgmAudioSource.Play(0);
    }

    // For SFX on a trigger
    public static void PlaySound(SoundType sound, float volume = 1) {
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        instance.sfxAudioSource.PlayOneShot(clips[0], volume);
    }
    // For SFX on a trigger
    //public static void PlayCollectSound(SoundType sound, float volume = 1) 
    //{
    //    AudioClip[] clips = instance.soundList[(int)sound].Sounds;
    //}

    public void masterVolumeChange() {
        instance.masterVolume = m_Volume.value;
        instance.bgmAudioSource.volume = instance.masterVolume * instance.bgmVolume;
        instance.sfxAudioSource.volume = instance.masterVolume * instance.sfxVolume;
        //Debug.Log(instance.masterVolume);
    }

    public void bgmVolumeChange()
    {
        instance.bgmVolume = bgm_Volume.value;
        //Debug.Log(instance.bgmVolume);
        instance.bgmAudioSource.volume = instance.masterVolume * instance.bgmVolume;
    }

    public void sfxVolumeChange() {
        instance.sfxVolume = sfx_Volume.value;
        //Debug.Log(instance.sfxVolume);
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