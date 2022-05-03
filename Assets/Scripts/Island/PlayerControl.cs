using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    private Rigidbody2D body;
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        
        Time.timeScale = 1;
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
        FlipSprite(inputX);
        body.velocity = new Vector2(inputX * movementSpeed, inputY * movementSpeed);
    }

    private void FlipSprite(float inputX)
    {
        if (inputX != 0 && inputX != transform.localScale.x)
        {
            transform.localScale = new Vector3(inputX * Math.Abs(transform.localScale.x) , transform.localScale.y, transform.localScale.z);
        }
    }
}

