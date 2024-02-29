using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalSushi : SushiController
{
    [SerializeField] private float speed = 0f;

    [SerializeField] private bool toRight = true; //‰E‚É—¬‚ê‚éŽõŽi‚È‚Ì‚©
    bool speedCheck = false;

    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
        speed = rbody.velocity.x;

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
}
