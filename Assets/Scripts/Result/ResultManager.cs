using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using ProcRanking;
using System;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    [SerializeField] GameObject scoreTextObj;
    [SerializeField] GameObject highScoreTextObj;
    [SerializeField] GameObject sushiCountTextObj;
    TextMeshProUGUI scoreText;
    TextMeshProUGUI highScoreText;
    TextMeshProUGUI sushiCountText;

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

        score.SaveToProcRaAsync("SushiDataStore", "score");

        SoundManager.soundManager.PlayBGM(BGMType.Select);
        //PlayerPrefs.DeleteAll();

        if (highScore < score)
        {
            PlayerPrefs.SetInt("maxScore", score);
            isHighScore = true;
        }

        if(isHighScore)
        {
            conglaturateObj.SetActive(true);
            //scoreText.DOFade(0.0f, 1.0f).SetEase(Ease.InCubic).SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            conglaturateObj.SetActive(false);
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
        audioSource.PlayOneShot(clickSound);

        if (!isRanking)
        {
            isRanking = true;

            this.transform.DOLocalMove(new Vector3(-160f, -110f, 0f), 0.6f).SetEase(Ease.InOutSine);

            rankButtonImage.sprite = backButtonSprite;

            SpriteState spriteState = rankButton.spriteState; 
            spriteState.highlightedSprite = backButtonSprite_touch; 
            rankButton.spriteState = spriteState;



            var query = new ProcRaQuery<ProcRaData>("SushiDataStore")
                .SetLimit(10)
                .SetDescSort("score");

            query.FindAsync((List<ProcRaData> foundList, ProcRaException e) =>
            {
                if (e != null)
                {
                    // エラー発生時の処理
                }
                else
                {
                    // 検索成功時の処理例
                    for (int i = 0; i < foundList.Count; i++)
                    {

                        //32ビットintへキャスト
                        highScores[i] = Convert.ToInt32(foundList[i]["score"]);

                        //スコアをテキスト表示
                        highScoreTexts[i].GetComponent<TextMeshProUGUI>().text = highScores[i].ToString();

                    }
                }
            });

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
        Debug.Log("BackScene再読み込み");
        SceneManager.LoadSceneAsync(loadSceneName, LoadSceneMode.Additive);
    }
}
