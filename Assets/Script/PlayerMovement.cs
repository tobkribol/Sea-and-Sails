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
    [SerializeField] private float rotationSpeed = 10f;
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
    public ParticleSystem PS;


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
            rb.AddForce(transform.up * moveSpeed); //Physics based movement
            //https://answers.unity.com/questions/616195/how-to-make-an-object-go-the-direction-it-is-facin.html
            //https://github.com/vlytsus/unity-3d-boat/blob/cf17525846dd67147b21ee479b7e6e1572c27b92/Assets/Scenes/BoatForces.cs
        }


        //Rortation
        if (Input.GetKey(KeyCode.A))
        {
            Vector3 rotationToAdd = new Vector3(0, 0, rotationSpeed * Time.deltaTime);
            transform.Rotate(rotationToAdd);
            //transform.rotation = Quaternion.Euler(new Vector3(0, 0, transform.rotation.z + rotationSpeed));
            Debug.Log(transform.rotation.z);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Vector3 rotationToAdd = new Vector3(0, 0, -rotationSpeed * Time.deltaTime);
            transform.Rotate(rotationToAdd);
            Debug.Log(transform.rotation.z);
        }
    }
    private void FireCannon()
    {
        int numberOfGunsSide = numberOfGuns / 2;
        switch (ammoType)
        {
            case 0:
                //Canonball
                if (!alreadyShooting && (ammoCannonball > numberOfGunsSide))
                {
                    FireCannonSide(numberOfGunsSide);
                    ammoCannonball -= numberOfGunsSide;
                    ammoText[ammoType].text = "Cannonball: " + ammoCannonball;

                }
                else if (!alreadyShooting && (ammoCannonball > 0))
                {
                    FireCannonSide(ammoCannonball);
                    ammoCannonball -= ammoCannonball;
                    ammoText[ammoType].text = "Cannonball: " + ammoCannonball;
                }
                break;

            case 1:
                //Hotshot
                if (!alreadyShooting && (ammoHotshot > numberOfGunsSide))
                {
                    FireCannonSide(numberOfGunsSide);
                    ammoHotshot -= numberOfGunsSide;
                    ammoText[ammoType].text = "Hotshot: " + ammoHotshot;
                }
                else if (!alreadyShooting && (ammoHotshot > 0))
                {
                    FireCannonSide(ammoHotshot);
                    ammoHotshot -= ammoHotshot;
                    ammoText[ammoType].text = "Hotshot: " + ammoHotshot;
                }
                break;
        }
    }
    private void ShootCannonRight(int numberOfRounds)
    {
        
        for (int i = 0; i < numberOfRounds; i++)
        {
            Vector2 v = LaunchOffsetRight2.position - LaunchOffsetRight1.position; //find vector between two sides
            Vector2 launchOffsetPositon = (Vector2)LaunchOffsetRight1.position + (Random.value * v); //pick random position along ship side

            Instantiate(ProjectilePrefab[ammoType], launchOffsetPositon, transform.rotation);
            Instantiate(PS, launchOffsetPositon, transform.rotation);
        }

        Invoke(nameof(ResetAttack), timeBetweenAttack);
    }
    private void ShootCannonLeft(int numberOfRounds)
    {

        for (int i = 0; i < numberOfRounds; i++) //Weapons.numberOfGuns
        {

            Vector2 v = LaunchOffsetLeft2.position - LaunchOffsetLeft1.position; //find vector between two sides
            Vector2 launchOffsetPositon = (Vector2)LaunchOffsetLeft1.position + (Random.value * v); //pick random position along ship side

            Instantiate(ProjectilePrefab[ammoType], launchOffsetPositon, transform.rotation);
            Instantiate(PS, launchOffsetPositon, transform.rotation);
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
    }
    private void FireCannonSide(int numberOfRounds)
    {
        alreadyShooting = true;
        switch (shootSide)
        {
            case true:
                ShootCannonRight(numberOfRounds);
                break;
            case false:
                ShootCannonLeft(numberOfRounds);
                break;
        }
    }
}
