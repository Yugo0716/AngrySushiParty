using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class BubbleGenerator : MonoBehaviour
{
    public GameObject[] bubbleObjs;
    [SerializeField] GameObject generateBubbleObj;

    public Transform pointA;
    public Transform pointB;
    public Transform pointC;
    
    private float time;
    [SerializeField] float interval = 4.0f;
    [SerializeField] private int counter = 8;
    new Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time >= interval)
        {
            Generate("BubbleNormal");
            counter++;
            time = 0;
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

        //実際に出す吹き出しを決める
        generateBubbleObj = SelectBubble();

        //だんだん手前に配置する
        renderer = generateBubbleObj.GetComponent<Renderer>();
        renderer.sortingOrder = counter;

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

            Instantiate(generateBubbleObj, ranges[index], generateBubbleObj.transform.rotation);
        }
        else
        {
            Instantiate(generateBubbleObj, ranges[UnityEngine.Random.Range(0, 4)],generateBubbleObj.transform.rotation);
        }
        
    }

    //ふきだしの種類を決める(醤油かガリかなど)
    private GameObject SelectBubble()
    {
        int rnd = UnityEngine.Random.Range(0, bubbleObjs.Length);

        return bubbleObjs[rnd];
    }

        
}
