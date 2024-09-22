using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeManager : MonoBehaviour
{
    public static bool isFadeInstance = false;
    GameObject fadeObj;
    Image fadeImage;

    // Start is called before the first frame update
    void Start()
    {
        if (!isFadeInstance)//�N����
        {
            DontDestroyOnLoad(this);
            isFadeInstance = true;
        }
        else//�N�����ȊO�͏d�����Ȃ��悤�ɂ���
        {
            Destroy(this);
        }

        fadeObj = transform.GetChild(0).gameObject;
        fadeImage = fadeObj.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeIn()
    {
        fadeImage.DOFade(1, 0.3f);
    }

    public void FadeOut()
    {
        fadeImage.DOFade(0, 0.3f);
    }
}
