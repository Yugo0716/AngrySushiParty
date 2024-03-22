using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderSushiGenerator : MonoBehaviour
{
    public GameObject[] sushiObjPoses;
    public GameObject[] bubbles;

    public GameObject timeManagerObj;
    TimeManager timeManager;

    [SerializeField] float[] orderIntervals = new float[] { 15, 15, 15 }; //�������i������Ԋu(���ƂŕύX����)

    [SerializeField] private float time;

    int orderCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        //gameState�擾�̂���
        GameObject canvas = GameObject.FindGameObjectWithTag("canvas");
        timeManager = canvas.GetComponent<TimeManager>();

        //�����̎��i�Ɛ����o���̑g�������Ă��邩�`�F�b�N
        CheckPairs();

        foreach (GameObject obj in bubbles)
        {
            obj.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(timeManager.gameState == TimeManager.GameState.play)
        {
            time += Time.deltaTime;

            //�������i�𓮂����C�����o����\��������
            if(orderCount < 3)
            {
                if(time > orderIntervals[orderCount])
                {
                    StartCoroutine("StartOrder", orderCount);

                    time = 0;
                    orderCount++;
                }   
            }
        }
    }

    void CheckPairs()
    {
        if(sushiObjPoses.Length != bubbles.Length)
        {
            Debug.LogError("�������i�Ɛ����o���̑g�������܂���");
            return;
        }
        else
        {
            for (int i = 0; i < sushiObjPoses.Length; i++)
            {
                if (sushiObjPoses[i].transform.childCount != bubbles[i].transform.childCount)
                {
                    Debug.LogError("����1�Z�b�g�̎��i�Ɛ����o���̌��������܂���");
                    return;
                }
            }
        }
    }

    IEnumerator StartOrder(int orderCount)
    {
        sushiObjPoses[orderCount].transform.DOMove(new Vector3(20f, 0f, 0f), 3f).SetRelative().SetEase(Ease.OutSine);

        yield return new WaitForSeconds(3f);

        bubbles[orderCount].SetActive(true);
    }
}
