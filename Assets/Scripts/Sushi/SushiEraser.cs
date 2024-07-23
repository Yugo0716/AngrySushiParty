using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SushiEraser : MonoBehaviour
{
    GameObject canvas;
    //�X�R�A����郂�[�h�����Ȃ���
    GameMode gameMode;
    private bool isScored;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("canvas");

        //GameMode�擾�̂���
        gameMode = canvas.GetComponent<GameMode>();
        isScored = gameMode.isScored;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Sushi")
        {
            SushiController sushiController = collision.gameObject.GetComponent<SushiController>();

            //���i���h���b�O������Ȃ���
            if (!sushiController.sushiRay)
            {
                Destroy(collision.gameObject.transform.root.gameObject);

                //�G���h���X���[�h�Ȃ�c�@-1
                if (!isScored)
                {
                    canvas.GetComponent<LifeManager>().LifeMinus();
                }
            }            
        }
    }
}
