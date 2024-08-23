using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class SushiController : MonoBehaviour
{
    //ドラッグ処理関連
    private Vector3 offset;
    [SerializeField]public Vector3 iniPos;
    public GameObject getMousePosObj;
    protected GetMousePosSc getMousePosSc;

    public GameObject destroyObj;

    [SerializeField]GameObject tableObj;
    protected GameObject frontObj; //手前のオブジェクトを取得

    [SerializeField] private bool onSushi = false; //カーソルと寿司が重なってるときtrue
    public bool sushiRay = false; //寿司ドラッグ時マウスからrayを飛ばすか否か

    protected bool preorder = false; //orderのtrueorfalseを決めるのに使う
    [SerializeField] protected bool order = false;

    [SerializeField] protected GameObject sushiPos;

    protected Rigidbody2D rbody;

    new Renderer renderer;

    protected ScoreManager scoreManager;
    protected TimeManager timeManager;

    protected string defaultLayerName;

    AudioSource audioSource;
    public AudioClip correctPutSound;

    //スコアを取るモードか取らないか
    protected GameMode gameMode;
    private bool isScored;

    // Start is called before the first frame update
    public virtual void Start()
    {
        getMousePosObj = GameObject.FindWithTag("mousePos");
        getMousePosSc = getMousePosObj.GetComponent<GetMousePosSc>();

        tableObj = GameObject.FindGameObjectWithTag("Table");

        iniPos = transform.localPosition;
        sushiPos = transform.parent.gameObject;
        
        rbody = sushiPos.GetComponent<Rigidbody2D>();
        renderer = GetComponent<Renderer>();

        defaultLayerName = renderer.sortingLayerName;

        //ScorePlusをするため
        GameObject canvas = GameObject.FindGameObjectWithTag("canvas");
        scoreManager = canvas.GetComponent<ScoreManager>();

        timeManager = canvas.GetComponent<TimeManager>();

        //GameMode取得のため
        gameMode = canvas.GetComponent<GameMode>();
        isScored = gameMode.isScored;

        //サウンド
        audioSource = GameObject.FindGameObjectWithTag("AudioSource").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if(timeManager.gameState == TimeManager.GameState.play)
        {
            RaycastHit2D[] hits = getMousePosSc.GetRaycastHit();

            #region 寿司ドラッグ時の処理
            if (sushiRay)
            {
                SushiDrag(hits);
            }
            
            #endregion

            #region 寿司の上にカーソルを乗せた時とそこから離した時の処理
            else if (!sushiRay)
            {
                Dictionary<GameObject, int> keyValuePairs = new Dictionary<GameObject, int>(); //GameObjectとsortingLayerを格納

                if (hits != null)
                {
                    string[] tags = { "Sushi", "BubbleNormal", "BubbleOrder", "OrderSushi", "Frame" };
                    frontObj = getMousePosSc.GetFrontObj(hits, tags);

                    if (frontObj == gameObject)
                    {
                        if (!onSushi) onSushi = true;
                    }
                    else
                    {
                        if (onSushi) onSushi = false;
                    }
                }
            }
            #endregion

            #region ドラッグで移動させる処理
            if (onSushi && Input.GetMouseButtonDown(0))
            {
               //これでタッチ無効にできてるか試してみる
                if (Input.touchCount == 0)
                {
                    if (!sushiRay) sushiRay = true;
                    offset = transform.position - GetMousePos();
                    renderer.sortingOrder = 300;
                    renderer.sortingLayerName = "BubbleLayer";
                }
            }

            if (sushiRay && Input.GetMouseButton(0))
            {
                transform.position = new Vector3(GetMousePos().x, GetMousePos().y, 0) + offset;
            }

            if (sushiRay && Input.GetMouseButtonUp(0))
            {
                if (order) //寿司ゲット
                {
                    order = false;
                    audioSource.PlayOneShot(correctPutSound);

                    if (isScored) GetScore();
                    scoreManager.CountPlus();

                    Destroy(gameObject);
                    if(destroyObj != null) Destroy(destroyObj);
                }
                else
                {
                    ResetPos();
                }

                renderer.sortingOrder = 5;
                renderer.sortingLayerName = defaultLayerName;
                sushiRay = false;
                tableObj.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);

            }
            #endregion
        }
        else
        {
            ResetPos() ;
            tableObj.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            renderer.sortingLayerName = defaultLayerName;
        }
    }

    private Vector3 GetMousePos()
    {
        return getMousePosSc.GetMousePos();
    }

    public virtual void SushiDrag(RaycastHit2D[] hits)
    {
        if (hits != null)
        {
            bool onTable = false;

            //rayがあたったものの中にテーブルがあればorder=true(吹き出しと重なっててもいい)なければfalse
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject == tableObj)
                {         
                    onTable = true;
                    break;                   
                }               
            }
            if (onTable)
            {
                if (!preorder) preorder = true;
                tableObj.GetComponent<SpriteRenderer>().color = new Color(0.67f, 0.67f, 0.67f, 1f);
            }
            else
            {
                if (tableObj != null)
                {
                    tableObj.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                }
            }
    

            if (preorder)
            {
                order = true;
                preorder = false;
            }
            else order = false;
        }
    }

    public virtual void GetScore()
    {
        scoreManager.ScorePlus(ScoreManager.ScoreType.sushi);
    }
    
    
    private void ResetPos()
    {
        transform.position = sushiPos.transform.position + iniPos;
    }
}
