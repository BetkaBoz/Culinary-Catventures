using System;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    private Rigidbody2D body;
    private bool isDisabled = false;
    private Transform challengePosition;
    private Vector3 newPosition;
    
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        challengePosition = GameObject.FindGameObjectWithTag("Challenge").transform;


    }

    private Vector3 lastClickedPos;
    private bool moving = false;


    private void Start()
    {
        newPosition = transform.position;

        
        //Time.timeScale = 1;
    }

    private void Update()
    {
        /*
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("MovePlayer");

            RaycastHit hit = default;
            if (!Camera.main)
            {
                Debug.Log("CAMERA NULL");
                return;   
            }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.Log("Ray"+ ray);
            Debug.Log("hit"+ hit.transform);

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Raycast");

                newPosition = hit.point;
                transform.position = newPosition;
            }
        }*/
        
        if(Input.GetMouseButtonDown(0))
        {
            lastClickedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lastClickedPos.z = transform.position.z;
            moving = true;
        }
        if(moving && transform.position != lastClickedPos)
        {
            float step = movementSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, lastClickedPos, step);
        } else {
            moving = false;
        }
    }
    
    
    private void FixedUpdate()
    {
        if(!isDisabled) MovePlayer();
        
       
    }
    
    //Hýbanie hráčom
    private void MovePlayer()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
        FlipSprite(inputX);
        body.velocity = new Vector2(inputX * movementSpeed, inputY * movementSpeed);
    }
    //Otáčanie spritu hráča podľa hýbania
    private void FlipSprite(float inputX)
    {
        if (inputX != 0 && inputX != transform.localScale.x)
        {
            transform.localScale = new Vector3(inputX * Math.Abs(transform.localScale.x) , transform.localScale.y, transform.localScale.z);
        }
    }
    
    //Nehýbanie hráča, keď ho berie ruka
    public void DragPlayer(float speed)
    {
        isDisabled = true;
        body.velocity = Vector2.zero;
        transform.position = Vector2.MoveTowards(transform.position, challengePosition.position, speed * Time.deltaTime);
    }
}

