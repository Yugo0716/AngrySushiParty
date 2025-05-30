using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange_Result : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip clickSound;

    FadeManager fadeManager;
    GameObject fadeCanvas;
    GameObject canvas;

    [SerializeField] GameObject receipt;

    GameObject backSushiGeneratorObjA;
    GameObject backSushiGeneratorObjB;
    BackSushiGenerator backSushiGeneratorA;
    BackSushiGenerator backSushiGeneratorB;

    GameObject clickBlurObj;
    ClickBlur clickBlur;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("ResultCanvas");
        audioSource = GetComponent<AudioSource>();
        if (canvas != null)
        {
            animator = canvas.GetComponent<Animator>();
        }


        backSushiGeneratorObjA = GameObject.FindGameObjectWithTag("BackSushiGeneratorA");
        backSushiGeneratorObjB = GameObject.FindGameObjectWithTag("BackSushiGeneratorB");
        clickBlurObj = GameObject.FindGameObjectWithTag("ClickBlur");
        if (backSushiGeneratorObjA != null)
        {
            backSushiGeneratorA = backSushiGeneratorObjA.GetComponent<BackSushiGenerator>();
        }
        if (backSushiGeneratorObjB != null)
        {
            backSushiGeneratorB = backSushiGeneratorObjB.GetComponent<BackSushiGenerator>();
        }
        if (clickBlurObj != null)
        {
            clickBlur = clickBlurObj.GetComponent<ClickBlur>();
        }

        fadeCanvas = GameObject.FindGameObjectWithTag("FadeCanvas");
        fadeManager = fadeCanvas.GetComponent<FadeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RetryNormalGame()
    {
        SoundManager.soundManager.SEPlay(SEType.ButtonClick);
        if (backSushiGeneratorA != null)
        {
            backSushiGeneratorA.isGo = false;
        }
        if (backSushiGeneratorA != null)
        {
            backSushiGeneratorB.isGo = false;
        }


        animator.Play("RetryAnimation");
        receipt.transform.DOLocalMove(new Vector3(-160f, 707f, 0f), 0.45f).SetEase(Ease.Linear);

        StartCoroutine(SelectGame("GameScene"));
    }

    public void RetryEndlessGame()
    {
        SoundManager.soundManager.SEPlay(SEType.ButtonClick);
        if (backSushiGeneratorA != null)
        {
            backSushiGeneratorA.isGo = false;
        }
        if (backSushiGeneratorA != null)
        {
            backSushiGeneratorB.isGo = false;
        }

        //animator.Play("E_RetryAnimation");
        animator.Play("RetryAnimation");
        receipt.transform.DOLocalMove(new Vector3(-160f, 707f, 0f), 0.45f).SetEase(Ease.Linear);

        StartCoroutine(SelectGame("EndlessGameScene"));
    }

    public void SelectButtonClick()
    {
        SoundManager.soundManager.SEPlay(SEType.ButtonClick);

        animator.Play("SelectAnimation");
        receipt.transform.DOLocalMove(new Vector3(-160f, 707f, 0f), 0.45f).SetEase(Ease.Linear);
        StartCoroutine(LoadwithBack("SelectScene"));
    }

    //ゲーム画面に移行するとき
    IEnumerator SelectGame(string sceneName)
    {
        yield return new WaitForSeconds(0.3f);
        if(clickBlur != null)
        {
            clickBlur.Blur();
        }
        
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    public void TitleButtonClick()
    {
        SoundManager.soundManager.SEPlay(SEType.ButtonClick);

        StartCoroutine(FadeLoad("Title"));
    }

    IEnumerator LoadwithBack(string sceneName)
    {
        //fadeManager.FadeIn();
        yield return new WaitForSeconds(1f);
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.UnloadSceneAsync(currentScene.name);
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        UnduplicateLoad("BackScene");
    }

    IEnumerator FadeLoad(string sceneName)
    {
        fadeManager.FadeIn();
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(sceneName);
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
        SceneManager.LoadScene(loadSceneName, LoadSceneMode.Additive);
    }
}
