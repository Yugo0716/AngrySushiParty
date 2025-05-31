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

        if (gameObject != null)
            transform.DOScale(new Vector3(1.3f, 1.3f, 1), 3.0f).SetEase(Ease.InSine).SetDelay(15f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
