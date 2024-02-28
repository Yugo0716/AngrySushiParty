using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using static UnityEditor.Progress;

public class SushiController : MonoBehaviour
{
    //�h���b�O�����֘A
    private Vector3 offset;
    public Vector3 iniPos;
    public GameObject getMousePosObj;
    GetMousePosSc getMousePosSc;

    public GameObject tableObj;
    private GameObject frontObj; //��O�̃I�u�W�F�N�g���擾

    [SerializeField] private bool onSushi = false; //�J�[�\���Ǝ��i���d�Ȃ��Ă�Ƃ�true
    [SerializeField]private bool sushiRay = false; //���i�h���b�O���}�E�X����ray���΂����ۂ�

    private bool preorder = false; //order��trueorfalse�����߂�̂Ɏg��
    [SerializeField] private bool order = false;

    [SerializeField] private GameObject sushiPos;

    Rigidbody2D rbody;
    [SerializeField] private float speed = 0f;

    new Renderer renderer;

    [SerializeField] private bool toRight = true; //�E�ɗ������i�Ȃ̂�
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

        //ScorePlus�����邽��
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

            #region ���i�h���b�O���̏���
            if (sushiRay)
            {

                if (hits != null)
                {
                    //ray�������������̂̒��Ƀe�[�u���������order=true(�����o���Əd�Ȃ��ĂĂ�����)�Ȃ����false
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

            #region ���i�̏�ɃJ�[�\�����悹�����Ƃ������痣�������̏���
            else if (!sushiRay)
            {
                Dictionary<GameObject, int> keyValuePairs = new Dictionary<GameObject, int>(); //GameObject��sortingLayer���i�[

                if (hits != null)
                {
                    //ray�������������̂̂����A���i�Ɛ����o���̂��̂�sortingLayer�𒲂ׂ�
                    foreach (RaycastHit2D hit in hits)
                    {
                        if (hit.collider.gameObject.tag == "BubbleNormal" || hit.collider.gameObject.tag == "Sushi")
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

                    //sortingOrder���ő�̂��̂����i�Ȃ�sushiRay=true
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

            #region �h���b�O�ňړ������鏈��
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
                if (order) //���i�Q�b�g
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
