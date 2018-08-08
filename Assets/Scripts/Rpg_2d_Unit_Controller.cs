using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Rpg_2d_Unit_Controller : MonoBehaviour
{
    protected Animator animation;

    [SerializeField]
    protected int health = 1;

    protected List<GameObject> objectsinRange = new List<GameObject>();

    protected Vector3 direction = Vector3.down;

    [SerializeField]
    protected float speed = 0.1f;

    [Range(0, 1)]
    [SerializeField]
    protected float range = 0.25f;

    // Use this for initialization
    protected virtual void Awake()
    {
        animation = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
    }

    protected virtual void LateUpdate()
    {
    }

    protected virtual void FixedUpdate()
    {
    }

    public void TakenDamage(int damage)
    {
        health -= damage;
        animation.SetTrigger("tokeDamage");
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    protected void Attack(int damage, string tagToAtk)
    {
        animation.SetTrigger("Attack");

        for (int obj = 0; obj < objectsinRange.Count; obj++)
        {
            if (objectsinRange[obj].tag == tagToAtk)
            {
                objectsinRange[obj].GetComponent<Enemy_2d_controller>().TakenDamage(damage);
            }
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (!objectsinRange.Contains(other.gameObject) && other.gameObject.tag != this.gameObject.tag && other.gameObject.tag != "Static_obj")
        {
            objectsinRange.Add(other.gameObject);
            Debug.Log(other.gameObject + "added to list");
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (objectsinRange.Contains(other.gameObject) && other.gameObject.tag != this.gameObject.tag && other.gameObject.tag != "Static_obj")
        {
            objectsinRange.Remove(other.gameObject);
            Debug.Log(other.gameObject + "removed from list");
        }
    }
}