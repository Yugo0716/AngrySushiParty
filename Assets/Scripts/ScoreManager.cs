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

    

    public enum ScoreType //�X�R�A�𓾂邽�߂̎��
    {
        sushi, //�������i
        bubbleNormal, //�ʏ�̐����o��
        bubbleOrder //�������[���̎��i
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
        scoreText.text = "�X�R�A�F" + score.ToString();
    }
}
