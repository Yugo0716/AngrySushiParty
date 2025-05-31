using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.soundManager.PlayBGM(BGMType.Title);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
