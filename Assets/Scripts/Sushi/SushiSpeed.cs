using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SushiSpeed : MonoBehaviour
{
    TimeManager timeManager;
    GameMode gameMode;

    //エンドレスモードでの寿司のスピード(NormalSushiに渡す)
    float rate_speed = 1;//エンドレスでspeedを変更するときの倍率
    float delTime_speed = 0f;
    float interval_speed = 30f;
    public float speed = 0;


    // Start is called before the first frame update
    void Start()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("canvas");
        timeManager = canvas.GetComponent<TimeManager>();
        gameMode = canvas.GetComponent<GameMode>();

        speed = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        if(timeManager.gameState==TimeManager.GameState.play)
        {
            if(!gameMode.isScored)
            {
                delTime_speed += Time.deltaTime;

                if (delTime_speed > interval_speed && speed < 6.0f)
                {
                    speed += 0.5f;

                    delTime_speed = 0;
                    rate_speed++;

                    Debug.Log("Speed Up!!");

                    if(rate_speed >= 3 && rate_speed <=5)
                    {
                        interval_speed = 40f;
                    }
                    else
                    {
                        interval_speed = 60f;
                    }
                }
            }
        }
    }
}
