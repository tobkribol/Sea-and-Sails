using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    //"Ctrl + M, O" to collapse.

    Weapons weapons;
    SailRotation sr;
    [SerializeField] Transform [] shipSails;

    [Header("Ship stats")]
    [HideInInspector] public float moveSpeed;
    [SerializeField] private float maxSpeed = 1.5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] public float timeBetweenAttack = 0.15f;
    [SerializeField] public float timeBetweenAttackMax = 1.20f;
    [SerializeField] public float timeBetweenAttackMin = 0.05f;


    [Header("Ship Inventory")]
    [SerializeField] public int numberOfGuns = 24;

    [SerializeField] public int ammoCannonball = 200;
    [SerializeField] float cannonballDamage = 5f;

    [SerializeField] public int ammoHotshot = 200;
    [SerializeField] float hotshotDamage = 15f;

    [SerializeField] public float GunFireDisance = 4.00f;
    [SerializeField] public float shootForce = 4f;
    [SerializeField] private ProjectileBehaviour[] ProjectilePrefab;
    public float currentAmmoType;
    private int CurrentItemInt;
    private int numberOfCycles = 1;


    [Header("Inputdata")]
    [SerializeField] private Transform [] LaunchOffsetRight;
    [SerializeField] private Transform[] LaunchOffsetLeft;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform shipRotateAxis;
    [SerializeField] private Transform shipSailMid;


    [Header("Canvas Data")]
    [SerializeField] private Text shipSpeedText; //need UnityEngine.UI
    [SerializeField] private Text shipSailAngleText; //need UnityEngine.UI
    [SerializeField] private Text[] ammoText; //need UnityEngine.UI
    [HideInInspector] public bool shootSide = true;
    public int ammoType = 0;

    bool alreadyShootingLeft = false;
    bool alreadyShootingRight = false;
    float inputMagnitude;
    [HideInInspector] public Vector2 moveDirection;
    public ParticleSystem PS;

    float shipCurrentSpeed;


    private void Start()
    {
        ammoText[0].text = "Cannonball: " + ammoCannonball;
        ammoText[1].text = "Hotshot: " + ammoHotshot;
        sr = shipSails[0].GetComponent<SailRotation>();
        currentAmmoType = cannonballDamage;
        timeBetweenAttack = timeBetweenAttackMax;

    }
    void Update()
    {
        ProcessInputs();
        ItemCycleList();

        shipSpeedText.text = string.Format("Ship Speed: {0:F1} m/s", rb.velocity.magnitude*10, 1);
        shipSailAngleText.text = string.Format("Sail angle: {0:F0} °", sr.GetSailWindAngle(), 1);
    }
    private void FixedUpdate()
    {
        Move();

        if (Input.GetButton("Fire1"))

        {
            FireCannon();

            //Increase fire rate while holding "fire1" down
            if (timeBetweenAttack > timeBetweenAttackMin)
            {
                Invoke(nameof(TimeBetweenAttackIncrease), 0.5f);
            }

        }

        else if (Input.GetButtonUp("Fire1"))
        {
            timeBetweenAttack = timeBetweenAttackMax;
        }
        

    }
    void ProcessInputs()
    {
        //Move input
        if (Input.GetKeyDown(KeyCode.W) && moveSpeed < 75)
        {
            moveSpeed += 25;
        }

        if (Input.GetKeyDown(KeyCode.S) && moveSpeed > 0)
        {
            moveSpeed -= 25;
        }

        //Weapon
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ammoType = 0;
            currentAmmoType = cannonballDamage;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ammoType = 1;
            currentAmmoType = hotshotDamage;
        }


        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchCannonSide();
        }

    }
    private void Move()
    {

        if (rb.velocity.magnitude < maxSpeed)
        {
            shipCurrentSpeed = moveSpeed * sr.GetWindSpeedBoost();
            rb.AddForce(transform.up * shipCurrentSpeed); //Physics based movement
            //Debug.Log("GetWindSpeedBoost: " + sr.GetWindSpeedBoost() + "| GetSailWindAngle: " + sr.GetSailWindAngle() + "| GetWindSpeed: " + sr.GetWindSpeed());
            //https://answers.unity.com/questions/616195/how-to-make-an-object-go-the-direction-it-is-facin.html
            //https://github.com/vlytsus/unity-3d-boat/blob/cf17525846dd67147b21ee479b7e6e1572c27b92/Assets/Scenes/BoatForces.cs
        }


        //Rotation
        if (Input.GetKey(KeyCode.A))
        {
            Vector3 rotationToAdd = new Vector3(0, 0, rotationSpeed * Time.deltaTime);
            transform.Rotate(rotationToAdd);
            //transform.rotation = Quaternion.Euler(new Vector3(0, 0, transform.rotation.z + rotationSpeed));
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Vector3 rotationToAdd = new Vector3(0, 0, -rotationSpeed * Time.deltaTime);
            transform.Rotate(rotationToAdd);
        }
    }

    private void TimeBetweenAttackIncrease()
    {
        timeBetweenAttack -= 0.001f;
        Debug.Log("TimeBetweenAttack: " + timeBetweenAttack);
    }

    private void FireCannon()
    {
        int numberOfGunsSide = numberOfGuns / 2;

        switch (ammoType)
        {
            case 0:
                //Canonball
                //Full broadside fire
                if (ammoCannonball > numberOfGunsSide)
                {
                    FireCannonSide(numberOfGunsSide, ammoType);


                }
                //In case of less than full broadside use remaining ammo
                else if (ammoCannonball > 0)
                {
                    FireCannonSide(ammoCannonball, ammoType);
                }
                break;

            case 1:
                //Hotshot
                if (ammoHotshot > numberOfGunsSide)
                {
                    FireCannonSide(numberOfGunsSide, ammoType);
                }
                else if (ammoHotshot > 0)
                {
                    FireCannonSide(ammoHotshot, ammoType);
                }
                break;
        }
    }
    private void ShootCannon(int numberOfRounds, Transform position1, Transform position2)
    {
        //should watch for object pooling https://www.youtube.com/watch?v=uxm4a0QnQ9E
        float rotationfactor = 1;
        if (shootSide)
        {
            rotationfactor = -1;
        }
        else
        {
            rotationfactor = 1;
        }

        switch (CurrentItemInt)
        {
            case 0:
                for (int i = 0; i < numberOfRounds; i++)
                {
                    Vector2 v = position2.position - position1.position; //find vector between two sides
                    Vector2 launchOffsetPositon = (Vector2)position1.position + (Random.value * v); //pick random position along ship side

                    Instantiate(ProjectilePrefab[ammoType], launchOffsetPositon, transform.rotation);
                    Instantiate(PS, launchOffsetPositon, transform.rotation * Quaternion.Euler(new Vector3(0, 0, rotationfactor * 90f))); //Quaternion must be multiplied (as vectors) https://answers.unity.com/questions/1353333/how-to-add-2-quaternions.html
                }
                break;

            case 1:

                for (int i = 0; i < numberOfRounds; i++)
                {
                    Vector2 v = position2.position - position1.position; //find vector between two sides
                    Vector2 fireLineStep = ((1 / (float)numberOfRounds)) * (float)i * v ;
                    Vector2 launchOffsetPositon = (Vector2)position1.position + fireLineStep; //pick random position along ship side
                    Instantiate(ProjectilePrefab[ammoType], launchOffsetPositon, transform.rotation);
                    Instantiate(PS, launchOffsetPositon, transform.rotation * Quaternion.Euler(new Vector3(0, 0, rotationfactor * 90f))); //Quaternion must be multiplied (as vectors) https://answers.unity.com/questions/1353333/how-to-add-2-quaternions.html
                }
                break;
        }
        //Invoke(nameof(ResetAttackRight), Random.Range(-0.02f,0.02f) + timeBetweenAttack); //Create some randomness to the fire mechanism
    }
    public void ResetAttackLeft()
    {
        alreadyShootingLeft = false;
    }
    public void ResetAttackRight()
    {
        alreadyShootingRight = false;
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
    private void FireCannonSide(int numberOfRounds, int ammoType)
    {
        string ammoTextType = "";
        if (ammoType == 0)
        {
            ammoTextType = "Cannonball: ";
        }
        else if (ammoType == 1)
        {
            ammoTextType = "Hotshot: ";
        }

        switch (shootSide)
        {
            case true:
                if (alreadyShootingRight == false)
                {
                    alreadyShootingRight = true;
                    ShootCannon(numberOfRounds, LaunchOffsetRight[0], LaunchOffsetRight[1]);
                    ammoText[ammoType].text = ammoTextType + ReduceAmmo(numberOfRounds);
                    Invoke(nameof(ResetAttackRight), Random.Range(-0.02f, 0.02f) + timeBetweenAttack);
                }
                break;

            case false:
                if (alreadyShootingLeft == false)
                {
                    alreadyShootingLeft = true;
                    ShootCannon(numberOfRounds, LaunchOffsetLeft[0], LaunchOffsetLeft[1]);
                    ammoText[ammoType].text = ammoTextType + ReduceAmmo(numberOfRounds);
                    Invoke(nameof(ResetAttackLeft), Random.Range(-0.02f, 0.02f) + timeBetweenAttack);
                }
                break;
        }
    }
    private int ReduceAmmo(int numberOfRounds)
    {
        if (ammoType == 0)
        {
            return ammoCannonball -= numberOfRounds;
        }
        else if (ammoType == 1)
        {
            return ammoHotshot -= numberOfRounds;
        }
        else return 0;
    }
    private void ItemCycleList()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (CurrentItemInt >= numberOfCycles)
            {
                CurrentItemInt = 0;
            }
            else
            {
                CurrentItemInt += 1;
            }
            Debug.Log("CurrentItemInt: " + CurrentItemInt);
        }
        //CurrentItemObject = Items[CurrentItemInt];
    }

}

