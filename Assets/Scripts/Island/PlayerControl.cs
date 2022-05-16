using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    private Rigidbody2D body;
    private bool isDisabled = false;
    private Transform challengePosition;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        
        challengePosition = GameObject.FindGameObjectsWithTag("Challenge")[0].transform;

        Time.timeScale = 1;
    }

    void FixedUpdate()
    {
        if(!isDisabled) MovePlayer();
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

    public void DragPlayer(float speed)
    {
        isDisabled = true;
        body.velocity = Vector2.zero;
        transform.position = Vector2.MoveTowards(transform.position, challengePosition.position, speed * Time.deltaTime);
    }
}

