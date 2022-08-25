using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{

    //class Input
    
    Weapons weapons;


    // Variable player input
    [Header("Ship stats")]
    [HideInInspector] public float moveSpeed = 0.0f;
    [SerializeField] private float maxSpeed = 1.5f;
    [SerializeField] private float acceleration = 1f;
    [SerializeField] private float deacceleration = 0.9f;
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] public float timeBetweenAttack = 0.15f;


    //Inventory
    [Header("Ship Inventory")]
    [SerializeField] public int numberOfGuns = 24;
    [SerializeField] public int ammoCannonball = 200;
    [SerializeField] public int ammoHotshot = 200;
    [SerializeField] private ProjectileBehaviour[] ProjectilePrefab;

    //Standard  
    [Header("Inputdata")]
    public Transform LaunchOffsetRight1;
    public Transform LaunchOffsetRight2;
    public Transform LaunchOffsetLeft1;
    public Transform LaunchOffsetLeft2;
    [SerializeField] private Rigidbody2D rb;


    [Header("Canvas Data")]
    [SerializeField] private Text[] ammoText; //need UnityEngine.UI
    [HideInInspector] public bool shootSide = true;
    private int ammoType = 0;

    bool alreadyShooting = false;
    float inputMagnitude;
    [HideInInspector] public Vector2 moveDirection;


    private void Start()
    {
        ammoText[0].text = "Cannonball: " + ammoCannonball;
        ammoText[1].text = "Hotshot: " + ammoHotshot;
    }
    void Update()
    {
        ProcessInputs();
    }

    private void FixedUpdate()
    {
        Move();


        if (Input.GetButton("Fire1"))

        {
            FireCannon();
        }
    }

    void ProcessInputs()
    {
        //Move input
        if (Input.GetKeyDown(KeyCode.W) && moveSpeed < 1000)
        {
            moveSpeed += 50;
        }

        if (Input.GetKeyDown(KeyCode.S) && moveSpeed > 0)
        {
            moveSpeed -= 50;
        }

        ////movement
        //float moveX = Input.GetAxis("Horizontal");
        //float moveY = Input.GetAxis("Vertical");
        //moveDirection = new Vector2(moveX, moveY);
        //inputMagnitude = Mathf.Clamp01(moveDirection.magnitude);
        //moveDirection.Normalize();

        //Weapon
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ammoType = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ammoType = 1;
        }


        if (Input.GetKeyDown(KeyCode.E))
        {
            SwitchCannonSide();
        }

    }

    private void Move()
    {

        if (rb.velocity.magnitude < maxSpeed)
        {
            rb.AddRelativeForce(transform.up * moveSpeed); //Physics based movement
            //https://answers.unity.com/questions/616195/how-to-make-an-object-go-the-direction-it-is-facin.html
        }


        //Rortation
        if (Input.GetKey(KeyCode.A))
        {
            Vector3 rotationToAdd = new Vector3(0, 0, rotationSpeed);
            transform.Rotate(rotationToAdd);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Vector3 rotationToAdd = new Vector3(0, 0, -rotationSpeed);
            transform.Rotate(rotationToAdd);
        }


        //Old rotation
        //if (moveDirection != Vector2.zero)
        //{
        //    Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, moveDirection);

        //    transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        //}
    }

    private void FireCannon()
    {
        if (!alreadyShooting && (ammoCannonball > 0 && ammoType == 0))
        {
            alreadyShooting = true;

            switch (shootSide)
            {
                case true:
                    ShootCannonRight();
                    break;
                case false:
                    ShootCannonLeft();
                    break;
            }

            ammoCannonball -= numberOfGuns / 2;
            ammoText[ammoType].text = "Cannonball: " + ammoCannonball;

        }
        else if (!alreadyShooting && (ammoHotshot > 0 && ammoType == 1))
        {
            alreadyShooting = true;

            switch (shootSide)
            {
                case true:
                    ShootCannonRight();
                    break;
                case false:
                    ShootCannonLeft();
                    break;
            }

            ammoHotshot -= numberOfGuns / 2;
            ammoText[ammoType].text = "Hotshot: " + ammoHotshot;

        }
    }
    private void ShootCannonRight()
    {
        
        for (int i = 0; i < numberOfGuns; i++)
        {
            Vector2 v = LaunchOffsetRight2.position - LaunchOffsetRight1.position; //find vector between two sides
            Vector2 launchOffsetPositon = (Vector2)LaunchOffsetRight1.position + (Random.value * v); //pick random position along ship side

            Instantiate(ProjectilePrefab[ammoType], launchOffsetPositon, transform.rotation);
        }

        Invoke(nameof(ResetAttack), timeBetweenAttack);
    }

    private void ShootCannonLeft()
    {

        for (int i = 0; i < numberOfGuns; i++) //Weapons.numberOfGuns
        {

            Vector2 v = LaunchOffsetLeft2.position - LaunchOffsetLeft1.position; //find vector between two sides
            Vector2 launchOffsetPositon = (Vector2)LaunchOffsetLeft1.position + (Random.value * v); //pick random position along ship side

            Instantiate(ProjectilePrefab[ammoType], launchOffsetPositon, transform.rotation);
        }

        Invoke(nameof(ResetAttack), timeBetweenAttack);
    }

    public void ResetAttack()
    {
        alreadyShooting = false;
    }

    public void SwitchCannonSide()
    {
        if (shootSide == true)
        {
            shootSide = false;
        }
        else
        {
            shootSide = true;
        }
        Debug.Log("Shootside: " + shootSide);
    }
}


7
