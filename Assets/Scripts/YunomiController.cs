using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;


public class YunomiController : MonoBehaviour
{
    public GameObject yunomiObj;
    private Vector3 offset;
    public Vector3 iniPos;

    GameObject getMousePosObj; //マウス座標取得のオブジェクト
    GetMousePosSc getMousePosSc;

    [SerializeField] private bool onItem = false; //カーソルとアイテムが重なってるときtrue
    [SerializeField] private bool itemRay = false; //アイテムドラッグ時マウスからrayを飛ばすか否か
    private GameObject frontObjA;
    private GameObject frontObjB;

    private GameObject bubbleObj;
    [SerializeField] private bool order = false; //マウスから離したら注文対応できるという状況

    new Renderer renderer;
    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite normalSprite;
    [SerializeField] Sprite onSprite;

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

    int bonusScore;

    //スコアを取るモードか取らないか
    GameMode gameMode;
    private bool isScored;

    AudioSource audioSource;
    public AudioClip correctPutSound;

    [SerializeField] GameObject scorePlusTextObj;

    [SerializeField] ParticleSystem particleSystem;

    // Start is called before the first frame update
    void Start()
    {
        yunomiObj.SetActive(false);

        iniPos = transform.position;

        getMousePosObj = GameObject.FindGameObjectWithTag("mousePos");
        getMousePosSc = getMousePosObj.GetComponent<GetMousePosSc>();

        renderer = yunomiObj.GetComponent<Renderer>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        //デフォルトのスプライト
        spriteRenderer.sprite = normalSprite;

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
        if (timeManager.gameState == TimeManager.GameState.play)
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
                        if (order) order = false;
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

                    if (spriteRenderer.sprite != onSprite)
                    {
                        spriteRenderer.sprite = onSprite;
                    }
                }
                else
                {
                    onItem = false;

                    if (spriteRenderer.sprite != normalSprite)
                    {
                        spriteRenderer.sprite = normalSprite;
                    }
                }
            }
            #endregion

            #region ドラッグで移動させる処理
            if (onItem && Input.GetMouseButtonDown(0))
            {
                yunomiObj.SetActive(true);
                if (!itemRay) itemRay = true;
                offset = new Vector3(0, 0, 1);
                renderer.sortingOrder = 300;
                renderer.sortingLayerName = "BubbleLayer";
                SoundManager.soundManager.SEPlay(SEType.SushiClick);
            }

            if (itemRay && Input.GetMouseButton(0))
            {
                yunomiObj.transform.position = GetMousePos() + offset;
            }

            if (itemRay && Input.GetMouseButtonUp(0))
            {
                if (order) //吹き出しの注文に対応完了
                {
                    order = false;
                    bonusScore = (int)bubbleObj.GetComponent<Bubble_test>().bonusScore;
                    GetScore();
                    SoundManager.soundManager.SEPlay(SEType.Get);
                    Instantiate(particleSystem, yunomiObj.transform.position, particleSystem.transform.rotation);
                    Destroy(bubbleObj);
                }               

                renderer.sortingOrder = 5;
                renderer.sortingLayerName = "ItemLayer";
                itemRay = false;

                yunomiObj.SetActive(false);
            }
            #endregion

        }
        else
        {
            ResetPos();
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
            scorePlusTextObj2.transform.position = yunomiObj.transform.position;

            ScorePlusText scorePlusText = scorePlusTextObj2.GetComponent<ScorePlusText>();

            bool isMax = false;
            if (bonusScore == 100) isMax = true;
            scorePlusText.ScorePlusAnime(scoreManager.baseScore[ScoreManager.ScoreType.bubbleNormal] + bonusScore, isMax);
        }
    }

    private Vector3 GetMousePos()
    {
        return getMousePosSc.GetMousePos();
    }



    public void ResetPos()
    {
        yunomiObj.transform.position = iniPos;
    }
}
