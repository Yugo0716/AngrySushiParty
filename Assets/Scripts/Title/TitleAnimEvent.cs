using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleAnimEvent : MonoBehaviour
{
    [SerializeField] GameObject panelObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UIActive()
    {
        Destroy(panelObj);
        int childCount = gameObject.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            Transform childTransform = gameObject.transform.GetChild(i);
            GameObject childObject = childTransform.gameObject;
            
            childObject.SetActive(true);
        }
    }
}
