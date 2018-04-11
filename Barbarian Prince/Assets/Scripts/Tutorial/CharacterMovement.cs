using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    // RigidBody component instance for the player
    private Rigidbody2D playerRigidBody2D;
    //Variable to track how much movement is needed from input
    private float movePlayerHorizontal;
    private float movePlayerVertical;
    private Vector2 movement;
    // Animator component for the player
    private Animator playerAnim;
    // Speed modifier for player movement
    public float speed = 4.0f;
    //Initialize any component references
    void Awake()
    {
        playerAnim = (Animator)GetComponent(typeof(Animator));
        playerRigidBody2D = (Rigidbody2D)GetComponent(typeof(Rigidbody2D));
    }
    // Update is called once per frame
    void Update()
    {
        movePlayerHorizontal = Input.GetAxis("Horizontal");
        movePlayerVertical = Input.GetAxis("Vertical");
        movement = new Vector2(movePlayerHorizontal, movePlayerVertical);
        playerRigidBody2D.velocity = movement * speed;
        playerAnim.SetBool("moving", false);
        if (movePlayerHorizontal > 0)
        {
            playerAnim.SetInteger("xMove", 1);
            playerAnim.SetBool("moving", true);
        }
        else if (movePlayerHorizontal < 0)
        {
            playerAnim.SetInteger("xMove", -1);
            playerAnim.SetBool("moving", true);
        }
        else
        {
            playerAnim.SetInteger("xMove", 0);
        }
        if (movePlayerVertical > 0)
        {
            playerAnim.SetInteger("yMove", 1);
            playerAnim.SetBool("moving", true);
        }
        else if (movePlayerVertical < 0)
        {
            playerAnim.SetInteger("yMove", -1);
            playerAnim.SetBool("moving", true);
        }
        else
        {
            playerAnim.SetInteger("yMove", 0);
        }
    }
}
