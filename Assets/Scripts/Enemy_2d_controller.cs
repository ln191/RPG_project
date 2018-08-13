using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2d_controller : Rpg_2d_Unit_Controller
{
    private bool atk = true;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        if (objectsinRange.Count > 0 && atk)
        {
            Attack(1, "Player");
            atk = false;
        }

        base.Update();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }

    protected override void FixedUpdate()
    {
        //transform.Translate(Vector3.down * speed);

        base.FixedUpdate();
    }
}