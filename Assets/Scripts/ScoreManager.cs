using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public GameObject scoreText;
    public int score = 0;

    

    public enum ScoreType //�X�R�A�𓾂邽�߂̎��
    {
        sushi, //�������i
        bubbleNormal, //�ʏ�̐����o��
        bubbleOrder //�������[���̎��i
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
        scoreText.GetComponent<TextMeshProUGUI>().text = "�X�R�A�F" + score.ToString();
    }
}
