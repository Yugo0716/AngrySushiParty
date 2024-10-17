using DG.Tweening;
using ProcRanking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ProcRanking;
using TMPro;
using System;
using UnityEngine.SocialPlatforms.Impl;

public class RankingReceipt : MonoBehaviour
{
    bool isRanking = false;
    [SerializeField] GameObject rankButtonObj;
    Button rankButton;
    Image rankButtonImage;

    [SerializeField] GameObject highScoreTextObj;
    TextMeshProUGUI highScoreText;

    int highScore = 0;

    int[] highScores = new int[10];
    [SerializeField] GameObject[] highScoreTexts = new GameObject[10];

    [SerializeField] Sprite rankButtonSprite;
    [SerializeField] Sprite backButtonSprite;
    [SerializeField] Sprite rankButtonSprite_touch;
    [SerializeField] Sprite backButtonSprite_touch;

    AudioSource audioSource;
    public AudioClip clickSound;

    // Start is called before the first frame update
    void Start()
    {
        highScore = PlayerPrefs.GetInt("maxScore", 0);
        highScoreText = highScoreTextObj.GetComponent<TextMeshProUGUI>();

        rankButtonImage = rankButtonObj.GetComponent<Image>();
        rankButton = rankButtonObj.GetComponent<Button>();

        audioSource = GetComponent<AudioSource>();

        highScoreText.text = highScore.ToString();

        SoundManager.soundManager.PlayBGM(BGMType.Select);
    }

    

    public void RankingButtonClicked()
    {
        audioSource.PlayOneShot(clickSound);

        if (!isRanking)
        {
            isRanking = true;

            this.transform.DOLocalMove(new Vector3(176f, -27f, 0f), 0.8f).SetEase(Ease.InOutSine);
            //this.transform.DOLocalMove(new Vector3(0, 0f, 0f), 0.8f).SetEase(Ease.InOutSine);

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

            this.transform.DOLocalMove(new Vector3(176f, -719f, 0f), 0.8f).SetEase(Ease.InOutSine);

            rankButtonImage.sprite = rankButtonSprite;

            SpriteState spriteState = rankButton.spriteState;
            spriteState.highlightedSprite = rankButtonSprite_touch;
            rankButton.spriteState = spriteState;
        }
    }
}
