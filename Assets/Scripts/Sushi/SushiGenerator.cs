using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SushiGenerator : MonoBehaviour
{
    public GameObject sushiObj;
    TimeManager timeManager;
    GameMode gameMode;

    [SerializeField]private float delTime;
    [SerializeField]int totalTime = 0;
    [SerializeField] private float interval = 1.1f;

    [SerializeField] public bool omomi = true;

    [SerializeField][Tooltip("寿司の枠数")]int maxCount;
    [SerializeField][Tooltip("流れる寿司の数")]int sushiCount = 24;

    List<bool> goSushi = new List<bool>();
    
    List<int> indexList = new List<int>();

    int[] nums = new int[] { 0, 0, 0 }; //序盤、中盤、終盤に配置する寿司の数
    int[] maxNums = new int[] { 17, 17, 17 }; //序盤、中盤、終盤に配置できる寿司の枠数(19×3)

    //エンドレスモードでの寿司の排出ペース
    float delTime_pace = 0;
    float interval_pace = 10f;
    float rate_pace = 1f; //エンドレスでintervalを変更するときの倍率

    [SerializeField] GameObject generateBalancerObj;
    GenerateBalancer generateBalancer;

    // Start is called before the first frame update
    void Start()
    {
        //gameState取得のため
        GameObject canvas = GameObject.FindGameObjectWithTag("canvas");
        timeManager = canvas.GetComponent<TimeManager>();

        //エンドレスモードかどうか
        gameMode = canvas.GetComponent<GameMode>();

        if(generateBalancerObj != null )
        {
            generateBalancer = generateBalancerObj.GetComponent<GenerateBalancer>();
        }

        interval = 2.9f;

        if (gameMode.isScored)
        {
            #region //流れる寿司の数や流れるタイミングを決める処理
            maxCount = timeManager.maxTime - 3;

            //一旦寿司を流すか否かをfalseで初期化 バグ対策でちょっと大きめにサイズを取る
            for (int i = 0; i < maxCount + 5; i++)
            {
                goSushi.Add(false);
            }

            if (omomi)
            {
                nums[0] = 6;
                nums[1] = 8;
                nums[2] = 9;

                //序盤中盤終盤それぞれの寿司の配置場所を確定する
                List<int> earlyIndexList = GetRandom(maxNums[0], nums[0], 0);
                List<int> middleIndexList = GetRandom(maxNums[1], nums[1], maxNums[0]);
                List<int> lastIndexList = GetRandom(maxNums[2], nums[2], maxNums[0] + maxNums[1]);

                indexList = Unit3List(earlyIndexList, middleIndexList, lastIndexList);
            }

            else
            {
                //indexList = GetRandom(maxCount, sushiCount, 0);←0から57で完全ランダムにしたいときはこっち
                nums[0] = 7;
                nums[1] = 8;
                nums[2] = 8;

                //序盤中盤終盤それぞれの寿司の配置場所を確定する
                List<int> earlyIndexList = GetRandom(maxNums[0], nums[0], 0);
                List<int> middleIndexList = GetRandom(maxNums[1], nums[1], maxNums[0]);
                List<int> lastIndexList = GetRandom(maxNums[2], nums[2], maxNums[0] + maxNums[1]);

                indexList = Unit3List(earlyIndexList, middleIndexList, lastIndexList);
            }

            foreach (int i in indexList)
            {
                goSushi[i] = true;
            }
            #endregion
        }


        delTime = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        delTime += Time.deltaTime;
        delTime_pace += Time.deltaTime;

        if (timeManager.gameState == TimeManager.GameState.play)
        {
            if (gameMode.isScored)
            {
                interval = 1.1f;

                if (delTime >= interval)
                {
                    if (goSushi[totalTime]) StartCoroutine(Generate());
                    delTime = 0;
                    totalTime++;
                }
            }
            else //エンドレスモードならintervalとspeedは時間経過とともに変化させる
            {
                if (delTime_pace > interval_pace && interval > 0.6f)
                {
                    if(rate_pace <= 3)
                    {
                        interval -= 0.3f;
                    }
                    else if(rate_pace <= 8)
                    {
                        interval -= 0.2f;
                    }
                    else
                    {
                        interval -= 0.1f;
                    }

                    delTime_pace = 0;
                    rate_pace++;

                    Debug.Log("Pace Up!!");

                    if(rate_pace <= 4)
                    {
                        interval_pace = 6f;
                    }
                    else if(rate_pace <= 7)
                    {
                        interval_pace = 10f;
                    }
                    else if(rate_pace <= 10)
                    {
                        interval_pace = 40f;
                    }
                    else
                    {
                        interval_pace = 60f;
                    }
                }

                if(delTime >= interval)
                {
                    //2つのGeneratorのどちらか一方からのみしか1度に流さないのでそれを制御させる関数を呼ぶ．(2回呼んだら意味ない)
                    if (omomi)generateBalancer.SelectAndGenerate();                    
                    delTime = 0;
                }
            }
        }
    }

    public IEnumerator Generate()
    {
        float time = 0;
        if(!gameMode.isScored)
        {
            time = UnityEngine.Random.Range(0, 0.3f);
        }

        yield return new WaitForSeconds(time);
        Instantiate(sushiObj, transform.position, sushiObj.transform.rotation);
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

    //3つのリストを連結
    public List<int> Unit3List(List<int> listA, List<int> listB, List<int> listC)
    {
        var list = (listA.Concat(listB).ToList()).Concat(listC).ToList();

        return list;
    }
}