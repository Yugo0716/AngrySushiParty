using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderSushiGenerator : MonoBehaviour
{
    public GameObject[] sushiObjPoses;
    public GameObject[] bubbles;

    public GameObject timeManagerObj;
    TimeManager timeManager;

    [SerializeField] float[] orderIntervals = new float[] { 13, 15, 15 }; //�������i������Ԋu(���ƂŕύX����)

    [SerializeField] private float time;

    [SerializeField]int orderCount = 0;
    [SerializeField]int orderChance = 0;

    public Image orderAnnounce;


    // Start is called before the first frame update
    void Start()
    {
        //gameState�擾�̂���
        GameObject canvas = GameObject.FindGameObjectWithTag("canvas");
        timeManager = canvas.GetComponent<TimeManager>();

        //���������邱�Ƃ�m�点��_��
        orderAnnounce = GameObject.FindGameObjectWithTag("Announce").GetComponent<Image>();
        

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

            //�������i�𓮂����C�����o����\�������� �O�̒������i���c���Ă�Ȃ疳��
            if(orderChance < 3)
            {
                if(time > orderIntervals[orderCount])
                {
                    if (orderCount == 0 || (orderCount > 0 && sushiObjPoses[orderCount-1] == null))
                    {
                        orderAnnounce.DOFade(1f, 0.5f).SetEase(Ease.InCubic).SetLoops(6, LoopType.Yoyo);

                        StartCoroutine("StartOrder", orderCount);
                        orderAnnounce.DOFade(0f, 0.5f).SetEase(Ease.InCubic).SetDelay(3.0f);
                        
                        orderCount++;
                    }                    
                    time = 0;
                    orderChance++;
                }
            }
        }
        else if(timeManager.gameState == TimeManager.GameState.end)
        {
            foreach (GameObject obj in sushiObjPoses)
            {
                if(obj != null && obj.activeSelf) obj.SetActive(false);
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
        yield return new WaitForSeconds(1.8f);
        sushiObjPoses[orderCount].transform.DOMove(new Vector3(20f, 0f, 0f), 3f).SetRelative().SetEase(Ease.OutSine);

        yield return new WaitForSeconds(3f);

        bubbles[orderCount].SetActive(true);
    }
}
