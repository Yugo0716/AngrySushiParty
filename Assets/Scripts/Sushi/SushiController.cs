using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static ItemTypeSc;

public class SushiController : MonoBehaviour
{
    //�h���b�O�����֘A
    private Vector3 offset;
    [SerializeField]public Vector3 iniPos;
    public GameObject getMousePosObj;
    protected GetMousePosSc getMousePosSc;

    public GameObject destroyObj;

    [SerializeField]GameObject tableObj;
    protected GameObject frontObj; //��O�̃I�u�W�F�N�g���擾

    [SerializeField] private bool onSushi = false; //�J�[�\���Ǝ��i���d�Ȃ��Ă�Ƃ�true
    public bool sushiRay = false; //���i�h���b�O���}�E�X����ray���΂����ۂ�

    protected bool preorder = false; //order��trueorfalse�����߂�̂Ɏg��
    [SerializeField] protected bool order = false;

    [SerializeField] protected GameObject sushiPos;

    protected Rigidbody2D rbody;

    protected GameObject canvas;

    new Renderer renderer;

    protected SpriteRenderer spriteRenderer;
    public List<Sprite> normalSushiSprites = new List<Sprite>();
    public List<Sprite> onSushiSprites = new List<Sprite>();

    protected ScoreManager scoreManager;
    protected TimeManager timeManager;

    protected string defaultLayerName;

    AudioSource audioSource;
    public AudioClip correctPutSound;

    //�X�R�A����郂�[�h�����Ȃ���
    protected GameMode gameMode;
    private bool isScored;

    [SerializeField] protected GameObject scorePlusTextObj;

    protected Dictionary<SushiTypeSc.SushiType, int> sushiTypeAndNum = new Dictionary<SushiTypeSc.SushiType, int>()
    {
        {SushiTypeSc.SushiType.Tamago, 0 }, {SushiTypeSc.SushiType.Ebi, 1}, {SushiTypeSc.SushiType.Ika, 2}, {SushiTypeSc.SushiType.Maguro, 3}
        , {SushiTypeSc.SushiType.Ikura, 4}
    };

    protected SushiTypeSc sushiType;

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
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        sushiType = GetComponent<SushiTypeSc>();
        //�f�t�H���g�̃X�v���C�g
        spriteRenderer.sprite = normalSushiSprites[sushiTypeAndNum[sushiType.type]];


        defaultLayerName = renderer.sortingLayerName;

        //ScorePlus�����邽��
        canvas = GameObject.FindGameObjectWithTag("canvas");
        scoreManager = canvas.GetComponent<ScoreManager>();

        timeManager = canvas.GetComponent<TimeManager>();

        //GameMode�擾�̂���
        gameMode = canvas.GetComponent<GameMode>();
        isScored = gameMode.isScored;

        //�T�E���h
        audioSource = GameObject.FindGameObjectWithTag("AudioSource").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if(timeManager.gameState == TimeManager.GameState.play)
        {
            RaycastHit2D[] hits = getMousePosSc.GetRaycastHit();

            #region ���i�h���b�O���̏���
            if (sushiRay)
            {
                SushiDrag(hits);
            }
            
            #endregion

            #region ���i�̏�ɃJ�[�\�����悹�����Ƃ������痣�������̏���
            else if (!sushiRay)
            {
                Dictionary<GameObject, int> keyValuePairs = new Dictionary<GameObject, int>(); //GameObject��sortingLayer���i�[

                if (hits != null)
                {
                    string[] tags = { "Sushi", "BubbleNormal", "BubbleOrder", "OrderSushi", "Frame" };
                    frontObj = getMousePosSc.GetFrontObj(hits, tags);

                    if (frontObj == gameObject)
                    {
                        if (!onSushi) onSushi = true;

                        if (spriteRenderer.sprite != onSushiSprites[sushiTypeAndNum[sushiType.type]])
                        {
                            spriteRenderer.sprite = onSushiSprites[sushiTypeAndNum[sushiType.type]];
                        }
                    }
                    else
                    {
                        if (onSushi) onSushi = false;

                        if (spriteRenderer.sprite != normalSushiSprites[sushiTypeAndNum[sushiType.type]])
                        {
                            spriteRenderer.sprite = normalSushiSprites[sushiTypeAndNum[sushiType.type]];
                        }
                    }
                }
            }
            #endregion

            #region �h���b�O�ňړ������鏈��
            if (onSushi && Input.GetMouseButtonDown(0))
            {
               //����Ń^�b�`�����ɂł��Ă邩�����Ă݂�
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
                if (order) //���i�Q�b�g
                {
                    order = false;
                    audioSource.PlayOneShot(correctPutSound);

                    GetScore();
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
            spriteRenderer.sprite = normalSushiSprites[sushiTypeAndNum[sushiType.type]];
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

            //ray�������������̂̒��Ƀe�[�u���������order=true(�����o���Əd�Ȃ��ĂĂ�����)�Ȃ����false
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
        scoreManager.ScorePlus(ScoreManager.ScoreType.sushi, 0);

        GameObject scorePlusCanvas = GameObject.FindGameObjectWithTag("ScorePlusCanvas");

        if (gameMode.isScored)
        {
            GameObject scorePlusTextObj2 = Instantiate(scorePlusTextObj);
            scorePlusTextObj2.transform.SetParent(scorePlusCanvas.transform, false);
            scorePlusTextObj2.transform.position = gameObject.transform.position;

            ScorePlusText scorePlusText = scorePlusTextObj2.GetComponent<ScorePlusText>();
            scorePlusText.ScorePlusAnime(scoreManager.baseScore[ScoreManager.ScoreType.sushi], false);
        }        
    }
    
    
    private void ResetPos()
    {
        transform.position = sushiPos.transform.position + iniPos;
    }
}
