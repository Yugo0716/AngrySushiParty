using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
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

    public Sprite[] sushiSprites;

    Dictionary<int, SushiTypeSc.SushiType> numAndSushiType = new Dictionary<int, SushiTypeSc.SushiType>()
    {
        {0, SushiTypeSc.SushiType.Tamago}, {1, SushiTypeSc.SushiType.Ebi}, {2, SushiTypeSc.SushiType.Ika}, {3, SushiTypeSc.SushiType.Maguro}
    };

    Dictionary<int, string> numAndSushiName = new Dictionary<int, string>()
    {
        {0, "たまご"}, {1, "エビ"}, {2, "イカ"}, {3, "マグロ"}
    };

    GameObject sushiText;

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
        SetSushiImage(sushiObjPoses[orderCount], orderCount);

        yield return new WaitForSeconds(1.8f);
        sushiObjPoses[orderCount].transform.DOMove(new Vector3(20f, 0f, 0f), 3f).SetRelative().SetEase(Ease.OutSine);

        yield return new WaitForSeconds(3f);

        bubbles[orderCount].SetActive(true);
    }

    //寿司のスプライトとSushiTypeの指定
    void SetSushiImage(GameObject sushiPos, int orderCount)
    {
        int sushiCount = sushiPos.transform.childCount;

        // 子オブジェクトを格納する配列作成
        GameObject[] childObj = new GameObject[sushiCount];

        //子オブジェクトを配列に格納
        for (var i = 0; i < childObj.Length; ++i)
        {
            childObj[i] = sushiPos.transform.GetChild(i).gameObject;
        }

        //0~3の中から注文寿司の数だけランダムに整数を得る
        List<int> nums = GetRandom(4, childObj.Length, 0);

        //上でえた数値に対応するスプライトにする　さらにsushitypeを指定
        for (int i = 0; i < childObj.Length; ++i)
        {
            childObj[i].GetComponent<SpriteRenderer>().sprite = sushiSprites[nums[i]];

            SushiTypeSc sushiTypeSc = childObj[i].GetComponent<SushiTypeSc>();
            sushiTypeSc.type = numAndSushiType[nums[i]];
        }

        SetBubbleType(bubbles[orderCount], nums);
    }

    void SetBubbleType(GameObject bubblePos, List<int> nums)
    {
        int bubbleCount = bubblePos.transform.childCount;

        // 子オブジェクトを格納する配列作成
        GameObject[] childObj = new GameObject[bubbleCount];

        //子オブジェクトを配列に格納
        for (var i = 0; i < childObj.Length; ++i)
        {
            childObj[i] = bubblePos.transform.GetChild(i).gameObject;
        }

        //numsをシャッフル(どの吹き出しにどの注文寿司を入れるかをランダムにするため)
        Shuffle(nums);

        //上でえた数値に対応するスプライトにする　さらにsushitypeを指定
        for (int i = 0; i < childObj.Length; ++i)
        {
            //childObj[i].GetComponent<SpriteRenderer>().sprite = sushiSprites[nums[i]];

            SushiTypeSc sushiTypeSc = childObj[i].GetComponent<SushiTypeSc>();
            sushiTypeSc.type = numAndSushiType[nums[i]];

            GameObject obj = childObj[i].transform.GetChild(0).gameObject;

            sushiText = obj.transform.GetChild(0).gameObject.gameObject;
            sushiText.GetComponent<TextMeshProUGUI>().text = numAndSushiName[nums[i]] + "\n取って~";
        }
    }

    //要素数がsizeコのリストからランダムにnコの要素番号を入れたリストを返す(startの値を足したものを返すこともできる)
    public List<int> GetRandom(int size, int n, int start)
    {
        if(n > size)
        {
            throw new ArgumentOutOfRangeException("リストの要素数よりnが大きいです。");
        }
        var indexList = new List<int>(size);
        var returnList = new List<int>(n);

        for (int p = 0; p < size; p++) indexList.Add(p);

        for (int i = 0; i < n; i++)
        {
            int index = UnityEngine.Random.Range(0, indexList.Count);
            int value = indexList[index];
            indexList.RemoveAt(index);
            returnList.Add(value + start);
        }
        return returnList;
    }

    void Shuffle(List<int> array)
    {
        for (var i = array.Count - 1; i > 0; --i)
        {
            // 0以上i以下のランダムな整数を取得
            // Random.Rangeの最大値は第２引数未満なので、+1することに注意
            var j = UnityEngine.Random.Range(0, i + 1);

            // i番目とj番目の要素を交換する
            var tmp = array[i];
            array[i] = array[j];
            array[j] = tmp;
        }
    }
}
