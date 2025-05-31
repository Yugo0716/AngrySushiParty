using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    static public BGMManager instance;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {

            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {

            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
