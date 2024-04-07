using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public class SushiController : MonoBehaviour
{
    //ドラッグ処理関連
    private Vector3 offset;
    [SerializeField]public Vector3 iniPos;
    public GameObject getMousePosObj;
    protected GetMousePosSc getMousePosSc;

    public GameObject destroyObj;

    public GameObject tableObj;
    protected GameObject frontObj; //手前のオブジェクトを取得

    [SerializeField] private bool onSushi = false; //カーソルと寿司が重なってるときtrue
    [SerializeField]private bool sushiRay = false; //寿司ドラッグ時マウスからrayを飛ばすか否か

    protected bool preorder = false; //orderのtrueorfalseを決めるのに使う
    [SerializeField] protected bool order = false;

    [SerializeField] protected GameObject sushiPos;

    protected Rigidbody2D rbody;

    new Renderer renderer;

    protected ScoreManager scoreManager;
    TimeManager timeManager;

    // Start is called before the first frame update
    public virtual void Start()
    {
        getMousePosObj = GameObject.FindWithTag("mousePos");
        getMousePosSc = getMousePosObj.GetComponent<GetMousePosSc>();

        iniPos = transform.localPosition;
        sushiPos = transform.root.gameObject;
        
        rbody = sushiPos.GetComponent<Rigidbody2D>();
        renderer = GetComponent<Renderer>();
        

        //ScorePlusをするため
        GameObject canvas = GameObject.FindGameObjectWithTag("canvas");
        scoreManager = canvas.GetComponent<ScoreManager>();

        timeManager = canvas.GetComponent<TimeManager>();
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
                    string[] tags = { "Sushi", "BubbleNormal", "BubbleOrder", "OrderSushi" };
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
                if (!sushiRay) sushiRay = true;
                offset = transform.position - GetMousePos();
                renderer.sortingOrder = 300;
                renderer.sortingLayerName = "BubbleLayer";
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
                    GetScore();
                    Destroy(gameObject);
                    if(destroyObj != null) Destroy(destroyObj);
                }
                else
                {
                    ResetPos();
                }

                renderer.sortingOrder = 5;
                renderer.sortingLayerName = "Default";
                sushiRay = false;
            }
            #endregion
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
            //rayがあたったものの中にテーブルがあればorder=true(吹き出しと重なっててもいい)なければfalse
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject.tag == "Table")
                {
                    if (!preorder) preorder = true;
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
