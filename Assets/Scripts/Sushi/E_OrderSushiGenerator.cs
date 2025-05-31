using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class E_OrderSushiGenerator : MonoBehaviour
{
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;
    [SerializeField] Transform pointC;

    public List<GameObject> sushiObjPoses = new List<GameObject>();
    public List<GameObject> bubbles = new List<GameObject>();

    TimeManager timeManager;

    [SerializeField] float orderInterbal;

    [SerializeField] private float time;

    [SerializeField] int orderCount = 0;
    [SerializeField] int orderChance = 0;

    public Image lampOn;

    public List<Sprite> sushiSprites = new List<Sprite>();

    public List<Sprite> orderBubblesSprites = new List<Sprite>();
    public List<Sprite> cOrderBubblesSprites = new List<Sprite>();


    Dictionary<int, SushiTypeSc.SushiType> numAndSushiType = new Dictionary<int, SushiTypeSc.SushiType>()
    {
        {0, SushiTypeSc.SushiType.Tamago}, {1, SushiTypeSc.SushiType.Ebi}, {2, SushiTypeSc.SushiType.Ika}, {3, SushiTypeSc.SushiType.Maguro}
        ,{4, SushiTypeSc.SushiType.Ikura}
    };

    Dictionary<int, string> numAndSushiName = new Dictionary<int, string>()
    {
        {0, "たまご"}, {1, "エビ"}, {2, "イカ"}, {3, "マグロ"}, {4, "イクラ"}
    };

    GameObject sushiText;
    GameObject giveText;

    List<string> giveTextList = new List<string> { "取って~", "ください", "くれ~", "ちょうだい", "ほしいな~", "くれる？", "お願い！"};

    //新規
    [SerializeField] GameObject orderSushiPos;
    int orderSushiNum = 2;

    [SerializeField] GameObject orderBubblePos;

    AudioSource audioSource;
    [SerializeField] AudioClip bubbleAppearSound;
    // Start is called before the first frame update
    void Start()
    {
        //gameState取得のため
        GameObject canvas = GameObject.FindGameObjectWithTag("canvas");
        timeManager = canvas.GetComponent<TimeManager>();

        audioSource = GameObject.FindGameObjectWithTag("AudioSource").GetComponent<AudioSource>();
        //注文が来ることを知らせる点滅
        lampOn = GameObject.FindGameObjectWithTag("Announce").GetComponent<Image>();

        orderInterbal = 17f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeManager.gameState == TimeManager.GameState.play)
        {
            time += Time.deltaTime;

            //注文寿司を動かす，吹き出しを表示させる 前の注文寿司が残ってるなら無し
            if (time > orderInterbal)
            {
                if (orderCount == 0 || (orderCount > 0 && sushiObjPoses[orderCount - 1] == null))
                {
                    lampOn.DOFade(1f, 0.5f).SetEase(Ease.InSine).SetLoops(6, LoopType.Yoyo);

                    StartCoroutine("StartOrder", orderCount);
                    lampOn.DOFade(0f, 0.5f).SetEase(Ease.InSine).SetDelay(3.0f);
                    SoundManager.soundManager.SEPlay(SEType.Alart);

                    orderCount++;
                }
                time = 0;
                orderChance++;

                if(orderChance > 3)
                {
                    if(orderChance % 3 == 1 && orderInterbal > 8)
                    {
                        orderInterbal -= 1.5f;
                        Debug.Log("orderPace Up!!!!");
                    }
                }
            }
        }
        
    }

    IEnumerator StartOrder(int orderCount)
    {
        
        GameObject orderSushiPosObj = Instantiate(orderSushiPos, this.transform.position + new Vector3(-14.25f, 1.03f,0f), transform.rotation, this.transform);        
        sushiObjPoses.Add(orderSushiPosObj);        

        GameObject orderBubbleObj = Instantiate(orderBubblePos, transform.position, transform.rotation, this.transform);
        bubbles.Add(orderBubbleObj);

        SetSushiImage(sushiObjPoses[orderCount], orderCount);

        yield return new WaitForSeconds(1.8f);

        sushiObjPoses[orderCount].transform.DOMove(new Vector3(20f, 0f, 0f), 3f).SetRelative().SetEase(Ease.OutSine);


        yield return new WaitForSeconds(3f);
        bubbles[orderCount].SetActive(true);

        //吹き出しはdelTimeおきにポポポンって感じにでるようにする
        List<int> bubbleCount = new List<int>();
        for (var i = 0; i < bubbles[orderCount].transform.childCount; i++)
        {
            bubbleCount.Add(i);
        }

        Shuffle(bubbleCount);

        int layer = 200;
        foreach (int i in bubbleCount)
        {
            GameObject childObj = bubbles[orderCount].transform.GetChild(i).gameObject;
            childObj.transform.position = SetBubblePosition(i);
            
            float delTime = 0.1f;
            yield return new WaitForSeconds(delTime);
            childObj.SetActive(true);
            childObj.GetComponent<Renderer>().sortingOrder = layer;
            childObj.transform.GetChild(0).gameObject.GetComponent<Canvas>().sortingOrder = layer + 1;
            layer += 10;
            SoundManager.soundManager.SEPlay(SEType.BubbleAppear);

        }
    }

    Vector2 SetBubblePosition(int num)
    {
        float[] x = new float[2];
        float[] y = new float[2];
        Vector2[] ranges = new Vector2[4];

        x[0] = UnityEngine.Random.Range(pointA.position.x, pointC.position.x - 0.8f);
        x[1] = UnityEngine.Random.Range(pointC.position.x + 0.8f, pointB.position.x);
        y[0] = UnityEngine.Random.Range(pointC.position.y + 0.8f, pointA.position.y);
        y[1] = UnityEngine.Random.Range(pointB.position.y, pointC.position.y - 0.8f);

        ranges[0] = new Vector2(x[0], y[0]);
        ranges[1] = new Vector2(x[0], y[1]);
        ranges[2] = new Vector2(x[1], y[0]);
        ranges[3] = new Vector2(x[1], y[1]);

        Vector2 returnPos;
        if (num == 0)
        {
            returnPos = ranges[UnityEngine.Random.Range(0, 2)];
        }
        else if(num == 1)
        {
            returnPos = ranges[UnityEngine.Random.Range(2, 4)];
        }
        else
        {
            returnPos = ranges[UnityEngine.Random.Range(0, 4)];
        }
        return returnPos;
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
        List<int> nums = GetRandom(numAndSushiName.Count, childObj.Length, 0);

        //上でえた数値に対応するスプライトにする　さらにsushitypeを指定
        for (int i = 0; i < childObj.Length; ++i)
        {
            childObj[i].GetComponent<SpriteRenderer>().sprite = sushiSprites[nums[i]];

            SushiTypeSc sushiTypeSc = childObj[i].GetComponent<SushiTypeSc>();
            sushiTypeSc.type = numAndSushiType[nums[i]];
        }
        
        SetBubbleType(bubbles[orderCount], nums);
    }

    //吹き出しの設定
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
            SushiTypeSc sushiTypeSc = childObj[i].GetComponent<SushiTypeSc>();
            sushiTypeSc.type = numAndSushiType[nums[i]];

            GameObject canvas = childObj[i].transform.GetChild(0).gameObject;

            canvas.GetComponent<Canvas>().sortingOrder = childObj[i].GetComponent<SpriteRenderer>().sortingOrder;

            sushiText = canvas.transform.GetChild(0).gameObject;
            giveText = canvas.transform.GetChild(1).gameObject;

            sushiText.GetComponent<TextMeshProUGUI>().text = numAndSushiName[nums[i]];
            giveText.GetComponent<TextMeshProUGUI>().text = giveTextList[UnityEngine.Random.Range(0, giveTextList.Count)];

            childObj[i].GetComponent<SpriteRenderer>().sprite = orderBubblesSprites[nums[i]];
        }
    }

    //要素数がsizeコのリストからランダムにnコの要素番号を入れたリストを返す(startの値を足したものを返すこともできる)
    public List<int> GetRandom(int size, int n, int start)
    {
        if (n > size)
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
