using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMousePosSc : MonoBehaviour
{
     [SerializeField] new Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 GetMousePos()
    {
        return camera.ScreenToWorldPoint(Input.mousePosition);
    }
}
