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
        if(backSushiGeneratorObjA != null)
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

    public void SelectNormalGame()
    {
        if (Input.touchCount == 0)
        {
            audioSource.PlayOneShot(clickSound);
            backSushiGeneratorA.isGo = false;
            backSushiGeneratorB.isGo = false;
            StartCoroutine("SelectGame", "GameScene");
        }
    }

    public void SelectEndlessGame()
    {
        if (Input.touchCount == 0)
        {
            audioSource.PlayOneShot(clickSound);
            backSushiGeneratorA.isGo = false;
            backSushiGeneratorB.isGo = false;
            StartCoroutine("SelectGame", "EndlessGameScene");
        }
    }


    public void StartButtonClick()
    {
        if (Input.touchCount == 0)
        {
            audioSource.PlayOneShot(clickSound);

            StartCoroutine("Load", "SelectScene");
        }
    }
    public void PlayButtonClick()
    {

        if (Input.touchCount == 0)
        {
            audioSource.PlayOneShot(clickSound);
            
            StartCoroutine("Load", "GameScene");
        }
        
    }

    public void E_PlayButtonClick()
    {

        if (Input.touchCount == 0)
        {
            audioSource.PlayOneShot(clickSound);

            StartCoroutine("Load", "EndlessGameScene");
        }

    }

    public void TitleButtonClick()
    {
        audioSource.PlayOneShot(clickSound);
        
        StartCoroutine("Load", "Title");
        //FadeLoadScene("Title");
    }

    IEnumerator SelectGame(string sceneName)
    {
        
        //fadeManager.FadeIn();
        yield return new WaitForSeconds(0.3f);
        clickBlur.Blur();
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        yield return new WaitForSeconds(3.0f);
        SceneManager.UnloadSceneAsync("SelectScene");
    }

    IEnumerator Load(string sceneName)
    {
        fadeManager.FadeIn();
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(sceneName);
    }

    public async void FadeLoadScene(string sceneName)
    {
        fadeManager.FadeIn();
        await Task.Delay(300);
        SceneManager.LoadScene(sceneName);
    }
}
