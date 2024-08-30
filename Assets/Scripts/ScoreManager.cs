using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] GameObject scoreTextObj;
    TextMeshProUGUI scoreText;

    public static int score = 0;

    GameMode gameMode;

    

    public enum ScoreType //スコアを得るための種類
    {
        sushi, //流れる寿司
        bubbleNormal, //通常の吹き出し
        bubbleOrder //注文レーンの寿司
    };

    // Start is called before the first frame update
    void Start()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("canvas");
        gameMode = canvas.GetComponent<GameMode>();

        scoreText = scoreTextObj.GetComponent<TextMeshProUGUI>();

        score = 0;
        UpdateScore();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ScorePlus(ScoreType type, float bonus)
    {
        if (gameMode.isScored)
        {
            if (type == ScoreType.sushi)
            {
                score += 300;
            }
            else if (type == ScoreType.bubbleNormal)
            {
                score += 100 + (int)bonus;
            }
            else if (type == ScoreType.bubbleOrder)
            {
                score += 200 + (int)bonus;
            }
        }
        else
        {
            score++;
        }
        
        UpdateScore();
    }

    public void CountPlus()
    {
        GetSushiCount.count++;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "スコア：" + score.ToString();
    }
}
