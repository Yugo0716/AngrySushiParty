using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using ProcRanking;
using System;
using DG.Tweening;
using UnityEngine.UI;

public class E_ResultManager : MonoBehaviour
{
    [SerializeField] GameObject scoreTextObj;
    [SerializeField] GameObject highScoreTextObj;
    [SerializeField] GameObject timeTextObj;
    [SerializeField] GameObject touchTextObj;
    TextMeshProUGUI scoreText;
    TextMeshProUGUI highScoreText;
    TextMeshProUGUI timeText;

    public int E_score = 3;
    int highScore = 0;
    float time = 0;

    bool isRanking = false;
    [SerializeField] GameObject rankButtonObj;
    Image rankButtonImage;
    Button rankButton;
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
        E_score = ScoreManager.score;
        highScore = PlayerPrefs.GetInt("E_maxScore", 0);
        time = TimeManager.time;

        scoreText = scoreTextObj.GetComponent<TextMeshProUGUI>();
        highScoreText = highScoreTextObj.GetComponent<TextMeshProUGUI>();
        timeText = timeTextObj.GetComponent<TextMeshProUGUI>();

        rankButtonImage = rankButtonObj.GetComponent<Image>();
        rankButton = rankButtonObj.GetComponent<Button>();

        rankImage = RankObj.GetComponent<Image>();
        if (E_score < 40)
        {
            rankImage.sprite = RankCImage;
        }
        else if (E_score < 100)
        {
            rankImage.sprite = RankBImage;
        }
        else if (E_score < 200)
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
            E_score.SaveToProcRaAsync("SushiDataStore", "E_score");

            if (highScore < E_score)
            {
                PlayerPrefs.SetInt("E_maxScore", E_score);
                isHighScore = true;
            }

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
            touchTextObj.SetActive(true );
        }
        

        
        scoreText.text = E_score.ToString();
        highScoreText.text = highScore.ToString();
        timeText.text = (Mathf.Ceil(time)).ToString();
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


            var query = new ProcRaQuery<ProcRaData>("SushiDataStore")
                .SetLimit(10)
                .SetDescSort("E_score");

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
                        highScores[i] = Convert.ToInt32(foundList[i]["E_score"]);

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
}
