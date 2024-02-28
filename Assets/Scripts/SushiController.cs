using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using static UnityEditor.Progress;

public class SushiController : MonoBehaviour
{
    //ドラッグ処理関連
    private Vector3 offset;
    public Vector3 iniPos;
    public GameObject getMousePosObj;
    GetMousePosSc getMousePosSc;

    public GameObject tableObj;
    private GameObject frontObj; //手前のオブジェクトを取得

    [SerializeField] private bool onSushi = false; //カーソルと寿司が重なってるときtrue
    [SerializeField]private bool sushiRay = false; //寿司ドラッグ時マウスからrayを飛ばすか否か

    private bool preorder = false; //orderのtrueorfalseを決めるのに使う
    [SerializeField] private bool order = false;

    [SerializeField] private GameObject sushiPos;

    Rigidbody2D rbody;
    [SerializeField] private float speed = 0f;

    new Renderer renderer;

    [SerializeField] private bool toRight = true; //右に流れる寿司なのか
    bool speedCheck = false;

    GameManager gameManager;
    TimeManager timeManager;

    // Start is called before the first frame update
    void Start()
    {
        getMousePosObj = GameObject.FindWithTag("mousePos");
        getMousePosSc = getMousePosObj.GetComponent<GetMousePosSc>();

        iniPos = transform.position;
        sushiPos = transform.root.gameObject;

        rbody = sushiPos.GetComponent<Rigidbody2D>();
        speed = rbody.velocity.x;

        renderer = GetComponent<Renderer>();

        //ScorePlusをするため
        GameObject canvas = GameObject.FindGameObjectWithTag("canvas");
        gameManager = canvas.GetComponent<GameManager>();

        timeManager = canvas.GetComponent<TimeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(timeManager.gameState == TimeManager.GameState.play)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(GetMousePos(), Vector3.forward);

            #region 寿司ドラッグ時の処理
            if (sushiRay)
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
            #endregion

            #region 寿司の上にカーソルを乗せた時とそこから離した時の処理
            else if (!sushiRay)
            {
                Dictionary<GameObject, int> keyValuePairs = new Dictionary<GameObject, int>(); //GameObjectとsortingLayerを格納

                if (hits != null)
                {
                    //rayがあたったもののうち、寿司と吹き出しのもののsortingLayerを調べる
                    foreach (RaycastHit2D hit in hits)
                    {
                        if (hit.collider.gameObject.tag == "BubbleNormal" || hit.collider.gameObject.tag == "Sushi")
                        {
                            Renderer renderer = hit.collider.gameObject.GetComponent<Renderer>();
                            keyValuePairs.Add(hit.collider.gameObject, renderer.sortingOrder);
                        }
                    }

                    //そもそも寿司と吹き出しに1個もヒットしないとき
                    if (keyValuePairs.Count <= 0)
                    {
                        if (frontObj != null) frontObj = null;
                    }

                    //sortingOrderが最大のものが寿司ならsushiRay=true
                    if (keyValuePairs.Count > 0)
                    {
                        frontObj = keyValuePairs.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
                    }

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
                    gameManager.ScorePlus(GameManager.ScoreType.sushi);
                    Destroy(gameObject.transform.root.gameObject);
                }
                else
                {
                    ResetPos();
                }

                renderer.sortingOrder = 5;
                sushiRay = false;
            }
            #endregion

        }
        else
        {
            ResetPos() ;
        }

    }

    private void FixedUpdate()
    {
        if (toRight)
        {
            if (speed != 2.0f) speed = 2.0f;
        }
        else
        {
            if (speed != -2.0f) speed = -2.0f;
        }

        if (!speedCheck)
        {
            rbody.velocity = new Vector2(speed, 0f);
            speedCheck = true;
        }
    }

    private Vector3 GetMousePos()
    {
        return getMousePosSc.GetMousePos();
    }

    
    
    private void ResetPos()
    {
        transform.position = sushiPos.transform.position;
    }
}
