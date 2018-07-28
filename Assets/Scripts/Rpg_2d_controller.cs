using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Rpg_2d_controller : MonoBehaviour
{
    private bool isAttacking = false;

    [SerializeField]
    private float speed = 5f;

    private float horizontal = 0;
    private float vertical = 0;
    private Animator animation;

    // Use this for initialization
    private void Awake()
    {
        animation = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (horizontal == 0 && vertical == 0)
        {
            animation.SetBool("Walking", false);
        }
        else
        {
            animation.SetBool("Walking", true);
            animation.SetFloat("Horizontal", horizontal);
            animation.SetFloat("Vertical", vertical);
            transform.position = Move(horizontal, vertical);
        }
    }

    private Vector3 Move(float horizontalInput, float verticalInput)
    {
        Vector3 newPosition = transform.position;
        newPosition.x += horizontalInput * speed * Time.deltaTime;
        newPosition.y += verticalInput * speed * Time.deltaTime;
        return newPosition;
    }
}