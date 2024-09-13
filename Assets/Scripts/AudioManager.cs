using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
public enum SoundType {
    PEACEFUL_BGM,
    END_OF_ALL_BGM,
    GAME_OVER_BGM,
    UI_BUTTON_INTERACT,
    COLLECT,
    PLAYER_HURT,
    PLAYER_UPGRADE,
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

    private bool bgmFade = false;
    private float bgmFadeValue = 0.001f;

    void Awake()
    {
        try {
            DontDestroyOnLoad(this);
        } catch (InvalidOperationException e) {
            Debug.Log(e.Message);
        }
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
        volumeSet();
    }

    // Start is called before the first frame update
    void Start()
    {
        sfxAudioSource = GetComponent<AudioSource>();
        bgmAudioSource = bgmAudioObject.GetComponent<AudioSource>();
        // this is the main bgm
        AudioManager.PlayMainBGM(0);
    }

    void Update() 
    {
        if (bgmFade == true) {
            BGMFade();
        } else {
            BGMUnfade();
        }
    }

    public void volumeSet() {
        instance.masterVolume = instance.m_Volume.value;
        instance.bgmVolume = instance.bgm_Volume.value;
        instance.sfxVolume = instance.sfx_Volume.value;
    }

    // If we want multiple types of sounds
    //public static void PlayRandomSound(SoundType sound, float volume = 1) {
    //    AudioClip[] clips = instance.soundList[(int)sound].Sounds;
    //    AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
    //    instance.audioSource.PlayOneShot(randomClip, volume);
    //}
    [Tooltip("value is dependent on which bgm to be played in the list of bgms starting from 0")]
    public static void PlayMainBGM(int value) {
        instance.bgmAudioSource.volume = instance.masterVolume * instance.bgmVolume;
        AudioClip[] clips = instance.soundList[(int)SoundType.PEACEFUL_BGM].Sounds;
        instance.bgmAudioSource.loop = true;
        instance.bgmAudioSource.clip = clips[value];
        instance.bgmAudioSource.Play(0);
    }
    public static void PlayGameOverBGM() {
        instance.bgmAudioSource.volume = instance.masterVolume * instance.bgmVolume;
        AudioClip[] clips = instance.soundList[(int)SoundType.GAME_OVER_BGM].Sounds;
        instance.bgmAudioSource.loop = true;
        instance.bgmAudioSource.clip = clips[0];
        instance.bgmAudioSource.Play(0);
    }

    IEnumerator BgmChanger(SoundType soundType) {
        yield return new WaitUntil(checkBGMVolZero);
        AudioClip[] clips = instance.soundList[(int)soundType].Sounds;
        instance.bgmAudioSource.loop = true;
        instance.bgmAudioSource.clip = clips[0];
        BGMFadeOff();
        instance.bgmAudioSource.Play(0);
    }
    private bool checkBGMVolZero() {
        return instance.bgmAudioSource.volume == 0;
    }
    public static void changeBGM(SoundType soundType) {
        Debug.Log("CHANGING BGM...");
        BGMFadeOn();
        instance.StartCoroutine(instance.BgmChanger(soundType));
        
    }
    public static void toggleBGMFade() {
        instance.bgmFade = !instance.bgmFade;
        Debug.Log("BGM FADE TOGGLED.");
    }
    public static void BGMFadeOn() {
        instance.bgmFade = true;
    }
    public static void BGMFadeOff() {
        instance.bgmFade = false;
    }
    public bool getBGMFade {
        get { return instance.bgmFade; }
    }

    private static void BGMFade() 
    {
        if (instance.bgmAudioSource.volume > 0) {
            instance.bgmAudioSource.volume -= instance.bgmFadeValue;
        }
    }
    private static void BGMUnfade()
    {
        if (instance.bgmAudioSource.volume < (instance.masterVolume * instance.bgmVolume)) {
            instance.bgmAudioSource.volume += instance.bgmFadeValue;
        }
    }
    
    // For SFX on a trigger
    public static void PlaySound(SoundType sound)
    {
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        instance.sfxAudioSource.volume = instance.masterVolume * instance.sfxVolume;
        instance.sfxAudioSource.PlayOneShot(clips[0], instance.sfxAudioSource.volume);
    }
    // For SFX on a trigger
    //public static void PlayCollectSound(SoundType sound, float volume = 1) 
    //{
    //    AudioClip[] clips = instance.soundList[(int)sound].Sounds;
    //}

    public void masterVolumeChange()
    {
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

    public void sfxVolumeChange()
    {
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