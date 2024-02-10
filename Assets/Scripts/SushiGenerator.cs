using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SushiGenerator : MonoBehaviour
{
    public GameObject sushiObj;
    private float time;
    [SerializeField] private float interval = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        time = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time >= interval)
        {
            Generate(sushiObj);
            time = 0;
        }
    }

    void Generate(GameObject generateObj)
    {
        Instantiate(generateObj, transform.position, generateObj.transform.rotation);
    }
}
