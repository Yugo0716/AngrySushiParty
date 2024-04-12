using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ResultManager : MonoBehaviour
{
    public GameObject scoreText;
    public GameObject highScoreText;
    public GameObject sushiCountText;
    public GameObject congImg;

    public int score = 3;
    int highScore = 0;
    public int sushiCount = 0;

    bool isHighScore = false;

    // Start is called before the first frame update
    void Start()
    {
        score = ScoreManager.score;
        sushiCount = GetSushiCount.count;
        highScore = PlayerPrefs.GetInt("maxScore", 0);

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


        scoreText.GetComponent<TextMeshProUGUI>().text = score.ToString();
        highScoreText.GetComponent<TextMeshProUGUI>().text = highScore.ToString();
        sushiCountText.GetComponent<TextMeshProUGUI>().text = sushiCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
