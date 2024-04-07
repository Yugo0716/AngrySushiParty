using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NormalSushi : SushiController
{
    [SerializeField] private float speed = 0f;

    [SerializeField] private bool toRight = true; //‰E‚É—¬‚ê‚éŽõŽi‚È‚Ì‚©
    bool speedCheck = false;

    public Sprite[] sushiSprites;

    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
        speed = rbody.velocity.x;

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = GetSushiSprite(sushiSprites);

    }

    // Update is called once per frame
    override public void Update()
    {
        base.Update();
    }

    private void FixedUpdate()
    {
        if (toRight)
        {
            if (speed != 3.0f) speed = 3.0f;
        }
        else
        {
            if (speed != -3.0f) speed = -3.0f;
        }

        if (!speedCheck)
        {
            rbody.velocity = new Vector2(speed, 0f);
            speedCheck = true;
        }
    }

    Sprite GetSushiSprite(Sprite[] sprites)
    {
        return sprites[Random.Range(0, sprites.Length)];
    }
}
