using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderSushiGenerator : MonoBehaviour
{
    public GameObject[] sushiObjPoses;
    public GameObject[] bubbles;

    public GameObject timeManagerObj;
    TimeManager timeManager;

    [SerializeField] float[] orderIntervals = new float[] { 13, 15, 15 }; //注文寿司がくる間隔(あとで変更する)

    [SerializeField] private float time;

    [SerializeField]int orderCount = 0;
    [SerializeField]int orderChance = 0;

    public Image orderAnnounce;


    // Start is called before the first frame update
    void Start()
    {
        //gameState取得のため
        GameObject canvas = GameObject.FindGameObjectWithTag("canvas");
        timeManager = canvas.GetComponent<TimeManager>();

        //注文が来ることを知らせる点滅
        orderAnnounce = GameObject.FindGameObjectWithTag("Announce").GetComponent<Image>();
        

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

            //注文寿司を動かす，吹き出しを表示させる 前の注文寿司が残ってるなら無し
            if(orderChance < 3)
            {
                if(time > orderIntervals[orderCount])
                {
                    if (orderCount == 0 || (orderCount > 0 && sushiObjPoses[orderCount-1] == null))
                    {
                        orderAnnounce.DOFade(1f, 0.5f).SetEase(Ease.InCubic).SetLoops(6, LoopType.Yoyo);

                        StartCoroutine("StartOrder", orderCount);
                        orderAnnounce.DOFade(0f, 0.5f).SetEase(Ease.InCubic).SetDelay(3.0f);
                        
                        orderCount++;
                    }                    
                    time = 0;
                    orderChance++;
                }
            }
        }
        else if(timeManager.gameState == TimeManager.GameState.end)
        {
            foreach (GameObject obj in sushiObjPoses)
            {
                if(obj != null && obj.activeSelf) obj.SetActive(false);
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
        yield return new WaitForSeconds(1.8f);
        sushiObjPoses[orderCount].transform.DOMove(new Vector3(20f, 0f, 0f), 3f).SetRelative().SetEase(Ease.OutSine);

        yield return new WaitForSeconds(3f);

        bubbles[orderCount].SetActive(true);
    }
}
