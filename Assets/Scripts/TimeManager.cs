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

    GameObject sushiCounterObj;
    SushiCounter sushiCounter;

    FadeManager fadeManager;
    GameObject fadeCanvas;

    public bool countDown = false;
    public int maxTime = 60;
    public float displayTime = 0;
    public static float time = 0;

    bool isStart = true;
    bool isFinish = true;

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

        fadeCanvas = GameObject.FindGameObjectWithTag("FadeCanvas");
        fadeManager = fadeCanvas.GetComponent<FadeManager>();

        sushiCounterObj = GameObject.FindGameObjectWithTag("SushiCounter");
        if(sushiCounterObj != null)
        {
            sushiCounter = sushiCounterObj.GetComponent<SushiCounter>();
        }

        timeText = timeTextObj.GetComponent<TextMeshProUGUI>();
        time = 0;
        //�G���h���X���[�h�̎��̂ݎc�@���l������
        if (!gameMode.isScored)
        {
            lifeManager = canvas.GetComponent<LifeManager>();
        }
        
        SoundManager.soundManager.StopBGM();
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
        if(sushiCounter.counter <= 0 && isStart)
        {
            StartCoroutine(StartProcess());
            isStart = false;
        }
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
        if (isFinish)
        {
            StartCoroutine(FinishProcess());
            isFinish = false;
        }
        
    }

    IEnumerator StartProcess()
    {
        yield return new WaitForSeconds(0.2f);
        uiManager.DisplayStart();
        yield return new WaitForSeconds(2.2f);

        uiManager.HideStart();
        gameState = GameState.play;
        SoundManager.soundManager.PlayBGM(BGMType.Play);

        ExistUnload("SelectScene");
        ExistUnload("BackScene");
        ExistUnload("Result");
        ExistUnload("EndlessResult");
        Resources.UnloadUnusedAssets();
    }

    IEnumerator FinishProcess()
    {
        uiManager.DisplayFinish();

        yield return new WaitForSeconds(2.0f);

        uiManager.HideFinish();

        SoundManager.soundManager.StopBGM();

        // �V�[���؂�ւ�
        if (gameMode.isScored)
        {
            StartCoroutine(FadeLoadwithBack("Result"));
        }
        else
        {
            StartCoroutine(FadeLoadwithBack("EndlessResult"));
        }
    }

    IEnumerator FadeLoadwithBack(string sceneName)
    {
        fadeManager.FadeIn();
        yield return new WaitForSeconds(0.3f);

        SceneManager.LoadScene(sceneName);
        UnduplicateLoad("BackScene");
    }

    void UnduplicateLoad(string loadSceneName)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);

            if (scene.name == loadSceneName)
            {
                return;
            }
        }

        SceneManager.LoadSceneAsync(loadSceneName, LoadSceneMode.Additive);
    }

    void ExistUnload(string unloadSceneName)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);

            if (scene.name == unloadSceneName)
            {
                SceneManager.UnloadSceneAsync(unloadSceneName);
            }
        }
    }
}
