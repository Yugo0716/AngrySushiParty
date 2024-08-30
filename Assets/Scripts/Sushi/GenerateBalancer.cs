using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBalancer : MonoBehaviour
{
    [SerializeField] GameObject generatorObjA;
    [SerializeField] GameObject generatorObjB;

    SushiGenerator sushiGeneratorA;
    SushiGenerator sushiGeneratorB;

    bool isGeneratorA = false;

    // Start is called before the first frame update
    void Start()
    {
        sushiGeneratorA = generatorObjA.GetComponent<SushiGenerator>();
        sushiGeneratorB = generatorObjB.GetComponent<SushiGenerator>();        
    }

    private void Update()
    {
        
    }

    public void SelectAndGenerate()
    {
        isGeneratorA = GetRandBool(50);
        if (isGeneratorA)
        {
            StartCoroutine(sushiGeneratorA.Generate());
        }
        else
        {
            StartCoroutine(sushiGeneratorB.Generate());
        }
    }

    private bool GetRandBool(int rate)
    {
        int num = UnityEngine.Random.Range(0, 100);

        if (num %2==0)
        {
            return true;
        }
        return false;
    }
}
