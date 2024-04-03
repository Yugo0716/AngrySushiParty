using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject startText;
    public GameObject finishText;


    // Start is called before the first frame update
    void Start()
    {
        startText.SetActive(false);
        finishText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayStart()
    {
        startText.SetActive(true);
    }

    public void HideStart()
    {
        startText.SetActive(false);
    }

    public void DisplayFinish()
    {
        finishText.SetActive(true);
    }

    public void HideFinish()
    {
        finishText.SetActive(false);
    }
}
