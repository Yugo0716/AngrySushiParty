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

    public void ScorePlusAnime(int plusScore, bool isMax)
    {
        TextMeshProUGUI scorePlusText = gameObject.GetComponent<TextMeshProUGUI>();

        if (isMax)
        {
            scorePlusText.color = new Color(0.745283f, 0.6941177f, 0, 1);
        }
        else
        {

        }
        scorePlusText.text = "+" + plusScore.ToString();

        scorePlusText.transform.DOMoveY(1f, 0.5f).SetRelative(true);
        scorePlusText.DOFade(0, 0.3f).SetDelay(0.2f).OnComplete(()=>
        {
            Destroy(gameObject);
        }
        );
    }
}
