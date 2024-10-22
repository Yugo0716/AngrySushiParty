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

    public Dictionary<ScoreType, int> baseScore = new Dictionary<ScoreType, int>()
    {
        {ScoreType.sushi, 300}, {ScoreType.bubbleNormal, 100}, {ScoreType.bubbleOrder, 200}
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

    public void ScorePlus(ScoreType type, int bonus)
    {
        if (gameMode.isScored)
        {
            if (type == ScoreType.sushi)
            {
                score += baseScore[ScoreType.sushi]; //300
            }
            else if (type == ScoreType.bubbleNormal)
            {
                score += baseScore[ScoreType.bubbleNormal] + bonus; //100
            }
            else if (type == ScoreType.bubbleOrder)
            {
                score += baseScore[ScoreType.bubbleOrder] + bonus; //200
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
        scoreText.text = score.ToString();
    }
}
