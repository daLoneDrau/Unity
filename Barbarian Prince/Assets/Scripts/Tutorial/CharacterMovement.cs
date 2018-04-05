using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {
    // RigidBody component instance for the player
    private Rigidbody2D playerRigidBody2D;
    //Variable to track how much movement is needed from input
    private float movePlayerHorizontal;
    private float movePlayerVertical;
    private Vector2 movement;
    // Speed modifier for player movement
    public float speed = 4.0f;
    //Initialize any component references
    void Awake()
    {
        playerRigidBody2D = (Rigidbody2D)GetComponent(typeof(Rigidbody2D));
    }
    // Update is called once per frame
    void Update()
    {
        movePlayerHorizontal = Input.GetAxis("Horizontal");
        movePlayerVertical = Input.GetAxis("Vertical");
        movement
        = new Vector2(movePlayerHorizontal, movePlayerVertical);
        playerRigidBody2D.velocity = movement * speed;
    }
}
