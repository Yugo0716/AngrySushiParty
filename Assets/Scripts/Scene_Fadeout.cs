using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scene_Fadeout : MonoBehaviour
{
    FadeManager fadeManager;
    GameObject fadeCanvas;
    GameObject fadeObj;

    float alpha;
    // Start is called before the first frame update
    void Start()
    {
        fadeCanvas = GameObject.FindGameObjectWithTag("FadeCanvas");
        fadeManager = fadeCanvas.GetComponent<FadeManager>();
        fadeObj = fadeCanvas.transform.GetChild(0).gameObject;
        
        alpha = fadeObj.GetComponent<Image>().color.a;

        if(alpha > 0)
        {
            fadeManager.FadeOut();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
