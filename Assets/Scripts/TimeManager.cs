using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public GameObject timeText;
    public ScoreManager scoreManager;
    public UIManager uiManager;

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
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameState)
        {
            case GameState.ready:
                ReadyUpdate();
                break;

            case GameState.play:
                PlayUpdate();
                break;

            case GameState.end:
                EndUpdate();
                break;
        }

        timeText.GetComponent<TextMeshProUGUI>().text = "time: " + (Mathf.Ceil(displayTime)).ToString();
    }

    void ReadyUpdate()
    {
        StartCoroutine(StartProcess());
    }

    void PlayUpdate()
    {
        time += Time.deltaTime;
        displayTime = maxTime - time;

        if (displayTime <= 0)
        {
            gameState = GameState.end;
        }
    }

    void EndUpdate()
    {
        StartCoroutine(FinishProcess());
    }

    IEnumerator StartProcess()
    {
        uiManager.DisplayStart();

        yield return new WaitForSeconds(2.0f);

        uiManager.HideStart();
        gameState = GameState.play;
    }

    IEnumerator FinishProcess()
    {
        uiManager.DisplayFinish();

        yield return new WaitForSeconds(2.0f);

        uiManager.HideFinish();

        // イベントに登録
        SceneManager.sceneLoaded += GameSceneLoaded;

        // シーン切り替え
        SceneManager.LoadScene("Result");
    }

    public void GameSceneLoaded(Scene Result, LoadSceneMode mode)
    {
        ResultManager resultManager = GameObject.FindGameObjectWithTag("canvas").GetComponent<ResultManager>();
        resultManager.score = scoreManager.score;

        SceneManager.sceneLoaded -= GameSceneLoaded;
    }
}
