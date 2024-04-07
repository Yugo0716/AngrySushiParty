using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
//using UnityEditor.U2D.Aseprite;
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
    private GameObject frontObjA;
    private GameObject frontObjB;

    private GameObject bubbleObj;
    [SerializeField] private bool order = false; //マウスから離したら注文対応できるという状況

    public GameObject regenerator;
    Regenerator regeneratorSc;

    new Renderer renderer;

    ScoreManager scoreManager;
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
        scoreManager = canvas.GetComponent<ScoreManager>();

        //gameState取得のため
        timeManager = canvas.GetComponent<TimeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(timeManager.gameState == TimeManager.GameState.play)
        {
            RaycastHit2D[] hits = getMousePosSc.GetRaycastHit();

            #region アイテムドラッグでの吹き出しに当たった時の処理
            if (itemRay)
            {
                Dictionary<GameObject, int> keyValuePairs = new Dictionary<GameObject, int>(); //GameObjectとsortingLayerを格納

                if (hits != null)
                {
                    string[] tagsA = { "BubbleNormal", "BubbleOrder" };
                    frontObjA = getMousePosSc.GetFrontObj(hits, tagsA);

                    //bubbleObjとアイテムの種類が一致してるなら対応可能(order=true)
                    if (frontObjA != null && frontObjA.tag == "BubbleNormal")
                    {
                        bubbleObj = frontObjA;
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
                    else
                    {
                        if(order) order = false;
                    }
                }
            }
            #endregion

            #region アイテムの上にカーソルを乗せた時とそこから離した時の処理
            else if (!itemRay)
            {
                Dictionary<GameObject, int> keyValuePairs = new Dictionary<GameObject, int>(); //GameObjectとsortingLayerを格納
                string[] tagsB = { "Item", "BubbleNormal", "BubbleOrder" };

                frontObjB = getMousePosSc.GetFrontObj(hits, tagsB);

                if (frontObjB == gameObject)
                {
                    onItem = true;
                }
                else
                {
                    onItem = false;
                }
            }
            #endregion

            #region ドラッグで移動させる処理
            if (onItem && Input.GetMouseButtonDown(0))
            {
                if (!itemRay) itemRay = true;
                offset = transform.position - GetMousePos();
                renderer.sortingOrder = 300;
                renderer.sortingLayerName = "BubbleLayer";
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
                    scoreManager.ScorePlus(ScoreManager.ScoreType.bubbleNormal);
                    Destroy(bubbleObj);
                    regeneratorSc.StartCoroutine("Regenerate", gameObject);
                }
                else
                {
                    ResetPos();
                }

                renderer.sortingOrder = 5;
                renderer.sortingLayerName = "ItemLayer";
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
