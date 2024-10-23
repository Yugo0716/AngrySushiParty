using DG.Tweening;
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
    GameObject canvas;

    [SerializeField] GameObject backSushiGeneratorObjA;
    [SerializeField] GameObject backSushiGeneratorObjB;
    BackSushiGenerator backSushiGeneratorA;
    BackSushiGenerator backSushiGeneratorB;

    [SerializeField] GameObject clickBlurObj;
    ClickBlur clickBlur;

    [SerializeField] GameObject normalReceipt;
    [SerializeField] GameObject endlessReceipt;
    [SerializeField] GameObject howtoPanel;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("SelectCanvas");
        audioSource = GetComponent<AudioSource>();
        if(canvas != null)
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
        /*�^�b�`�����F��������^�C�g���ɔ�΂�
        if (Input.touchCount > 0)
        {
            SoundManager.soundManager.SEPlay(SEType.LifeMinus);
            StartCoroutine(FadeLoad("Title"));
        }*/
    }

    //�Z���N�g��ʂŎg�p
    public void SelectNormalGame()
    {
        if (Input.touchCount == 0)
        {
            SoundManager.soundManager.SEPlay(SEType.ButtonClick);
            backSushiGeneratorA.isGo = false;
            backSushiGeneratorB.isGo = false;

            animator.Play("NormalSelectAnimation");
            normalReceipt.transform.DOLocalMove(new Vector3(176f, -719f, 0f), 0.45f).SetEase(Ease.Linear);
            howtoPanel.transform.DOLocalMove(new Vector3(0f, 500f, 0f), 0.1f).SetEase(Ease.Linear);
            StartCoroutine(SelectGame("GameScene"));
        }
    }

    public void SelectEndlessGame()
    {
        if (Input.touchCount == 0)
        {
            SoundManager.soundManager.SEPlay(SEType.ButtonClick);
            backSushiGeneratorA.isGo = false;
            backSushiGeneratorB.isGo = false;

            animator.Play("EndlessSelectAnimation");
            endlessReceipt.transform.DOLocalMove(new Vector3(-176f, -719f, 0f), 0.45f).SetEase(Ease.Linear);
            howtoPanel.transform.DOLocalMove(new Vector3(0f, 500f, 0f), 0.1f).SetEase(Ease.Linear);
            StartCoroutine(SelectGame("EndlessGameScene"));
        }
    }

    public void SelectTitle()
    {
        if (Input.touchCount == 0)
        {
            SoundManager.soundManager.SEPlay(SEType.ButtonClick);

            StartCoroutine(FadeLoad("Title"));
        }
    }

    //�^�C�g������ƃQ�[��������Ŏg�p
    public void StartButtonClick()
    {
        if (Input.touchCount == 0)
        {
            SoundManager.soundManager.SEPlay(SEType.ButtonClick);

            StartCoroutine(FadeLoadwithBack("SelectScene"));
        }
    }

    

    //�Q�[����ʂɈڍs����Ƃ�
    IEnumerator SelectGame(string sceneName)
    {
        yield return new WaitForSeconds(0.5f);
        clickBlur.Blur();
        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }


    IEnumerator FadeLoadwithBack(string sceneName)
    {
        fadeManager.FadeIn();
        yield return new WaitForSeconds(0.3f);
        // ���݃A�N�e�B�u�ȃV�[���̖��O���擾
        Scene currentScene = SceneManager.GetActiveScene();
        // ���݂̃V�[����񓯊��ŃA�����[�h
        SceneManager.UnloadSceneAsync(currentScene.name);
        SceneManager.LoadSceneAsync(sceneName);
        SceneManager.LoadScene("BackScene", LoadSceneMode.Additive);
        //UnduplicateLoad("BackScene");
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
