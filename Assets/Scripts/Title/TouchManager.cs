using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    public static bool isTouch = false;
    bool canControl = true;
    bool wasTwoFingerTouching = false;
    [SerializeField] GameObject touchTextObj;

    // Start is called before the first frame update
    void Start()
    {
        if(isTouch)
        {
            touchTextObj.SetActive(true);
        }
        else
        {
            touchTextObj.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool isTwoFingerTouching = Input.touchCount == 2;
        bool isTwoFingerTapped = isTwoFingerTouching && !wasTwoFingerTouching;
        wasTwoFingerTouching = isTwoFingerTouching;

        // �uT�L�[�v�܂��́u2�{�w��V���ɒu�����u�ԁv
        if ((Input.GetKeyDown(KeyCode.T) || isTwoFingerTapped) && canControl)
        {
            isTouch = !isTouch;

            touchTextObj.SetActive(isTouch);
            if (isTouch)
            {
                SoundManager.soundManager.SEPlay(SEType.SushiClick);
                touchTextObj.SetActive(true) ;
            }
            else
            {
                SoundManager.soundManager.SEPlay(SEType.LifeMinus);
                touchTextObj.SetActive(false) ;
            }

            canControl = false;
        }
        else if (!Input.GetKey(KeyCode.T) && !isTwoFingerTouching)
        {
            // T�L�[��2�{�w�^�b�`�����Ă��Ȃ���ԂɂȂ�����A�ăg���K�[����
            canControl = true;
        }

    }
}
