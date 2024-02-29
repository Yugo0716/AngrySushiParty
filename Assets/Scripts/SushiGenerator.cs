using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SushiGenerator : MonoBehaviour
{
    public GameObject sushiObj;
    public GameObject timeManagerObj;
    TimeManager timeManager;

    [SerializeField]private float time;
    [SerializeField]int totalTime = 0;
    [SerializeField] private float interval = 1.0f;

    [SerializeField] public bool omomi = true;

    [SerializeField][Tooltip("���i�̘g��")]int maxCount;
    [SerializeField][Tooltip("�������i�̐�")]int sushiCount = 35;

    List<bool> goSushi = new List<bool>();
    
    List<int> indexList = new List<int>();

    //[SerializeField] AnimationCurve curve;

    //float[] probs = new float[] { 20f, 30f, 50f }; //���ՁA���ՁA�I�Ղ̊m���̏d��
    int[] nums = new int[] { 0, 0, 0 }; //���ՁA���ՁA�I�Ղɔz�u������i�̐�
    int[] maxNums = new int[] { 19, 19, 19 }; //���ՁA���ՁA�I�Ղɔz�u�ł�����i�̘g��(19�~3)


    // Start is called before the first frame update
    void Start()
    {
        //gameState�擾�̂���
        GameObject canvas = GameObject.FindGameObjectWithTag("canvas");
        timeManager = canvas.GetComponent<TimeManager>();

        maxCount = timeManager.maxTime-3;

        /*//���ՁA���ՁA�I�Ղɔz�u������i�̐����m��
        for (int i = 0; i < sushiCount; i++)
        {
            int sushiId = Choose(probs);
            nums[sushiId] += 1;
        }*/

        //��U���i�𗬂����ۂ���false�ŏ����� �o�O�΍�ł�����Ƒ傫�߂ɃT�C�Y�����
        for (int i = 0; i < maxCount + 5; i++)
        {
            goSushi.Add(false);
        }

        if (omomi)
        {
            nums[0] = 7;
            nums[1] = 12;
            nums[2] = 16;

            //���Ւ��ՏI�Ղ��ꂼ��̎��i�̔z�u�ꏊ���m�肷��
            List<int> earlyIndexList = GetRandom(maxNums[0], nums[0], 0);
            List<int> middleIndexList = GetRandom(maxNums[1], nums[1], maxNums[0]);
            List<int> lastIndexList = GetRandom(maxNums[2], nums[2], maxNums[0] + maxNums[1]);

            indexList = Unit3List(earlyIndexList, middleIndexList, lastIndexList);
        }

        else
        {
            indexList = GetRandom(maxCount, sushiCount, 0);
        }               
        
        foreach (int i in indexList)
        {
            goSushi[i] = true;
        }

        time = 0f;

        
        
        /*//animationcurve�ŏd�݂Â�����悤�@��������
        for(int i = 0; i < 30; i++)
        {
            tests[i] = (int)(CurveWeightedRandom(curve) * 56);
        }

        foreach(int i in tests) Debug.Log(tests[i]);
        
        test = (int)(CurveWeightedRandom(curve) * 56);
        Debug.Log(test);*/
    }

    // Update is called once per frame
    void Update()
    {
        if(timeManager.gameState == TimeManager.GameState.play)
        {           
            time += Time.deltaTime;

            if (time >= interval)
            {
                if (goSushi[totalTime]) Generate(sushiObj);
                time = 0;
                totalTime++;
                Debug.Log(totalTime);
            }            
        }
    }

    void Generate(GameObject generateObj)
    {
        Instantiate(generateObj, transform.position, generateObj.transform.rotation);
    }

    float CurveWeightedRandom(AnimationCurve curve)
    {
        return curve.Evaluate(UnityEngine.Random.value);
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

    public List<int> Unit3List(List<int> listA, List<int> listB, List<int> listC)
    {
        var list = (listA.Concat(listB).ToList()).Concat(listC).ToList();

        return list;
    }

    int Choose(float[] probs)
    {

        float total = 0;

        foreach (float elem in probs)
        {
            total += elem;
        }

        float randomPoint = UnityEngine.Random.value * total;

        for (int i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
            {
                return i;
            }
            else
            {
                randomPoint -= probs[i];
            }
        }
        return probs.Length - 1;
    }
}