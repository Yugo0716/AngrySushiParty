using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using unityroom.Api;

public class ResultManager : MonoBehaviour
{
    [SerializeField] GameObject scoreTextObj;
    [SerializeField] GameObject highScoreTextObj;
    [SerializeField] GameObject sushiCountTextObj;
    TextMeshProUGUI scoreText;
    TextMeshProUGUI highScoreText;
    TextMeshProUGUI sushiCountText;

    [SerializeField] GameObject touchTextObj;

    //[SerializeField] GameObject congImg;

    public int score = 3;
    int highScore = 0;
    int sushiCount = 0;

    bool isRanking = false;
    [SerializeField] GameObject rankButtonObj;
    Button rankButton;
    Image rankButtonImage;

    [SerializeField] Sprite rankButtonSprite;
    [SerializeField] Sprite backButtonSprite;
    [SerializeField] Sprite rankButtonSprite_touch;
    [SerializeField] Sprite backButtonSprite_touch;

    [SerializeField] GameObject conglaturateObj;

    [SerializeField] GameObject RankObj;
    Image rankImage;
    [SerializeField] Sprite RankSImage;
    [SerializeField] Sprite RankAImage;
    [SerializeField] Sprite RankBImage;
    [SerializeField] Sprite RankCImage;

    bool isHighScore = false;

    int[] highScores = new int[10];
    [SerializeField] GameObject[] highScoreTexts = new GameObject[10];

    AudioSource audioSource;
    public AudioClip clickSound;

    // Start is called before the first frame update
    void Start()
    {

        UnduplicateLoad("BackScene");
        score = ScoreManager.score;
        sushiCount = GetSushiCount.count;
        highScore = PlayerPrefs.GetInt("maxScore", 0);

        scoreText = scoreTextObj.GetComponent<TextMeshProUGUI>();
        highScoreText = highScoreTextObj.GetComponent<TextMeshProUGUI>();
        sushiCountText = sushiCountTextObj.GetComponent<TextMeshProUGUI>();

        rankButtonImage = rankButtonObj.GetComponent<Image>();
        rankButton = rankButtonObj.GetComponent<Button>();

        rankImage = RankObj.GetComponent<Image>();
        if (score < 8000)
        {
            rankImage.sprite = RankCImage;
        }
        else if(score< 15000)
        {
            rankImage.sprite = RankBImage;
        }
        else if (score < 20000)
        {
            rankImage.sprite = RankAImage;
        }
        else
        {
            rankImage.sprite = RankSImage;
        }

        audioSource = GetComponent<AudioSource>();
        SoundManager.soundManager.PlayBGM(BGMType.Select);

        if(TouchManager.isTouch == false)
        {
            touchTextObj.SetActive(false);


            if (highScore < score)
            {
                PlayerPrefs.SetInt("maxScore", score);
                
                isHighScore = true;
            }
            UnityroomApiClient.Instance.SendScore(1, score, ScoreboardWriteMode.HighScoreDesc);
            if (isHighScore)
            {
                conglaturateObj.SetActive(true);
                //scoreText.DOFade(0.0f, 1.0f).SetEase(Ease.InCubic).SetLoops(-1, LoopType.Yoyo);
            }
            else
            {
                conglaturateObj.SetActive(false);
            }
        }
        else
        {
            conglaturateObj.SetActive(false);
            touchTextObj.SetActive(true);
        }
        


        scoreText.text = score.ToString();
        highScoreText.text = highScore.ToString();
        sushiCountText.text = sushiCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RankingButtonClicked()
    {
        SoundManager.soundManager.SEPlay(SEType.ButtonClick);

        if (!isRanking)
        {
            isRanking = true;

            this.transform.DOLocalMove(new Vector3(-160f, -110f, 0f), 0.6f).SetEase(Ease.InOutSine);

            rankButtonImage.sprite = backButtonSprite;

            SpriteState spriteState = rankButton.spriteState; 
            spriteState.highlightedSprite = backButtonSprite_touch; 
            rankButton.spriteState = spriteState;
        }
        else
        {
            isRanking = false;

            this.transform.DOLocalMove(new Vector3(-160f, 230f, 0f), 0.6f).SetEase(Ease.InOutSine);

            rankButtonImage.sprite = rankButtonSprite;

            SpriteState spriteState = rankButton.spriteState;
            spriteState.highlightedSprite = rankButtonSprite_touch;
            rankButton.spriteState = spriteState;
        }
    }

    void UnduplicateLoad(string loadSceneName)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);

            if (scene.name == loadSceneName)
            {
                return;
            }
        }
        Debug.Log("BackScene�ēǂݍ���");
        SceneManager.LoadSceneAsync(loadSceneName, LoadSceneMode.Additive);
    }
}
