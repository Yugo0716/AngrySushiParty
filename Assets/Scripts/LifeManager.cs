using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    public int life = 100;
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
