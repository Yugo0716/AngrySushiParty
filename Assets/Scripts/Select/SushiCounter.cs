using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SushiCounter : MonoBehaviour
{
    public int counter = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("count:"+counter);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag=="BackSushi")
        {
            counter++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BackSushi")
        {
            counter--;
        }
    }
}
