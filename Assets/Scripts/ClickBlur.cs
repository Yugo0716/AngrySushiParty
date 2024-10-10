using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ClickBlur : MonoBehaviour
{
    [SerializeField] Volume volume;
    DepthOfField depthOfField;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Blur()
    {
        if (volume.profile.TryGet<DepthOfField>(out depthOfField))
        {
            // 0.1‚©‚ç50‚É1•b‚Å•Ï‰»‚³‚¹‚é
            DOTween.To(() => depthOfField.focusDistance.value, x => depthOfField.focusDistance.value = x, 10f, 2f)
                .From(0.1f);
        }
    }
}
