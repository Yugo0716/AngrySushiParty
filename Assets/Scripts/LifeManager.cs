using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour
{
    public int life = 3;

    [SerializeField] GameObject lifeObj;
    Image lifeImage;
    [SerializeField] Sprite life3sprite;
    [SerializeField] Sprite life2sprite;
    [SerializeField] Sprite life1sprite;
    [SerializeField] Sprite life0sprite;

    // Start is called before the first frame update
    void Start()
    {
        lifeImage = lifeObj.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (life < 0) life = 0;
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            life = 100;
        }*/
    }

    public void LifeMinus()
    {
        if (life > 0)
        {
            life--;
            SoundManager.soundManager.SEPlay(SEType.LifeMinus);
        }
        if (life == 3)
        {
            lifeImage.sprite = life3sprite;
        }
        if (life == 2)
        {
            lifeImage.sprite = life2sprite;
        }
        else if(life == 1)
        {
            lifeImage.sprite = life1sprite;
        }
        else
        {
            lifeImage.sprite = life0sprite;
        }
    }
}
