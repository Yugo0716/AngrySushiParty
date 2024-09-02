using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class E_OrderSushiGenerator : MonoBehaviour
{
    public List<GameObject> sushiObjPoses = new List<GameObject>();
    public List<GameObject> bubbles = new List<GameObject>();

    TimeManager timeManager;

    [SerializeField] float orderInterbal;

    [SerializeField] private float time;

    [SerializeField] int orderCount = 0;
    [SerializeField] int orderChance = 0;

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

    //�V�K
    [SerializeField] GameObject orderSushiPos;
    int orderSushiNum = 2;

    [SerializeField] GameObject orderBubblePos;

    // Start is called before the first frame update
    void Start()
    {
        //gameState�擾�̂���
        GameObject canvas = GameObject.FindGameObjectWithTag("canvas");
        timeManager = canvas.GetComponent<TimeManager>();

        //���������邱�Ƃ�m�点��_��
        lampOn = GameObject.FindGameObjectWithTag("Announce").GetComponent<Image>();

        orderInterbal = 17f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeManager.gameState == TimeManager.GameState.play)
        {
            time += Time.deltaTime;

            //�������i�𓮂����C�����o����\�������� �O�̒������i���c���Ă�Ȃ疳��
            if (time > orderInterbal)
            {
                if (orderCount == 0 || (orderCount > 0 && sushiObjPoses[orderCount - 1] == null))
                {
                    lampOn.DOFade(1f, 0.5f).SetEase(Ease.InSine).SetLoops(6, LoopType.Yoyo);

                    StartCoroutine("StartOrder", orderCount);
                    lampOn.DOFade(0f, 0.5f).SetEase(Ease.InSine).SetDelay(3.0f);

                    orderCount++;
                }
                time = 0;
                orderChance++;

                if(orderChance > 3)
                {
                    if(orderChance % 3 == 1 && orderInterbal > 8)
                    {
                        orderInterbal -= 1.5f;
                        Debug.Log("orderPace Up!!!!");
                    }
                }
            }
        }
        else if (timeManager.gameState == TimeManager.GameState.end)
        {
            foreach (GameObject obj in sushiObjPoses)
            {
                if (obj != null && obj.activeSelf) obj.SetActive(false);
            }
        }
    }

    IEnumerator StartOrder(int orderCount)
    {
        
        GameObject orderSushiPosObj = Instantiate(orderSushiPos, this.transform.position + new Vector3(-14.25f, 1.03f,0f), transform.rotation, this.transform);        
        sushiObjPoses.Add(orderSushiPosObj);        

        GameObject orderBubbleObj = Instantiate(orderBubblePos, transform.position, transform.rotation, this.transform);
        bubbles.Add(orderBubbleObj);

        SetSushiImage(sushiObjPoses[orderCount], orderCount);

        yield return new WaitForSeconds(1.8f);

        sushiObjPoses[orderCount].transform.DOMove(new Vector3(20f, 0f, 0f), 3f).SetRelative().SetEase(Ease.OutSine);


        yield return new WaitForSeconds(3f);
        bubbles[orderCount].SetActive(true);

        //�����o����delTime�����Ƀ|�|�|�����Ċ����ɂł�悤�ɂ���
        List<int> bubbleCount = new List<int>();
        for (var i = 0; i < bubbles[orderCount].transform.childCount; i++)
        {
            bubbleCount.Add(i);
        }

        Shuffle(bubbleCount);

        int layer = 200;
        foreach (int i in bubbleCount)
        {
            GameObject childObj = bubbles[orderCount].transform.GetChild(i).gameObject;
            //float delTime = UnityEngine.Random.Range(0f, 0.2f);
            float delTime = 0.1f;
            yield return new WaitForSeconds(delTime);
            childObj.SetActive(true);
            childObj.GetComponent<Renderer>().sortingOrder = layer;
            childObj.transform.GetChild(0).gameObject.GetComponent<Canvas>().sortingOrder = layer + 1;
            layer += 10;
        }
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

    //�����o���̐ݒ�
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
            SushiTypeSc sushiTypeSc = childObj[i].GetComponent<SushiTypeSc>();
            sushiTypeSc.type = numAndSushiType[nums[i]];

            GameObject canvas = childObj[i].transform.GetChild(0).gameObject;

            canvas.GetComponent<Canvas>().sortingOrder = childObj[i].GetComponent<SpriteRenderer>().sortingOrder;

            sushiText = canvas.transform.GetChild(0).gameObject;
            giveText = canvas.transform.GetChild(1).gameObject;

            sushiText.GetComponent<TextMeshProUGUI>().text = numAndSushiName[nums[i]];
            giveText.GetComponent<TextMeshProUGUI>().text = giveTextList[UnityEngine.Random.Range(0, giveTextList.Count)];

            childObj[i].GetComponent<SpriteRenderer>().sprite = orderBubblesSprites[nums[i]];
        }
    }

    //�v�f����size�R�̃��X�g���烉���_����n�R�̗v�f�ԍ�����ꂽ���X�g��Ԃ�(start�̒l�𑫂������̂�Ԃ����Ƃ��ł���)
    public List<int> GetRandom(int size, int n, int start)
    {
        if (n > size)
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
