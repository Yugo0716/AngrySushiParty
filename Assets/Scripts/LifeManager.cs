using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    public int life = 3;
    [SerializeField] GameObject lifeTextObj;
    TextMeshProUGUI lifeText;

    // Start is called before the first frame update
    void Start()
    {
        lifeText = lifeTextObj.GetComponent<TextMeshProUGUI>();
        UpdateLife();
    }

    // Update is called once per frame
    void Update()
    {
        if (life < 0) life = 0;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            life = 100;
            UpdateLife();
        }
    }

    public void LifeMinus()
    {
        if (life > 0) life--;
        UpdateLife();
    }

    void UpdateLife()
    {
        lifeText.text = "ÉâÉCÉtÅF" + life.ToString();
    }
}
