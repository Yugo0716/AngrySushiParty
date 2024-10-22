using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class E_OrderBubble : MonoBehaviour
{
    GameObject canvas;
    GameMode gameMode;

    float time = 0;
    float speed;
    float delay;
    float deleteTime;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("canvas");
        gameMode = canvas.GetComponent<GameMode>();

        if(!gameMode.isScored)
        {
            speed = 9.0f;
            deleteTime = 15f;
            delay = deleteTime / 6f;

            transform.DOScale(new Vector3(1.2f, 1.2f, 1), speed).SetEase(Ease.InSine).SetDelay(delay);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
