using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_OrderBubblePos : MonoBehaviour
{
    int sushiNum = 2;

    private void Awake()
    {
        gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        if (sushiNum == 1)
        {
            transform.GetChild(0).gameObject.transform.position = new Vector2(-4.61f/2f, -2.28f/2f);
            Destroy(transform.GetChild(1).gameObject);
        }
        if (sushiNum == 2)
        {
            transform.GetChild(0).gameObject.transform.position = new Vector2(-7.63f/2f, -4.05f / 2f);
            transform.GetChild(1).gameObject.transform.position = new Vector2(7.93f/2f, -1.58f/2f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
