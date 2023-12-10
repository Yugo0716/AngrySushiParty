using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Regenerator : MonoBehaviour
{
    //public GameObject itemObj;
    ItemController itemControllerSc;

    // Start is called before the first frame update
    void Start()
    {
        //itemControllerSc = itemObj.GetComponent<ItemController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //アイテムを消して初期位置に再生成
    public IEnumerator Regenerate(GameObject itemObj)
    {
        itemControllerSc = itemObj.GetComponent<ItemController>();

        itemObj.SetActive(false);
        itemObj.transform.position = itemControllerSc.iniPos;

        yield return new WaitForSeconds(2.0f);

        itemObj.SetActive(true);
    }

}
