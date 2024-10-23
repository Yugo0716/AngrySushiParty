using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum BGMType
{
    None,
    Title,
    Select,
    Play
}

public enum SEType
{
    ButtonClick,
    StartPanel,
    FinishPanel,
    SushiClick,
    BubbleAppear,
    Get,
    LifeMinus,
    Alart,
}
public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioClip TitleBGM;
    [SerializeField] AudioClip SelectBGM;
    [SerializeField] AudioClip GameBGM;

    [SerializeField] AudioClip ButtonClickSE;
    [SerializeField] AudioClip StartPanelSE;
    [SerializeField] AudioClip FinishPanelSE;
    [SerializeField] AudioClip SushiClickSE;
    [SerializeField] AudioClip BubbleAppearSE;
    [SerializeField] AudioClip GetSE;
    [SerializeField] AudioClip LifeMinusSE;
    [SerializeField] AudioClip AlartSE;

    public AudioSource audioSource;

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
            if(audioSource.loop == false)
            {
                audioSource.loop = true;
            }

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

    public void SEPlay(SEType type)
    {
        if(type == SEType.ButtonClick)
        {
            audioSource.PlayOneShot(ButtonClickSE);
        }
        else if(type == SEType.StartPanel)
        {
            audioSource.PlayOneShot(StartPanelSE);
        }
        else if(type == SEType.FinishPanel)
        {
            audioSource.PlayOneShot(FinishPanelSE);
        }
        else if (type == SEType.ButtonClick)
        {
            audioSource.PlayOneShot(ButtonClickSE);
        }
        else if (type == SEType.SushiClick)
        {
            audioSource.PlayOneShot(SushiClickSE);
        }
        else if (type == SEType.BubbleAppear)
        {
            audioSource.PlayOneShot(BubbleAppearSE);
        }
        else if (type == SEType.Get)
        {
            audioSource.PlayOneShot(GetSE);
        }
        else if (type == SEType.LifeMinus)
        {
            audioSource.PlayOneShot(LifeMinusSE);
        }
        else if (type == SEType.Alart)
        {
            audioSource.PlayOneShot(AlartSE);
        }
    }
}
