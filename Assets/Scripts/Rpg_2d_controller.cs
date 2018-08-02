using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate bool MovementDetection(float horizontal, float vertical);

[RequireComponent(typeof(Animator))]
public class Rpg_2d_controller : MonoBehaviour
{
    private bool isAttacking = false;
    private bool isWalking = false;
    private List<GameObject> ObjectsinRange = new List<GameObject>();

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
        //isAttacking = Input.GetKeyDown(KeyCode.K);
        isWalking = (horizontal == 0 && vertical == 0) ? false : true;

        //Idles
        //animation.SetBool("Walking", isWalking);
        //Walking
        //if (isWalking && !animation.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        //{
        //    animation.SetFloat("Horizontal", horizontal);
        //    animation.SetFloat("Vertical", vertical);
        //    //transform.position = Move(horizontal, vertical);
        //    transform.Translate(horizontal * speed * Time.deltaTime, vertical * speed * Time.deltaTime, 0);
        //}

        //Attack
        if (Input.GetKeyDown(KeyCode.K))
        {
            Attack(1);
        }

        //Pick Up Item
        if (Input.GetKeyDown(KeyCode.J))
        {
            PickUpItem();
        }
        //Put Down Item
        if (Input.GetKeyDown(KeyCode.L))
        {
            PutDownItem();
        }
    }

    private void FixedUpdate()
    {
        //Walking
        if (isWalking && !animation.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            transform.Translate(horizontal * speed, vertical * speed, 0);
        }
        attack_range_area.transform.position = new Vector3(transform.position.x + (animation.GetFloat("Horizontal") * range), transform.position.y + (animation.GetFloat("Vertical") * range), transform.position.z);
    }

    private void LateUpdate()
    {
        animation.SetBool("Walking", isWalking);

        if (isWalking && !animation.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            animation.SetFloat("Horizontal", horizontal);
            animation.SetFloat("Vertical", vertical);
        }
    }

    private Vector3 Move(float horizontalInput, float verticalInput)
    {
        Vector3 newPosition = transform.position;
        newPosition.x += horizontalInput * speed * Time.deltaTime;
        newPosition.y += verticalInput * speed * Time.deltaTime;
        return newPosition;
    }

    private void Walk(bool walking, Animator animation)
    {
    }

    private void PickUpItem()
    {
        animation.SetTrigger("PickUpItem");
    }

    private void PutDownItem()
    {
        animation.SetTrigger("PutDownItem");
    }

    private void Attack(int damage)
    {
        //attack_range_area.transform.position = new Vector3(transform.position.x + (animation.GetFloat("Horizontal") * range), transform.position.y + (animation.GetFloat("Vertical") * range), transform.position.z);
        animation.SetTrigger("Attack");

        for (int obj = 0; obj < ObjectsinRange.Count; obj++)
        {
            if (ObjectsinRange[obj].tag == "Enemy")
            {
                ObjectsinRange[obj].GetComponent<Enemy_2d_controller>().TakenDamage(damage);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!ObjectsinRange.Contains(other.gameObject) && other.gameObject != this.gameObject && other.gameObject.tag != "Static_obj")
        {
            ObjectsinRange.Add(other.gameObject);
            Debug.Log(other.gameObject + "added to list");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (ObjectsinRange.Contains(other.gameObject) && other.gameObject != this.gameObject && other.gameObject.tag != "Static_obj")
        {
            ObjectsinRange.Remove(other.gameObject);
            Debug.Log(other.gameObject + "removed to list");
        }
    }
}