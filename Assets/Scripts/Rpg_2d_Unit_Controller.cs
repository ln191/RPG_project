using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Rpg_2d_Unit_Controller : MonoBehaviour
{
    protected Animator animation;

    [SerializeField]
    protected int health = 1;

    [SerializeField]
    private int maxHealth = 5;

    protected List<GameObject> objectsinRange = new List<GameObject>();

    protected Vector3 direction = Vector3.down;

    [SerializeField]
    protected float speed = 0.1f;

    [Range(0, 1)]
    [SerializeField]
    protected float range = 0.25f;

    [SerializeField]
    protected GameObject attack_range_area;

    public int Health
    {
        get
        {
            return health;
        }

        set
        {
            health = value;
        }
    }

    public int MaxHealth
    {
        get
        {
            return maxHealth;
        }
    }

    // Use this for initialization
    protected virtual void Awake()
    {
        animation = GetComponent<Animator>();

        if (health > maxHealth)
        {
            health = maxHealth;
        }
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

    public virtual void TakenDamage(int damage)
    {
        health -= damage;
        animation.SetTrigger("TokeDamage");
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
                objectsinRange[obj].GetComponent<Rpg_2d_Unit_Controller>().TakenDamage(damage);
            }
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (!objectsinRange.Contains(other.gameObject) && other.gameObject.tag != this.gameObject.tag && other.gameObject.tag != "Static_obj" && other.gameObject.tag != "Trigger")
        {
            objectsinRange.Add(other.gameObject);
            Debug.Log(other.gameObject + "added to " + this.gameObject + "´s list");
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (objectsinRange.Contains(other.gameObject) && other.gameObject.tag != this.gameObject.tag && other.gameObject.tag != "Static_obj" && other.gameObject.tag != "Trigger")
        {
            objectsinRange.Remove(other.gameObject);
            Debug.Log(other.gameObject + "removed from" + this.gameObject + "´s list");
        }
    }
}