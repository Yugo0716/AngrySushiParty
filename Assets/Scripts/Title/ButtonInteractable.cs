using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInteractable : MonoBehaviour
{
    [SerializeField] List<Button> buttons = new List<Button>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IntearctableFalse()
    {
        foreach (var button in buttons)
        {
            if (button.interactable)
            {
                button.interactable = false;
            }
        }
    }
}
