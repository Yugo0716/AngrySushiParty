using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class StartButton : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip clickSound;

    FadeManager fadeManager;
    GameObject fadeCanvas;

    [SerializeField] GameObject backSushiGeneratorObjA;
    [SerializeField] GameObject backSushiGeneratorObjB;
    BackSushiGenerator backSushiGeneratorA;
    BackSushiGenerator backSushiGeneratorB;

    [SerializeField] GameObject clickBlurObj;
    ClickBlur clickBlur;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

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
        if(clickBlurObj != null)
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

    //セレクト画面とリザルトでのリトライで使用
    public void SelectNormalGame()
    {
        if (Input.touchCount == 0)
        {
            audioSource.PlayOneShot(clickSound);
            backSushiGeneratorA.isGo = false;
            backSushiGeneratorB.isGo = false;
            StartCoroutine(SelectGame("GameScene"));
        }
    }

    //セレクト画面とリザルトでのリトライで使用
    public void SelectEndlessGame()
    {
        if (Input.touchCount == 0)
        {
            audioSource.PlayOneShot(clickSound);
            backSushiGeneratorA.isGo = false;
            backSushiGeneratorB.isGo = false;
            StartCoroutine(SelectGame("EndlessGameScene"));
        }
    }

    //タイトルからとゲーム中からで使用
    public void StartButtonClick()
    {
        if (Input.touchCount == 0)
        {
            audioSource.PlayOneShot(clickSound);


            //SceneManager.LoadScene("SelectScene");
            StartCoroutine(FadeLoadwithBack("SelectScene"));
            //UnduplicateLoad("BackScene");


            //StartCoroutine("Load", "SelectScene");
        }
    }

    #region 使わん
    public void PlayButtonClick()
    {

        if (Input.touchCount == 0)
        {
            audioSource.PlayOneShot(clickSound);
            
            StartCoroutine(FadeLoad("GameScene"));
        }
        
    }

    public void E_PlayButtonClick()
    {

        if (Input.touchCount == 0)
        {
            audioSource.PlayOneShot(clickSound);

            StartCoroutine(FadeLoad("EndlessGameScene"));
        }

    }
    #endregion

    //セレクト画面からで使用
    public void TitleButtonClick()
    {
        audioSource.PlayOneShot(clickSound);
        
        StartCoroutine(FadeLoad("Title"));
    }

    //ゲーム画面に移行するとき
    IEnumerator SelectGame(string sceneName)
    {
        yield return new WaitForSeconds(0.3f);
        clickBlur.Blur();
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }


    IEnumerator FadeLoad(string sceneName)
    {
        fadeManager.FadeIn();
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator FadeLoadwithBack(string sceneName)
    {
        fadeManager.FadeIn();
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(sceneName);
        UnduplicateLoad("BackScene");
    }


    private void UnduplicateLoad(string loadSceneName)
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
