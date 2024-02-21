using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject scoreText;
    public int score = 0;

    public enum GameState //ゲームの状態(開始前、序盤~終盤、終了後)
    {
        ready,
        early, 
        middle,
        last,
        end
    }

    public enum ScoreType //スコアを得るための種類
    {
        sushi, //流れる寿司
        bubbleNormal, //通常の吹き出し
        bubbleOrder //注文レーンの寿司
    };

    // Start is called before the first frame update
    void Start()
    {
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
            score += 100;
        }
        else if(type == ScoreType.bubbleNormal)
        {
            score += 300;
        }
        else if (type == ScoreType.bubbleOrder)
        {
            score += 400;
        }
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.GetComponent<TextMeshProUGUI>().text = "score: " + score.ToString();
    }
}
