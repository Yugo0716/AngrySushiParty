using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Bubble_test : MonoBehaviour
{
    private float time;

    public bool order = false;

    ItemController itemController;

    Dictionary<ItemTypeSc.ItemType, int> itemTypeAndNum = new Dictionary<ItemTypeSc.ItemType, int>()
    {
        {ItemTypeSc.ItemType.shoyu, 0 }, {ItemTypeSc.ItemType.gari, 1}, {ItemTypeSc.ItemType.wasabi, 2}, {ItemTypeSc.ItemType.yunomi, 3}
    };

    public List<Sprite> bubbleSprite = new List<Sprite>();
    public List<Sprite> cBubbleSprite = new List<Sprite>();

    ItemTypeSc itemTypeSc;

    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        itemTypeSc = GetComponent<ItemTypeSc>();

        if(gameObject!=null)
        transform.DOScale(new Vector3(1.5f, 1.5f, 1), 9.0f).SetEase(Ease.InSine).SetDelay(2.7f);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;      

        if (time > 16.0f)
        {
            Destroy(gameObject);
        }
        /*
        if (order)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = cBubbleSprite[itemTypeAndNum[itemTypeSc.type]];

        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = bubbleSprite[itemTypeAndNum[itemTypeSc.type]];

        }
        
        if (itemController.bubbleObj != null)
        {
            if (itemController.bubbleObj == gameObject)
            {
                if (itemController.order)
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = cBubbleSprite[itemTypeAndNum[itemTypeSc.type]];
                }
                else
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = bubbleSprite[itemTypeAndNum[itemTypeSc.type]];
                }
            }
        }*/
        
        
    }
}
