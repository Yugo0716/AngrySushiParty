using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BGMType
{
    None,
    Title,
    Select,
    Play
}
public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioClip TitleBGM;
    [SerializeField] AudioClip SelectBGM;
    [SerializeField] AudioClip GameBGM;

    AudioSource audioSource;

    public static SoundManager soundManager;
    public static BGMType playingBGM = BGMType.None;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (soundManager == null)
        {
            soundManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayBGM(BGMType type)
    {
        if(type != playingBGM)
        {
            if(type == BGMType.Title)
            {
                audioSource.clip = TitleBGM;
            }
            else if(type == BGMType.Select)
            {
                audioSource.clip = SelectBGM;
            }
            else if(type == BGMType.Play)
            {
                audioSource.clip = GameBGM;
            }
            audioSource.Play();
        }
    }

    public void StopBGM()
    {
        audioSource.Stop();
        playingBGM = BGMType.None;
    }
}
