using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2d_controller : MonoBehaviour
{
    [SerializeField]
    private int health = 1;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void TakenDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}