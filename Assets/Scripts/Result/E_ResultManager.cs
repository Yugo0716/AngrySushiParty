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
    TextMeshProUGUI scoreText;
    TextMeshProUGUI highScoreText;
    TextMeshProUGUI timeText;

    [SerializeField] GameObject congImg;

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

        audioSource = GetComponent<AudioSource>();

        //E_score.SaveToProcRaAsync("SushiDataStore", "E_score");



        if (highScore < E_score)
        {
            PlayerPrefs.SetInt("E_maxScore", E_score);
            isHighScore = true;
        }

        if (isHighScore)
        {
            congImg.SetActive(true);
            scoreText.DOFade(0.0f, 1.0f).SetEase(Ease.InCubic).SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            congImg.SetActive(false);
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
        audioSource.PlayOneShot(clickSound);

        if (!isRanking)
        {
            isRanking = true;

            this.transform.DOLocalMove(new Vector3(-160f, -110f, -3750f), 0.7f).SetEase(Ease.InOutSine);

            rankButtonImage.sprite = backButtonSprite;

            SpriteState spriteState = rankButton.spriteState;
            spriteState.highlightedSprite = backButtonSprite_touch;
            rankButton.spriteState = spriteState;


            /*var query = new ProcRaQuery<ProcRaData>("SushiDataStore")
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
                        highScores[i] = Convert.ToInt32(foundList[i]["score"]);

                        //スコアをテキスト表示
                        highScoreTexts[i].GetComponent<TextMeshProUGUI>().text = highScores[i].ToString();

                    }
                }
            });*/

        }
        else
        {
            isRanking = false;

            this.transform.DOLocalMove(new Vector3(-160f, 230f, -3750f), 0.8f).SetEase(Ease.InOutSine);

            rankButtonImage.sprite = rankButtonSprite;

            SpriteState spriteState = rankButton.spriteState;
            spriteState.highlightedSprite = rankButtonSprite_touch;
            rankButton.spriteState = spriteState;
        }
    }
}
