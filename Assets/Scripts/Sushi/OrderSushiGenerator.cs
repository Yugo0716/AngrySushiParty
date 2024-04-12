using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
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

    public Image lampOn;

    public List<Sprite> sushiSprites = new List<Sprite>();

    public List<Sprite> orderBubblesSprites = new List<Sprite>();
    public List<Sprite> cOrderBubblesSprites = new List<Sprite>();


    Dictionary<int, SushiTypeSc.SushiType> numAndSushiType = new Dictionary<int, SushiTypeSc.SushiType>()
    {
        {0, SushiTypeSc.SushiType.Tamago}, {1, SushiTypeSc.SushiType.Ebi}, {2, SushiTypeSc.SushiType.Ika}, {3, SushiTypeSc.SushiType.Maguro}
        ,{4, SushiTypeSc.SushiType.Ikura}
    };

    Dictionary<int, string> numAndSushiName = new Dictionary<int, string>()
    {
        {0, "���܂�"}, {1, "�G�r"}, {2, "�C�J"}, {3, "�}�O��"}, {4, "�C�N��"}
    };

    GameObject sushiText;
    GameObject giveText;

    List<string> giveTextList = new List<string> { "�����~", "��������", "����~", "���傤����", "�ق�����~" };

    // Start is called before the first frame update
    void Start()
    {
        //gameState�擾�̂���
        GameObject canvas = GameObject.FindGameObjectWithTag("canvas");
        timeManager = canvas.GetComponent<TimeManager>();

        //���������邱�Ƃ�m�点��_��
        lampOn = GameObject.FindGameObjectWithTag("Announce").GetComponent<Image>();
        

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
                        lampOn.DOFade(1f, 0.5f).SetEase(Ease.InCubic).SetLoops(6, LoopType.Yoyo);

                        StartCoroutine("StartOrder", orderCount);
                        lampOn.DOFade(0f, 0.5f).SetEase(Ease.InCubic).SetDelay(3.0f);
                        
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
        SetSushiImage(sushiObjPoses[orderCount], orderCount);

        yield return new WaitForSeconds(1.8f);
        sushiObjPoses[orderCount].transform.DOMove(new Vector3(20f, 0f, 0f), 3f).SetRelative().SetEase(Ease.OutSine);

        yield return new WaitForSeconds(3f);

        bubbles[orderCount].SetActive(true);
    }

    //���i�̃X�v���C�g��SushiType�̎w��
    void SetSushiImage(GameObject sushiPos, int orderCount)
    {
        int sushiCount = sushiPos.transform.childCount;

        // �q�I�u�W�F�N�g���i�[����z��쐬
        GameObject[] childObj = new GameObject[sushiCount];

        //�q�I�u�W�F�N�g��z��Ɋi�[
        for (var i = 0; i < childObj.Length; ++i)
        {
            childObj[i] = sushiPos.transform.GetChild(i).gameObject;
        }

        //0~3�̒����璍�����i�̐����������_���ɐ����𓾂�
        List<int> nums = GetRandom(numAndSushiName.Count, childObj.Length, 0);

        //��ł������l�ɑΉ�����X�v���C�g�ɂ���@�����sushitype���w��
        for (int i = 0; i < childObj.Length; ++i)
        {
            childObj[i].GetComponent<SpriteRenderer>().sprite = sushiSprites[nums[i]];

            SushiTypeSc sushiTypeSc = childObj[i].GetComponent<SushiTypeSc>();
            sushiTypeSc.type = numAndSushiType[nums[i]];
        }

        SetBubbleType(bubbles[orderCount], nums);
    }

    void SetBubbleType(GameObject bubblePos, List<int> nums)
    {
        int bubbleCount = bubblePos.transform.childCount;

        // �q�I�u�W�F�N�g���i�[����z��쐬
        GameObject[] childObj = new GameObject[bubbleCount];

        //�q�I�u�W�F�N�g��z��Ɋi�[
        for (var i = 0; i < childObj.Length; ++i)
        {
            childObj[i] = bubblePos.transform.GetChild(i).gameObject;
        }

        //nums���V���b�t��(�ǂ̐����o���ɂǂ̒������i�����邩�������_���ɂ��邽��)
        Shuffle(nums);

        //��ł������l�ɑΉ�����X�v���C�g�ɂ���@�����sushitype���w��
        for (int i = 0; i < childObj.Length; ++i)
        {
            //childObj[i].GetComponent<SpriteRenderer>().sprite = sushiSprites[nums[i]];

            SushiTypeSc sushiTypeSc = childObj[i].GetComponent<SushiTypeSc>();
            sushiTypeSc.type = numAndSushiType[nums[i]];

            GameObject canvas = childObj[i].transform.GetChild(0).gameObject;

            canvas.GetComponent<Canvas>().sortingOrder = childObj[i].GetComponent<SpriteRenderer>().sortingOrder;

            sushiText = canvas.transform.GetChild(0).gameObject;
            giveText = canvas.transform.GetChild(1).gameObject;
            //sushiText.transform.position = childObj[i].transform.position;
            sushiText.GetComponent<TextMeshProUGUI>().text = numAndSushiName[nums[i]];
            giveText.GetComponent<TextMeshProUGUI>().text = giveTextList[UnityEngine.Random.Range(0, giveTextList.Count)];

            childObj[i].GetComponent<SpriteRenderer>().sprite = orderBubblesSprites[nums[i]];
        }
    }

    //�v�f����size�R�̃��X�g���烉���_����n�R�̗v�f�ԍ�����ꂽ���X�g��Ԃ�(start�̒l�𑫂������̂�Ԃ����Ƃ��ł���)
    public List<int> GetRandom(int size, int n, int start)
    {
        if(n > size)
        {
            throw new ArgumentOutOfRangeException("���X�g�̗v�f�����n���傫���ł��B");
        }
        var indexList = new List<int>(size);
        var returnList = new List<int>(n);

        for (int p = 0; p < size; p++) indexList.Add(p);

        for (int i = 0; i < n; i++)
        {
            int index = UnityEngine.Random.Range(0, indexList.Count);
            int value = indexList[index];
            indexList.RemoveAt(index);
            returnList.Add(value + start);
        }
        return returnList;
    }

    void Shuffle(List<int> array)
    {
        for (var i = array.Count - 1; i > 0; --i)
        {
            // 0�ȏ�i�ȉ��̃����_���Ȑ������擾
            // Random.Range�̍ő�l�͑�Q���������Ȃ̂ŁA+1���邱�Ƃɒ���
            var j = UnityEngine.Random.Range(0, i + 1);

            // i�Ԗڂ�j�Ԗڂ̗v�f����������
            var tmp = array[i];
            array[i] = array[j];
            array[j] = tmp;
        }
    }
}
