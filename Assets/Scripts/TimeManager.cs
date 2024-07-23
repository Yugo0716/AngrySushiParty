using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [SerializeField] GameObject timeTextObj;
    TextMeshProUGUI timeText;
    ScoreManager scoreManager;
    UIManager uiManager;
    GameMode gameMode;
    LifeManager lifeManager = null;
    

    public bool countDown = false;
    public int maxTime = 60;
    public float displayTime = 0;
    public float time = 0;

    public enum GameState //�Q�[���̏��(�J�n�O�A����~�I�ՁA�I����)
    {
        ready,
        play,
        end
    }

    public GameState gameState;

    // Start is called before the first frame update
    void Start()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("canvas");
        scoreManager = canvas.GetComponent<ScoreManager>();
        uiManager = canvas.GetComponent<UIManager>();
        gameMode = canvas.GetComponent<GameMode>();

        timeText = timeTextObj.GetComponent<TextMeshProUGUI>();

        //�G���h���X���[�h�̎��̂ݎc�@���l������
        if (!gameMode.isScored)
        {
            lifeManager = canvas.GetComponent<LifeManager>();
        }

        gameState = GameState.ready;

        //�ŏ��̕\���^�C��
        if (gameMode.isScored)
        {
            displayTime = maxTime;
        }
        else
        {
            displayTime = 0;
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
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

        timeText.text = "�^�C���F" + (Mathf.Ceil(displayTime)).ToString();
    }

    void ReadyUpdate()
    {
        StartCoroutine(StartProcess());
    }

    void PlayUpdate()
    {
        time += Time.deltaTime;

        if(gameMode.isScored)
        {
            displayTime = maxTime - time;

            if (displayTime <= 0)
            {
                gameState = GameState.end;
            }
        }
        else
        {
            displayTime = time;
            //�c�@�������Ȃ�����gameState��end�ɂ���
            if(lifeManager.life <= 0)
            {
                gameState = GameState.end;
            }
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

        // �V�[���؂�ւ�
        SceneManager.LoadScene("Result");
    }
}
