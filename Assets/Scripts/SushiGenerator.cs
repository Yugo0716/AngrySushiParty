using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SushiGenerator : MonoBehaviour
{
    public GameObject sushiObj;
    public GameObject timeManagerObj;
    TimeManager timeManager;

    private float time;
    [SerializeField] private float interval = 3.0f;

    int sushiCount = 57;
    List<bool> goSushi = new List<bool>();
    
    //List<int> indexList = new List<int>();

    [SerializeField] AnimationCurve curve;

    List<int> tests = new List<int>();
    int test = 0;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < sushiCount; i++)
        {
            goSushi.Add(false);
        }

        List<int> indexList = GetRandom(sushiCount, 30);

        
        foreach (int i in indexList)
        {
            goSushi[i] = true;
        }

        foreach (bool b in goSushi) Debug.Log(b);

        time = 0f;

        //gameState取得のため
        GameObject canvas = GameObject.FindGameObjectWithTag("canvas");
        timeManager = canvas.GetComponent<TimeManager>();
        
        /*
        for(int i = 0; i < 30; i++)
        {
            tests[i] = (int)(CurveWeightedRandom(curve) * 56);
        }

        foreach(int i in tests) Debug.Log(tests[i]);*/
        /*
        test = (int)(CurveWeightedRandom(curve) * 56);
        Debug.Log(test);*/
    }

    // Update is called once per frame
    void Update()
    {
        if(timeManager.gameState == TimeManager.GameState.play)
        {
            time += Time.deltaTime;

            if (time >= interval)
            {
                Generate(sushiObj);
                time = 0;
            }
        }
    }

    void Generate(GameObject generateObj)
    {
        Instantiate(generateObj, transform.position, generateObj.transform.rotation);
    }

    float CurveWeightedRandom(AnimationCurve curve)
    {
        return curve.Evaluate(UnityEngine.Random.value);
    }

    /*
    public IEnumerable<int> GetRandomN<T> (List<T> collection, int n)
    {
        if (n > collection.Count)
        {
            throw new ArgumentOutOfRangeException("リストの要素数よりnが大きいです。");
        }

        var indexList = new List<int>(collection.Count);
        var returnList = new List<int>(n);
        for (int p = 0; p < collection.Count; p++) indexList.Add(p);

        for (int i = 0; i < n; i++)
        {
            int index = UnityEngine.Random.Range(0, indexList.Count);
            int value = indexList[index];
            indexList.RemoveAt(index);
            returnList.Add(value);
            yield return returnList[value];
        }
    }*/
    public List<int> GetRandom(int size, int n)
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
            returnList.Add(value);
        }
        return returnList;
    }
}
/*
public static class IListExtensions
{
    public static IEnumerable<int> GetRandomN<T>(this IList<T> collection, int n)
    {
        if (n > collection.Count)
        {
            throw new ArgumentOutOfRangeException("リストの要素数よりnが大きいです。");
        }

        var indexList = new List<int>(collection.Count);
        var returnList = new List<int>();
        for (int p = 0; p < collection.Count; p++) indexList.Add(p);

        for (int i = 0; i < n; i++)
        {
            int index = UnityEngine.Random.Range(0, indexList.Count);
            int value = indexList[index];
            indexList.RemoveAt(index);
            returnList.Add(value);
            yield return returnList[value];
        }
    }
}*/