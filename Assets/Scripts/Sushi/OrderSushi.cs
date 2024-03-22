using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Profiling;
using System.Linq;

public class OrderSushi : SushiController
{
    GameObject bubbleObj;

    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    override public void Update()
    {
        base.Update();
    }

    //�܂���肭�����Ă��邩������
    public override void SushiDrag(RaycastHit2D[] hits)
    {
        if (hits != null)
        {
            string[] tags = { "BubbleOrder" , "BubbleNormal"};

            bubbleObj = getMousePosSc.GetFrontObj(hits, tags);

            //bubbleObj�ƃA�C�e���̎�ނ���v���Ă�Ȃ�Ή��\(order=true)
            if (bubbleObj != null && bubbleObj.tag == "BubbleOrder")
            {
                SushiTypeSc bubbleType = bubbleObj.GetComponent<SushiTypeSc>();
                SushiTypeSc sushiType = gameObject.GetComponent<SushiTypeSc>();

                if (bubbleType.type == sushiType.type)
                {
                    if (!order) order = true;
                    destroyObj = bubbleObj;
                }
                else
                {
                    if (order) order = false;
                    destroyObj = null;
                }
            }
        }
    }
    public override void GetScore()
    {
        gameManager.ScorePlus(GameManager.ScoreType.bubbleOrder);
    }
}
