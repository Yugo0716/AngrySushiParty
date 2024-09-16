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

    GameObject getMousePosObj; //マウス座標取得のオブジェクト
    GetMousePosSc getMousePosSc;

    [SerializeField]private bool onItem = false; //カーソルとアイテムが重なってるときtrue
    [SerializeField]private bool itemRay = false; //アイテムドラッグ時マウスからrayを飛ばすか否か
    private GameObject frontObjA;
    private GameObject frontObjB;

    public GameObject bubbleObj;
    [SerializeField] public bool order = false; //マウスから離したら注文対応できるという状況

    public GameObject regenerator;
    Regenerator regeneratorSc;

    new Renderer renderer;

    ScoreManager scoreManager;
    TimeManager timeManager;

    Dictionary<ItemTypeSc.ItemType, int> itemTypeAndNum = new Dictionary<ItemTypeSc.ItemType, int>()
    {
        {ItemTypeSc.ItemType.shoyu, 0 }, {ItemTypeSc.ItemType.gari, 1}, {ItemTypeSc.ItemType.wasabi, 2}, {ItemTypeSc.ItemType.yunomi, 3}
    };

    public Sprite defaultSprite;

    public List<Sprite> bubbleSprite = new List<Sprite>();
    public List<Sprite> cBubbleSprite = new List<Sprite>();

    public GameObject preFrontObj = null;

    AudioSource audioSource;
    public AudioClip CorrectPutSound;

    int bonusScore;

    //スコアを取るモードか取らないか
    GameMode gameMode;
    private bool isScored;

    [SerializeField] GameObject scorePlusTextObj;

    // Start is called before the first frame update
    void Start()
    {
        iniPos = transform.position;

        getMousePosObj = GameObject.FindGameObjectWithTag("mousePos");
        getMousePosSc = getMousePosObj.GetComponent<GetMousePosSc>();

        regeneratorSc = regenerator.GetComponent<Regenerator>();

        renderer = gameObject.GetComponent<Renderer>();

        //ScorePlusをするため
        GameObject canvas = GameObject.FindGameObjectWithTag("canvas");
        scoreManager = canvas.GetComponent<ScoreManager>();

        //gameState取得のため
        timeManager = canvas.GetComponent<TimeManager>();

        //GameMode取得のため
        gameMode = canvas.GetComponent<GameMode>();
        isScored = gameMode.isScored;

        audioSource = GameObject.FindGameObjectWithTag("AudioSource").GetComponent<AudioSource>();
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

                        int itemNum = itemTypeAndNum[bubbleType.type];
                        defaultSprite = bubbleSprite[itemNum];

                        if (bubbleType.type == itemType.type)
                        {
                            if (!order) order = true;
                            bubbleObj.GetComponent<SpriteRenderer>().sprite = cBubbleSprite[itemNum];
                        }
                        else
                        {
                            if (order) order = false;
                            bubbleObj.GetComponent<SpriteRenderer>().sprite = defaultSprite;
                        }
                        preFrontObj = bubbleObj;
                    }
                    else
                    {
                        if(order) order = false;
                        if (preFrontObj != null && defaultSprite != null)
                        {
                            preFrontObj.GetComponent<SpriteRenderer>().sprite = defaultSprite;
                        }
                        
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
                    bonusScore = (int)bubbleObj.GetComponent<Bubble_test>().bonusScore;
                    GetScore();
                    audioSource.PlayOneShot(CorrectPutSound);
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

    void GetScore()
    {
        scoreManager.ScorePlus(ScoreManager.ScoreType.bubbleNormal, bonusScore);

        GameObject scorePlusCanvas = GameObject.FindGameObjectWithTag("ScorePlusCanvas");
        if (gameMode.isScored)
        {
            GameObject scorePlusTextObj2 = Instantiate(scorePlusTextObj);
            scorePlusTextObj2.transform.SetParent(scorePlusCanvas.transform, false);
            scorePlusTextObj2.transform.position = gameObject.transform.position;

            ScorePlusText scorePlusText = scorePlusTextObj2.GetComponent<ScorePlusText>();
            scorePlusText.ScorePlusAnime(scoreManager.baseScore[ScoreManager.ScoreType.bubbleNormal]+bonusScore);
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
