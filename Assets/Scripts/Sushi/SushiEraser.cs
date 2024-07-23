using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SushiEraser : MonoBehaviour
{
    GameObject canvas;
    //スコアを取るモードか取らないか
    GameMode gameMode;
    private bool isScored;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("canvas");

        //GameMode取得のため
        gameMode = canvas.GetComponent<GameMode>();
        isScored = gameMode.isScored;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Sushi")
        {
            SushiController sushiController = collision.gameObject.GetComponent<SushiController>();

            //寿司をドラッグ中じゃない時
            if (!sushiController.sushiRay)
            {
                Destroy(collision.gameObject.transform.root.gameObject);

                //エンドレスモードなら残機-1
                if (!isScored)
                {
                    canvas.GetComponent<LifeManager>().LifeMinus();
                }
            }            
        }
    }
}
