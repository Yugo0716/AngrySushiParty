using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip clickSound;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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

    IEnumerator Load(string sceneName)
    {
        Button button = GetComponent<Button>();
        button.interactable = false;
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(sceneName);
    }
}
