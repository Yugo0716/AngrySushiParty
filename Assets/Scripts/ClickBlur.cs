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
            // 0.1から50に1秒で変化させる
            DOTween.To(() => depthOfField.focusDistance.value, x => depthOfField.focusDistance.value = x, 10f, 2f)
                .From(0.1f);
        }
    }

    public void UnBlur()
    {
        if (volume.profile.TryGet<DepthOfField>(out depthOfField))
        {
            // 50から0.1に1秒で変化させる
            DOTween.To(() => depthOfField.focusDistance.value, x => depthOfField.focusDistance.value = x, 0.1f, 2f)
                .From(10f);
        }
    }
}
