using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate bool MovementDetection(float horizontal, float vertical);

[RequireComponent(typeof(Animator))]
public class Rpg_2d_controller : MonoBehaviour
{
    private bool isAttacking = false;
    private bool isWalking = false;

    [SerializeField]
    private bool fourDirectionsOnly = false;

    private GameObject holdingItem = null;
    private List<GameObject> objectsinRange = new List<GameObject>();

    private Vector3 direction = Vector3.down;

    [SerializeField]
    private float speed = 5f;

    [Range(0, 1)]
    [SerializeField]
    private float range = 0.25f;

    private float horizontal = 0;
    private float vertical = 0;
    private Animator animation;

    [SerializeField]
    private GameObject attack_range_area;

    // Use this for initialization
    private void Awake()
    {
        animation = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        //Inputs
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        isAttacking = Input.GetButtonDown("Fire2");
        isWalking = (horizontal == 0 && vertical == 0) ? false : true;

        //Attack
        if (isAttacking && !holdingItem)
        {
            Attack(1);
        }

        //Pick Up Item
        if (Input.GetButtonDown("Fire1"))
        {
            if (!holdingItem)
            {
                PickUpItem();
            }
            else
            {
                PutDownItem();
            }
        }
    }

    private void FixedUpdate()
    {
        //Walking
        if (isWalking && !animation.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            CalculateDirection(fourDirectionsOnly);
            //transform.Translate(horizontal * speed, vertical * speed, 0);
            //makes sure that if you move diagonally you will not move faster than if you move straight
            transform.Translate((Math.Abs(direction.x) == Math.Abs(direction.y)) ? direction * (speed * 0.75f) : direction * speed);
            if (holdingItem)
            {
                HoldingItem();
            }
        }

        attack_range_area.transform.position = new Vector3(transform.position.x + (animation.GetFloat("Horizontal") * range), transform.position.y + (animation.GetFloat("Vertical") * range), transform.position.z);
    }

    private void LateUpdate()
    {
        //Animation

        //walking
        animation.SetBool("Walking", isWalking);

        if (isWalking && !animation.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            animation.SetFloat("Horizontal", direction.x);
            animation.SetFloat("Vertical", direction.y);
        }

        //Attack

        //Pickup/down
    }

    private void CalculateDirection(bool fourDirectionsOnly)
    {
        int xaxis = 0;
        int yaxis = 0;
        if (fourDirectionsOnly)
        {
            if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
                xaxis = horizontal > 0 ? 1 : -1;
            else
                yaxis = vertical > 0 ? 1 : -1;
        }
        else
        {
            if (horizontal != 0)
            {
                xaxis = horizontal > 0 ? 1 : -1;
            }
            if (vertical != 0)
            {
                yaxis = vertical > 0 ? 1 : -1;
            }
        }
        direction = new Vector3(1f * xaxis, 1f * yaxis, 0f);
    }

    private GameObject ObjectInRange(string tag)
    {
        if (objectsinRange.Count > 0)
        {
            for (int obj = 0; obj < objectsinRange.Count; obj++)
            {
                if (objectsinRange[obj].tag == tag)
                {
                    return objectsinRange[obj];
                }
            }
        }
        return null;
    }

    private void PickUpItem()
    {
        holdingItem = ObjectInRange("Pickup_item");
        //see if it found a item in range
        if (holdingItem)
        {
            animation.SetTrigger("PickUpItem");
            holdingItem.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
            holdingItem.GetComponent<SpriteRenderer>().sortingOrder = 5;
        }
    }

    private void HoldingItem()
    {
        holdingItem.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }

    private void PutDownItem()
    {
        animation.SetTrigger("PickUpItem");
        if (animation.GetFloat("Vertical") == 0)
        {
            holdingItem.transform.position = new Vector3(transform.position.x + (animation.GetFloat("Horizontal") * 0.8f), transform.position.y - 0.5f, transform.position.z);
        }
        else
        {
            holdingItem.transform.position = new Vector3(transform.position.x + (animation.GetFloat("Horizontal") * 0.8f), transform.position.y + (animation.GetFloat("Vertical") * 0.8f), transform.position.z);
        }
        holdingItem.GetComponent<SpriteRenderer>().sortingOrder = 0;
        holdingItem = null;
    }

    private void Attack(int damage)
    {
        animation.SetTrigger("Attack");

        for (int obj = 0; obj < objectsinRange.Count; obj++)
        {
            if (objectsinRange[obj].tag == "Enemy")
            {
                objectsinRange[obj].GetComponent<Enemy_2d_controller>().TakenDamage(damage);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!objectsinRange.Contains(other.gameObject) && other.gameObject != this.gameObject && other.gameObject.tag != "Static_obj")
        {
            objectsinRange.Add(other.gameObject);
            Debug.Log(other.gameObject + "added to list");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (objectsinRange.Contains(other.gameObject) && other.gameObject != this.gameObject && other.gameObject.tag != "Static_obj")
        {
            objectsinRange.Remove(other.gameObject);
            Debug.Log(other.gameObject + "removed from list");
        }
    }
}