using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NormalSushi : SushiController
{
    [SerializeField] private float speed = 0f;

    [SerializeField] private bool toRight = true; //‰E‚É—¬‚ê‚éŽõŽi‚È‚Ì‚©

    public Sprite[] sushiSprites;

    GameObject sushiSpeedobj;
    SushiSpeed sushiSpeed;

    Dictionary<int, SushiTypeSc.SushiType> numAndSushiType = new Dictionary<int, SushiTypeSc.SushiType>()
    {
        {0, SushiTypeSc.SushiType.Tamago}, {1, SushiTypeSc.SushiType.Ebi}, {2, SushiTypeSc.SushiType.Ika}, {3, SushiTypeSc.SushiType.Maguro}
        ,{4, SushiTypeSc.SushiType.Ikura}
    };

    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
        speed = rbody.velocity.x;

        spriteRenderer.sprite = GetSushiSprite(sushiSprites);

        sushiSpeedobj = GameObject.FindGameObjectWithTag("SushiSpeed");
        if(sushiSpeedobj != null)
        {
            sushiSpeed = sushiSpeedobj.GetComponent<SushiSpeed>();
        }
        

        if (toRight)
        {
            if (speed != 3.0f) speed = 3.0f;
        }
        else
        {
            if (speed != -3.0f) speed = -3.0f;
        }
        rbody.velocity = new Vector2(speed, 0f);
    }

    // Update is called once per frame
    override public void Update()
    {
        base.Update();
    }

    private void FixedUpdate()
    {
        if(!gameMode.isScored)
        {
            speed = sushiSpeed.speed;

            if(!toRight)
            {
                speed = -speed;
            }
            rbody.velocity = new Vector2(speed, 0f);
        }
    }

    Sprite GetSushiSprite(Sprite[] sprites)
    {
        int rand = Random.Range(0, sprites.Length);
        sushiType.type = numAndSushiType[rand];
        return sprites[Random.Range(0, sprites.Length)];
    }
}
