using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    //[SerializeField] new Camera camera;
    private Vector3 offset;
    public Vector3 iniPos;

    public GameObject getMousePosObj; //マウス座標取得のオブジェクト
    GetMousePosSc getMousePosSc;

    [SerializeField]private bool onItem = false; //カーソルとアイテムが重なってるときtrue
    [SerializeField]private bool itemRay = false; //アイテムドラッグ時マウスからrayを飛ばすか否か
    private GameObject frontObj;

    private GameObject bubbleObj;
    [SerializeField] private bool order = false; //マウスから離したら注文対応できるという状況

    public GameObject regenerator;
    Regenerator regeneratorSc;

    new Renderer renderer;

    GameManager gameManager;
    TimeManager timeManager;

    // Start is called before the first frame update
    void Start()
    {
        iniPos = transform.position;

        getMousePosSc = getMousePosObj.GetComponent<GetMousePosSc>();

        regeneratorSc = regenerator.GetComponent<Regenerator>();

        renderer = gameObject.GetComponent<Renderer>();

        //ScorePlusをするため
        GameObject canvas = GameObject.FindGameObjectWithTag("canvas");
        gameManager = canvas.GetComponent<GameManager>();

        //gameState取得のため
        timeManager = canvas.GetComponent<TimeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(timeManager.gameState == TimeManager.GameState.play)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(GetMousePos(), Vector2.zero);

            #region アイテムドラッグでの吹き出しに当たった時の処理
            if (itemRay)
            {
                Dictionary<GameObject, int> keyValuePairs = new Dictionary<GameObject, int>(); //GameObjectとsortingLayerを格納

                if (hits != null)
                {
                    //rayがあたったもののうち、吹き出しのもののsortingLayerを調べる
                    foreach (RaycastHit2D hit in hits)
                    {
                        if (hit.collider.gameObject.tag == "BubbleNormal")
                        {
                            Renderer renderer = hit.collider.gameObject.GetComponent<Renderer>();
                            keyValuePairs.Add(hit.collider.gameObject, renderer.sortingOrder);
                        }
                    }

                    //そもそも吹き出しに1個もヒットしないとき
                    if (keyValuePairs.Count <= 0)
                    {
                        if (order) order = false;
                        if (bubbleObj != null) bubbleObj = null;
                    }

                    //sortingOrderが最大のものがbubbleObjとして選ばれる
                    if (keyValuePairs.Count > 0)
                    {
                        bubbleObj = keyValuePairs.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
                    }

                    //bubbleObjとアイテムの種類が一致してるなら対応可能(order=true)
                    if (bubbleObj != null)
                    {
                        ItemTypeSc bubbleType = bubbleObj.GetComponent<ItemTypeSc>();
                        ItemTypeSc itemType = gameObject.GetComponent<ItemTypeSc>();

                        if (bubbleType.type == itemType.type)
                        {
                            if (!order) order = true;
                        }
                        else
                        {
                            if (order) order = false;
                        }
                    }
                }
            }
            #endregion

            #region アイテムの上にカーソルを乗せた時とそこから離した時の処理
            else if (!itemRay)
            {
                Dictionary<GameObject, int> keyValuePairs = new Dictionary<GameObject, int>(); //GameObjectとsortingLayerを格納

                if (hits != null)
                {
                    //rayがあたったもののうち、寿司と吹き出しのもののsortingLayerを調べる
                    foreach (RaycastHit2D hit in hits)
                    {
                        if (hit.collider.gameObject.tag == "BubbleNormal" || hit.collider.gameObject.tag == "Item")
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

                    //sortingOrderが最大のものが寿司ならitemRay=true
                    if (keyValuePairs.Count > 0)
                    {
                        frontObj = keyValuePairs.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
                    }

                    if (frontObj == gameObject)
                    {
                        onItem = true;
                    }
                    else
                    {
                        onItem = false;
                    }
                }
            }
            #endregion

            #region ドラッグで移動させる処理
            if (onItem && Input.GetMouseButtonDown(0))
            {
                if (!itemRay) itemRay = true;
                offset = transform.position - GetMousePos();
                renderer.sortingOrder = 300;
            }

            if (itemRay && Input.GetMouseButton(0))
            {
                transform.position = GetMousePos() + offset;
            }

            if (itemRay && Input.GetMouseButtonUp(0))
            {
                if (order) //吹き出しの注文に対応完了
                {
                    order = false;
                    gameManager.ScorePlus(GameManager.ScoreType.bubbleNormal);
                    Destroy(bubbleObj);
                    regeneratorSc.StartCoroutine("Regenerate", gameObject);
                }
                else
                {
                    ResetPos();
                }

                renderer.sortingOrder = 5;
                itemRay = false;
            }
            #endregion

        }
        else
        {
            ResetPos() ;
        }

    }
    
    private Vector3 GetMousePos()
    {
        return getMousePosSc.GetMousePos();
    }
    
    

    public void ResetPos()
    {
        transform.position = iniPos;
    }
}
