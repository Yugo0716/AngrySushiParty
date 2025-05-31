using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_OrderSushiChild : MonoBehaviour
{
    int sushiNum=2;
    // Start is called before the first frame update
    void Start()
    {
        if (sushiNum == 1)
        {
            transform.GetChild(0).gameObject.transform.localPosition = new Vector2(0, 0);
            Destroy(transform.GetChild(1).gameObject);
        }
        if (sushiNum == 2)
        {
            transform.GetChild(0).gameObject.transform.localPosition = new Vector2(-2, 0);
            transform.GetChild(1).gameObject.transform.localPosition = new Vector2(2, 0);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
