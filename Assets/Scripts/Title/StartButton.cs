using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip clickSound;

    FadeManager fadeManager;
    GameObject fadeCanvas;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        fadeCanvas = GameObject.FindGameObjectWithTag("FadeCanvas");
        fadeManager = fadeCanvas.GetComponent<FadeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void StartButtonClick()
    {
        if (Input.touchCount == 0)
        {
            audioSource.PlayOneShot(clickSound);

            StartCoroutine("Load", "SelectScene");
            //FadeLoadScene("SelectScene");
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
