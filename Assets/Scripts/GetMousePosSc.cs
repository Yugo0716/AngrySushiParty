using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GetMousePosSc : MonoBehaviour
{
     [SerializeField] new Camera camera;

    public Vector3 GetMousePos()
    {
        return camera.ScreenToWorldPoint(Input.mousePosition);
    }

    public RaycastHit2D[] GetRaycastHit()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(GetMousePos(), Vector3.forward);
        
        return hits;
    }
    
    //tags�Ɋ܂܂��^�O�̂Ȃ��ōł���O�̃I�u�W�F�N�g��Ԃ�
    public GameObject GetFrontObj(RaycastHit2D[] hits, string[] tags)
    {
        GameObject frontObj = null;

        Dictionary<GameObject, int> keyValuePairs = new Dictionary<GameObject, int>(); //GameObject��sortingLayer���i�[
        

        if (hits != null)
        {
            //ray�������������̂̂����Atags�Ɋ܂܂�Ă�����̂�sortingLayer�𒲂ׂ�
            foreach (RaycastHit2D hit in hits)
            {
                for(int i = 0; i < tags.Length; i++)
                {
                    if(hit.collider.gameObject.tag == tags[i])
                    {
                        Renderer renderer = hit.collider.gameObject.GetComponent<Renderer>();
                        keyValuePairs.Add(hit.collider.gameObject, renderer.sortingOrder);
                        break;
                    }
                }               
            }

            //��������1���q�b�g���Ȃ��Ƃ� �������₵��
            if (keyValuePairs.Count <= 0)
            {
                if (frontObj != null) frontObj = null;
            }
            else
            {
                frontObj = keyValuePairs.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
            }            
        }       
        return frontObj;
    }
}
