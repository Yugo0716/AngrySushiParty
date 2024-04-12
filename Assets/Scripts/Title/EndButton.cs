using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndButton : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip clickSound;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void EndButtonClick()
    {
        audioSource.PlayOneShot(clickSound);
        Button button = GetComponent<Button>();
        button.interactable = false;
        StartCoroutine("Load");
    }
    IEnumerator Load()
    {
        yield return new WaitForSeconds(0.3f);
        Application.Quit();
    }
}
