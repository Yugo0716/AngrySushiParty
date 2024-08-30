using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Bubble_test : MonoBehaviour
{
    private float time;

    public bool order = false;
    public float bonusScore = 0;

    //public List<Sprite> bubbleSprite = new List<Sprite>();
    //public List<Sprite> cBubbleSprite = new List<Sprite>();

    GameObject canvas;
    GameMode gameMode;
    bool isScored;

    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        bonusScore = 0;
        canvas = GameObject.FindGameObjectWithTag("canvas");

        //GameModeŽæ“¾‚Ì‚½‚ß
        gameMode = canvas.GetComponent<GameMode>();
        isScored = gameMode.isScored;

        if (gameObject!=null)
        transform.DOScale(new Vector3(1.2f, 1.2f, 1), 9.0f).SetEase(Ease.InSine).SetDelay(2.7f);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time <= 1f)
        {
            bonusScore = 100f;
        }
        else if (time <= 11f)
        {
            bonusScore = Mathf.Ceil( 100 - (time-1) * 10);
        }
        else
        {
            bonusScore = 0;
        }
        

        if (time > 16.0f)
        {
            if (!isScored)
            {
                canvas.GetComponent<LifeManager>().LifeMinus();
            }
            Destroy(gameObject);
        }
    }
}
