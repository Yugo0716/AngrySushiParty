using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class BackSushiController : MonoBehaviour
{
    Rigidbody2D rbody;
    float time = 0;

    [SerializeField] private float speed = 0f;

    [SerializeField] private bool toRight = true; //‰E‚É—¬‚ê‚éŽõŽi‚È‚Ì‚©

    public Sprite[] sushiSprites;

    SpriteRenderer spriteRenderer;
    //public List<Sprite> normalSushiSprites = new List<Sprite>();

    
    Dictionary<int, SushiTypeSc.SushiType> numAndSushiType = new Dictionary<int, SushiTypeSc.SushiType>()
    {
        {0, SushiTypeSc.SushiType.Tamago}, {1, SushiTypeSc.SushiType.Ebi}, {2, SushiTypeSc.SushiType.Ika}, {3, SushiTypeSc.SushiType.Maguro}
        ,{4, SushiTypeSc.SushiType.Ikura}
    };

    SushiTypeSc sushiType;

    // Start is called before the first frame update
    void Start()
    {
        GameObject parent = GameObject.FindGameObjectWithTag("BackSushiParent");
        gameObject.transform.SetParent(parent.transform);
        time = 0f;
        rbody = GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        sushiType = GetComponent<SushiTypeSc>();

        spriteRenderer.sprite = GetSushiSprite(sushiSprites);

        if (toRight)
        {
            if (speed != 3.2f) speed = 3.2f;
        }
        else
        {
            if (speed != -3.2f) speed = -3.2f;
        }
        rbody.velocity = new Vector2(speed, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        time += Time.deltaTime;

        if(time > 6f)
        {
            Destroy(gameObject);
        }
    }

    Sprite GetSushiSprite(Sprite[] sprites)
    {
        int rand = UnityEngine.Random.Range(0, sprites.Length);
        sushiType.type = numAndSushiType[rand];
        return sprites[UnityEngine.Random.Range(0, sprites.Length)];
    }
}
