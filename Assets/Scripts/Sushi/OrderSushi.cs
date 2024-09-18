using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Profiling;
using System.Linq;

public class OrderSushi : SushiController
{
    GameObject bubbleObj;

    Dictionary<SushiTypeSc.SushiType, int> sushiTypeAndNum = new Dictionary<SushiTypeSc.SushiType, int>()
    {
        {SushiTypeSc.SushiType.Tamago, 0 }, {SushiTypeSc.SushiType.Ebi, 1}, {SushiTypeSc.SushiType.Ika, 2}, {SushiTypeSc.SushiType.Maguro, 3}
        , {SushiTypeSc.SushiType.Ikura, 4}
    };

    public Sprite defaultSprite;

    public List<Sprite> bubbleSprite = new List<Sprite>();
    public List<Sprite> cBubbleSprite = new List<Sprite>();

    public GameObject preFrontObj = null;
    int bonusScore = 0;
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

    //まだ上手くいっているか怪しい
    public override void SushiDrag(RaycastHit2D[] hits)
    {
        if (hits != null)
        {
            string[] tags = { "BubbleOrder" , "BubbleNormal"};

            bubbleObj = getMousePosSc.GetFrontObj(hits, tags);

            //bubbleObjとアイテムの種類が一致してるなら対応可能(order=true)
            if (bubbleObj != null && bubbleObj.tag == "BubbleOrder")
            {
                SushiTypeSc bubbleType = bubbleObj.GetComponent<SushiTypeSc>();
                SushiTypeSc sushiType = gameObject.GetComponent<SushiTypeSc>();

                int sushiNum = sushiTypeAndNum[bubbleType.type];
                defaultSprite = bubbleSprite[sushiNum];

                if (bubbleType.type == sushiType.type)
                {
                    if (!order) order = true;
                    destroyObj = bubbleObj;
                    bubbleObj.GetComponent<SpriteRenderer>().sprite = cBubbleSprite[sushiNum];
                    bonusScore = (int)bubbleObj.GetComponent<OrderBubbleScore>().bonusScore;
                }
                else
                {
                    if (order) order = false;
                    destroyObj = null;
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
    public override void GetScore()
    {
        scoreManager.ScorePlus(ScoreManager.ScoreType.bubbleOrder, bonusScore);

        GameObject scorePlusCanvas = GameObject.FindGameObjectWithTag("ScorePlusCanvas");
        if (gameMode.isScored)
        {
            GameObject scorePlusTextObj2 = Instantiate(scorePlusTextObj);
            scorePlusTextObj2.transform.SetParent(scorePlusCanvas.transform, false);
            scorePlusTextObj2.transform.position = gameObject.transform.position;

            ScorePlusText scorePlusText = scorePlusTextObj2.GetComponent<ScorePlusText>();

            bool isMax = false;
            if (bonusScore == 300) isMax = true;
            scorePlusText.ScorePlusAnime(scoreManager.baseScore[ScoreManager.ScoreType.bubbleOrder]+bonusScore, isMax);
        }
    }
}
