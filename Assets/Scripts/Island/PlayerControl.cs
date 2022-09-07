using System;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    private Rigidbody2D body;
    private bool isDisabled;
    private Transform challengePosition;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        challengePosition = GameObject.FindGameObjectWithTag("Challenge").transform;
    }

    private void FixedUpdate()
    {
        if (!isDisabled && !EventManager.IsInEvent)
        {
            //Debug.Log(EventManager.IsInEvent);
            //Debug.Log(isDisabled);

            MovePlayer();

        } 
    }

    //HÝBANIE HRÁČOM
    private void MovePlayer()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
        FlipSprite(inputX);
        body.velocity = new Vector2(inputX * movementSpeed, inputY * movementSpeed);
    }
    //OTÁČANIE SPRITU HRÁČA PODĽA HÝBANIA
    private void FlipSprite(float inputX)
    {
        if (inputX != 0 && inputX != transform.localScale.x)
        {
            var localScale = transform.localScale;
            localScale = new Vector3(inputX * Math.Abs(localScale.x), localScale.y, localScale.z);
            transform.localScale = localScale;
        }
    }

    /*
    private Vector3 lastClickedPos;
    private bool moving;
    
    private void MovePlayerByMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastClickedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lastClickedPos.z = transform.position.z;
            moving = true;
        }
        if (moving && transform.position != lastClickedPos)
        {
            float step = movementSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, lastClickedPos, step);
        }
        else
        {
            moving = false;
        }
    }*/

    //NEHÝBANIE HRÁČA, KEĎ HO BERIE RUKA
    public void DragPlayer(float speed)
    {
        isDisabled = true;
        body.velocity = Vector2.zero;
        transform.position = Vector2.MoveTowards(transform.position, challengePosition.position, speed * Time.deltaTime);
    }

    public void ReleasePlayer()
    {
        isDisabled = false;
    }
}
