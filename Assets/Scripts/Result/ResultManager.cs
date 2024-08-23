using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using ProcRanking;
using System;
using DG.Tweening;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    [SerializeField] GameObject scoreTextObj;
    [SerializeField] GameObject highScoreTextObj;
    [SerializeField] GameObject sushiCountTextObj;
    TextMeshProUGUI scoreText;
    TextMeshProUGUI highScoreText;
    TextMeshProUGUI sushiCountText;

    [SerializeField] GameObject congImg;

    public int score = 3;
    int highScore = 0;
    int sushiCount = 0;

    bool isRanking = false;
    [SerializeField] GameObject rankButtonObj;
    Image rankButtonImage;
    [SerializeField] Sprite rankButtonSprite;
    [SerializeField] Sprite backButtonSprite;

    bool isHighScore = false;

    int[] highScores = new int[10];
    [SerializeField] GameObject[] highScoreTexts = new GameObject[10];

    AudioSource audioSource;
    public AudioClip clickSound;

    // Start is called before the first frame update
    void Start()
    {
        score = ScoreManager.score;
        sushiCount = GetSushiCount.count;
        highScore = PlayerPrefs.GetInt("maxScore", 0);

        scoreText = scoreTextObj.GetComponent<TextMeshProUGUI>();
        highScoreText = highScoreTextObj.GetComponent<TextMeshProUGUI>();
        sushiCountText = sushiCountTextObj.GetComponent<TextMeshProUGUI>();

        rankButtonImage = rankButtonObj.GetComponent<Image>();

        audioSource = GetComponent<AudioSource>();

        score.SaveToProcRaAsync("SushiDataStore", "score");
        


        if (highScore < score)
        {
            PlayerPrefs.SetInt("maxScore", score);
            isHighScore = true;
        }

        if(isHighScore)
        {
            congImg.SetActive(true);
        }
        else
        {
            congImg.SetActive(false);
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

            this.transform.DOLocalMove(new Vector3(-160f, -110f, -3750f), 0.7f).SetEase(Ease.InOutSine);

            rankButtonImage.sprite = backButtonSprite;

            
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

            this.transform.DOLocalMove(new Vector3(-160f, 230f, -3750f), 0.8f).SetEase(Ease.InOutSine);

            rankButtonImage.sprite = rankButtonSprite;
        }
    }
}
