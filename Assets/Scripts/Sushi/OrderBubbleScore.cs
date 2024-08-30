using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderBubbleScore : MonoBehaviour
{
    float time = 0;
    public float bonusScore = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        {
            time += Time.deltaTime;
            if (time <= 1f)
            {
                bonusScore = 300;
            }
            else if(time <= 11f)
            {
                bonusScore = Mathf.Ceil(300 - (time-1) * 30);
            }
            else
            {
                bonusScore = 0;
            }
        }
    }
}
