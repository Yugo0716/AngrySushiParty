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

    public GameObject getMousePosObj; //�}�E�X���W�擾�̃I�u�W�F�N�g
    GetMousePosSc getMousePosSc;

    [SerializeField]private bool onItem = false; //�J�[�\���ƃA�C�e�����d�Ȃ��Ă�Ƃ�true
    [SerializeField]private bool itemRay = false; //�A�C�e���h���b�O���}�E�X����ray���΂����ۂ�
    private GameObject frontObjA;
    private GameObject frontObjB;

    private GameObject bubbleObj;
    [SerializeField] private bool order = false; //�}�E�X���痣�����璍���Ή��ł���Ƃ�����

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

        //ScorePlus�����邽��
        GameObject canvas = GameObject.FindGameObjectWithTag("canvas");
        scoreManager = canvas.GetComponent<ScoreManager>();

        //gameState�擾�̂���
        timeManager = canvas.GetComponent<TimeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(timeManager.gameState == TimeManager.GameState.play)
        {
            RaycastHit2D[] hits = getMousePosSc.GetRaycastHit();

            #region �A�C�e���h���b�O�ł̐����o���ɓ����������̏���
            if (itemRay)
            {
                Dictionary<GameObject, int> keyValuePairs = new Dictionary<GameObject, int>(); //GameObject��sortingLayer���i�[

                if (hits != null)
                {
                    string[] tagsA = { "BubbleNormal", "BubbleOrder" };
                    frontObjA = getMousePosSc.GetFrontObj(hits, tagsA);

                    //bubbleObj�ƃA�C�e���̎�ނ���v���Ă�Ȃ�Ή��\(order=true)
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

            #region �A�C�e���̏�ɃJ�[�\�����悹�����Ƃ������痣�������̏���
            else if (!itemRay)
            {
                Dictionary<GameObject, int> keyValuePairs = new Dictionary<GameObject, int>(); //GameObject��sortingLayer���i�[
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

            #region �h���b�O�ňړ������鏈��
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
                if (order) //�����o���̒����ɑΉ�����
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
