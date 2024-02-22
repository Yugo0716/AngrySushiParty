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

    public GameObject getMousePosObj; //�}�E�X���W�擾�̃I�u�W�F�N�g
    GetMousePosSc getMousePosSc;

    [SerializeField]private bool onItem = false; //�J�[�\���ƃA�C�e�����d�Ȃ��Ă�Ƃ�true
    [SerializeField]private bool itemRay = false; //�A�C�e���h���b�O���}�E�X����ray���΂����ۂ�
    private GameObject frontObj;

    private GameObject bubbleObj;
    [SerializeField] private bool order = false; //�}�E�X���痣�����璍���Ή��ł���Ƃ�����

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

        //ScorePlus�����邽��
        GameObject canvas = GameObject.FindGameObjectWithTag("canvas");
        gameManager = canvas.GetComponent<GameManager>();

        //gameState�擾�̂���
        timeManager = canvas.GetComponent<TimeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(timeManager.gameState == TimeManager.GameState.play)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(GetMousePos(), Vector2.zero);

            #region �A�C�e���h���b�O�ł̐����o���ɓ����������̏���
            if (itemRay)
            {
                Dictionary<GameObject, int> keyValuePairs = new Dictionary<GameObject, int>(); //GameObject��sortingLayer���i�[

                if (hits != null)
                {
                    //ray�������������̂̂����A�����o���̂��̂�sortingLayer�𒲂ׂ�
                    foreach (RaycastHit2D hit in hits)
                    {
                        if (hit.collider.gameObject.tag == "BubbleNormal")
                        {
                            Renderer renderer = hit.collider.gameObject.GetComponent<Renderer>();
                            keyValuePairs.Add(hit.collider.gameObject, renderer.sortingOrder);
                        }
                    }

                    //�������������o����1���q�b�g���Ȃ��Ƃ�
                    if (keyValuePairs.Count <= 0)
                    {
                        if (order) order = false;
                        if (bubbleObj != null) bubbleObj = null;
                    }

                    //sortingOrder���ő�̂��̂�bubbleObj�Ƃ��đI�΂��
                    if (keyValuePairs.Count > 0)
                    {
                        bubbleObj = keyValuePairs.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
                    }

                    //bubbleObj�ƃA�C�e���̎�ނ���v���Ă�Ȃ�Ή��\(order=true)
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

            #region �A�C�e���̏�ɃJ�[�\�����悹�����Ƃ������痣�������̏���
            else if (!itemRay)
            {
                Dictionary<GameObject, int> keyValuePairs = new Dictionary<GameObject, int>(); //GameObject��sortingLayer���i�[

                if (hits != null)
                {
                    //ray�������������̂̂����A���i�Ɛ����o���̂��̂�sortingLayer�𒲂ׂ�
                    foreach (RaycastHit2D hit in hits)
                    {
                        if (hit.collider.gameObject.tag == "BubbleNormal" || hit.collider.gameObject.tag == "Item")
                        {
                            Renderer renderer = hit.collider.gameObject.GetComponent<Renderer>();
                            keyValuePairs.Add(hit.collider.gameObject, renderer.sortingOrder);
                        }
                    }

                    //�����������i�Ɛ����o����1���q�b�g���Ȃ��Ƃ�
                    if (keyValuePairs.Count <= 0)
                    {
                        if (frontObj != null) frontObj = null;
                    }

                    //sortingOrder���ő�̂��̂����i�Ȃ�itemRay=true
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

            #region �h���b�O�ňړ������鏈��
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
                if (order) //�����o���̒����ɑΉ�����
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
