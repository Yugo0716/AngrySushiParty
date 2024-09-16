using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScorePlusText : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ScorePlusAnime(int plusScore)
    {
        TextMeshProUGUI scorePlusText = gameObject.GetComponent<TextMeshProUGUI>();
        scorePlusText.text = "+" + plusScore.ToString();

        scorePlusText.transform.DOMoveY(1f, 0.5f).SetRelative(true);
        scorePlusText.DOFade(0, 0.5f).OnComplete(()=>
        {
            Destroy(gameObject);
        }
        );
    }
}
