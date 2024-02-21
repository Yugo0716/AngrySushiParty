using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject scoreText;
    public int score = 0;

    public enum GameState //�Q�[���̏��(�J�n�O�A����~�I�ՁA�I����)
    {
        ready,
        early, 
        middle,
        last,
        end
    }

    public enum ScoreType //�X�R�A�𓾂邽�߂̎��
    {
        sushi, //�������i
        bubbleNormal, //�ʏ�̐����o��
        bubbleOrder //�������[���̎��i
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
