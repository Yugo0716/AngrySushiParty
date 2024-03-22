using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public GameObject timeText;

    public bool countDown = false;
    public int maxTime = 60;
    public float displayTime = 0;
    public float time = 0;

    public enum GameState //ゲームの状態(開始前、序盤~終盤、終了後)
    {
        ready,
        play,
        end
    }

    public GameState gameState;

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameState.ready;
        displayTime = maxTime;

        StartCountDown();
    }

    // Update is called once per frame
    void Update()
    {
        if (countDown)
        {
            time += Time.deltaTime;
            displayTime = maxTime - time;

            

            if(displayTime <= 0)
            {
                gameState = GameState.end;
                countDown = false;
            }
        }

        timeText.GetComponent<TextMeshProUGUI>().text = "time: " + (Mathf.Ceil(displayTime)).ToString();
    }

    public void StartCountDown()
    {
        countDown = true;
        gameState = GameState.play;
    }
}
