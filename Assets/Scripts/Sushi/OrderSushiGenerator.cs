using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderSushiGenerator : MonoBehaviour
{
    public GameObject[] sushiObjPoses;
    public GameObject[] bubbles;

    public GameObject timeManagerObj;
    TimeManager timeManager;

    [SerializeField] float[] orderIntervals = new float[] { 15, 15, 15 }; //注文寿司がくる間隔(あとで変更する)

    [SerializeField] private float time;

    int orderCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        //gameState取得のため
        GameObject canvas = GameObject.FindGameObjectWithTag("canvas");
        timeManager = canvas.GetComponent<TimeManager>();

        //注文の寿司と吹き出しの組が合っているかチェック
        CheckPairs();

        foreach (GameObject obj in bubbles)
        {
            obj.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(timeManager.gameState == TimeManager.GameState.play)
        {
            time += Time.deltaTime;

            //注文寿司を動かす，吹き出しを表示させる
            if(orderCount < 3)
            {
                if(time > orderIntervals[orderCount])
                {
                    StartCoroutine("StartOrder", orderCount);

                    time = 0;
                    orderCount++;
                }   
            }
        }
    }

    void CheckPairs()
    {
        if(sushiObjPoses.Length != bubbles.Length)
        {
            Debug.LogError("注文寿司と吹き出しの組が合いません");
            return;
        }
        else
        {
            for (int i = 0; i < sushiObjPoses.Length; i++)
            {
                if (sushiObjPoses[i].transform.childCount != bubbles[i].transform.childCount)
                {
                    Debug.LogError("注文1セットの寿司と吹き出しの個数が合いません");
                    return;
                }
            }
        }
    }

    IEnumerator StartOrder(int orderCount)
    {
        sushiObjPoses[orderCount].transform.DOMove(new Vector3(20f, 0f, 0f), 3f).SetRelative().SetEase(Ease.OutSine);

        yield return new WaitForSeconds(3f);

        bubbles[orderCount].SetActive(true);
    }
}
