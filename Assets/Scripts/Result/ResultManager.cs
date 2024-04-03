using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ResultManager : MonoBehaviour
{
    public GameObject scoreText;
    public GameObject highScoreText;
    public GameObject congText;

    public int score;
    int highScore = 0;

    bool isHighScore = false;

    // Start is called before the first frame update
    void Start()
    {
        highScore = PlayerPrefs.GetInt("maxScore", 0);

        if (highScore < score)
        {
            PlayerPrefs.SetInt("maxScore", score);
            isHighScore = true;
        }

        if(isHighScore)
        {
            congText.SetActive(true);
        }
        else
        {
            congText.SetActive(false);
        }


        scoreText.GetComponent<TextMeshProUGUI>().text = "score: " + score.ToString();
        highScoreText.GetComponent<TextMeshProUGUI>().text = "highscore: " + highScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
