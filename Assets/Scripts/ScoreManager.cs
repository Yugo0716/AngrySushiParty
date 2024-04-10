using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public GameObject scoreText;
    public int score = 0;

    

    public enum ScoreType //スコアを得るための種類
    {
        sushi, //流れる寿司
        bubbleNormal, //通常の吹き出し
        bubbleOrder //注文レーンの寿司
    };

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        UpdateScore();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ScorePlus(ScoreType type)
    {
        if(type == ScoreType.sushi)
        {
            score += 300;
        }
        else if(type == ScoreType.bubbleNormal)
        {
            score += 100;
        }
        else if (type == ScoreType.bubbleOrder)
        {
            score += 400;
        }
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.GetComponent<TextMeshProUGUI>().text = "スコア：" + score.ToString();
    }
}
