using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private Slider m_Volume;
    [SerializeField]
    private Slider sfx_Volume;
    [SerializeField]
    private Slider bgm_Volume;

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Play(string name)
    { 
        
    }
}
