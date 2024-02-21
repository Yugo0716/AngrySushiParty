using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Bubble_test : MonoBehaviour
{
    private float time;


    // Start is called before the first frame update
    void Start()
    {
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time > 2.7f && transform.localScale.x < 8)
        {
            transform.localScale *= 1.0006f;
        }

        if(time > 16.0f)
        {
            Destroy(gameObject);
        }
        
    }
}
