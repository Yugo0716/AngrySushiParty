using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoTitleButton : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip clickSound;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void TitleButtonClick()
    {
        audioSource.PlayOneShot(clickSound);
        Button button = GetComponent<Button>();
        button.interactable = false;
        StartCoroutine("Load", "Title");
    }

    IEnumerator Load(string sceneName)
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(sceneName);
    }
}
