using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    public static bool isTouch = false;
    [SerializeField] GameObject touchTextObj;

    // Start is called before the first frame update
    void Start()
    {
        if(isTouch)
        {
            touchTextObj.SetActive(true);
        }
        else
        {
            touchTextObj.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T) || Input.touchCount == 2)
        {
            if (isTouch)
            {
                isTouch = false;
                touchTextObj.SetActive(false);
                SoundManager.soundManager.SEPlay(SEType.LifeMinus);
            }
            else
            {
                isTouch = true;
                touchTextObj.SetActive(true);
                SoundManager.soundManager.SEPlay(SEType.SushiClick);
            }
        }
    }
}
