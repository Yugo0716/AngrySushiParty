using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BackSushiGenerator : MonoBehaviour
{
    public GameObject sushiObj;

    [SerializeField] private float delTime;
    [SerializeField] private float interval;
    [SerializeField] bool omomi;
    public bool isGo = true;


    [SerializeField] GameObject generateBalancerObj;
    BackGenerateBalancer generateBalancer;

    // Start is called before the first frame update
    void Start()
    {
        isGo = true;
        if (generateBalancerObj != null)
        {
            generateBalancer = generateBalancerObj.GetComponent<BackGenerateBalancer>();
        }

        interval = 2f;


        delTime = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        delTime += Time.deltaTime;

        if (delTime >= interval && isGo)
        {
            //2‚Â‚ÌGenerator‚Ì‚Ç‚¿‚ç‚©ˆê•û‚©‚ç‚Ì‚İ‚µ‚©1“x‚É—¬‚³‚È‚¢‚Ì‚Å‚»‚ê‚ğ§Œä‚³‚¹‚éŠÖ”‚ğŒÄ‚ÔD(2‰ñŒÄ‚ñ‚¾‚çˆÓ–¡‚È‚¢)
            if (omomi) generateBalancer.SelectAndGenerate();
            delTime = 0;
        }
    }

    public IEnumerator Generate()
    {
        float time = 0;
        time = UnityEngine.Random.Range(0, 0.3f);

        yield return new WaitForSeconds(time);
        Instantiate(sushiObj, transform.position, sushiObj.transform.rotation);
    }
}