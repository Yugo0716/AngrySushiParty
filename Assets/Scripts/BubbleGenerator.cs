using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;

public class BubbleGenerator : MonoBehaviour
{
    public GameObject bubbleObj;
    //[SerializeField] GameObject generateBubbleObj;

    public Transform pointA;
    public Transform pointB;
    public Transform pointC;

    int prenum = 0;

    private float time;
    [SerializeField] float interval = 4.0f; //吹き出しを出す間隔
    [SerializeField] private int counter = 8; //吹き出しを出すたびに増加(layerに使う)
    new Renderer renderer;

    TimeManager timeManager;

    Dictionary<int, ItemTypeSc.ItemType> numAndItemType = new Dictionary<int, ItemTypeSc.ItemType>()
    {
        {0, ItemTypeSc.ItemType.shoyu}, {1, ItemTypeSc.ItemType.gari}, {2, ItemTypeSc.ItemType.wasabi}, {3, ItemTypeSc.ItemType.yunomi}
    };

    Dictionary<int, string> numAndItemName = new Dictionary<int, string>()
    {
        {0, "醤油"}, {1, "ガリ"}, {2, "わさび"}, {3, "湯のみ"}
    };

    public List<Sprite> ItemSprite = new List<Sprite>();

    GameObject itemText;

    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        GameObject canvas = GameObject.FindGameObjectWithTag("canvas");
        timeManager = canvas.GetComponent<TimeManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(timeManager.gameState == TimeManager.GameState.play)
        {
            time += Time.deltaTime;

            if (time >= interval)
            {
                Generate("BubbleNormal");
                counter++;
                time = 0;
            }
        }
        
    }

    //吹き出しを作る
    void Generate(string tag)
    {
        int[] nums = new int[4] { 0, 0, 0, 0 };
        Vector2[] ranges = new Vector2[4];
        float[] x = new float [2];
        float[] y = new float [2];

        x[0] = UnityEngine.Random.Range(pointA.position.x, pointC.position.x-0.5f);
        x[1] = UnityEngine.Random.Range(pointC.position.x+0.5f, pointB.position.x);
        y[0] = UnityEngine.Random.Range(pointC.position.y+0.5f, pointA.position.y);
        y[1] = UnityEngine.Random.Range(pointB.position.y, pointC.position.y-0.5f);

        ranges[0] = new Vector2(x[0], y[0]);
        ranges[1] = new Vector2(x[0], y[1]);
        ranges[2] = new Vector2(x[1], y[0]);
        ranges[3] = new Vector2(x[1], y[1]);

        //画面上に出ている吹き出しを検索
        GameObject[] tagObjects = GameObject.FindGameObjectsWithTag(tag);

        //実際に出す吹き出しを決める(前に出したnumを記憶しておく)
        prenum = SelectBubble(prenum);

        //画面上に出ている吹き出しを参考にどこに吹き出しを出すか決める
        if (tagObjects.Length > 0)
        {
            foreach (GameObject obj in tagObjects)
            {
                if (obj.transform.position.x < pointC.transform.position.x)
                {
                    if (obj.transform.position.y > pointC.transform.position.y) nums[0]++;
                    else nums[1]++;
                }
                else
                {
                    if (obj.transform.position.y > pointC.transform.position.y) nums[2]++;
                    else nums[3]++;
                }
            }

            int min = nums.Min();

            int index = Array.IndexOf(nums, min);
            //Debug.Log(nums[0]+", " + nums[1]+", " + nums[2]+", "+nums[3]);

            Instantiate(bubbleObj, ranges[index],   bubbleObj.transform.rotation);
        }
        else
        {
            Instantiate(bubbleObj, ranges[UnityEngine.Random.Range(0, 4)],bubbleObj.transform.rotation);
        }
        
    }

    //ふきだしの種類を決める(醤油かガリかなど)
    private int SelectBubble(int prenum)
    {
        int num = UnityEngine.Random.Range(0, numAndItemName.Count);

        if(num == prenum)
        {
            while (num == prenum)
            {
                num = UnityEngine.Random.Range(0, numAndItemName.Count);
            }
        }        

        ItemTypeSc itemTypeSc = bubbleObj.GetComponent<ItemTypeSc>();
        itemTypeSc.type = numAndItemType[num];

        GameObject canvas = bubbleObj.transform.GetChild(0).gameObject;


        renderer = bubbleObj.GetComponent<Renderer>();
        renderer.sortingOrder = counter;
        canvas.GetComponent<Canvas>().sortingOrder = bubbleObj.GetComponent<SpriteRenderer>().sortingOrder;

        bubbleObj.GetComponent<SpriteRenderer>().sprite = ItemSprite[num];

        itemText = canvas.transform.GetChild(0).gameObject;
        //itemText.transform.localPosition = bubbleObj.transform.position;
        itemText.GetComponent<TextMeshProUGUI>().text = numAndItemName[num]; //+ "\n取って~";

        return num;
    }

        
}
