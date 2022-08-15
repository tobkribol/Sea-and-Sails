using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    // Variable player input
    [SerializeField] public float moveSpeed = 0f;
    [SerializeField] private float maxSpeed = 1.5f;
    [SerializeField] private float acceleration = 1000f;
    [SerializeField] private float deacceleration = 0.9f;
    [SerializeField] private float rotationSpeed = 600f;
    [SerializeField] public float timeBetweenAttack = 0.2f;

    [SerializeField] private Rigidbody2D rb;

    public ProjectileBehaviour ProjectilePrefab;
    public Transform LaunchOffset;

    public GameObject LaunchOffsetRight;

    bool alreadyShooting = false;

    float inputMagnitude;


    private Vector2 moveDirection;

    // Update is called once per frame
    void Update()
    {
        //process input
        ProcessInputs();

    }

    private void FixedUpdate()
    {
        //physics Calclulations
        Move();

        if (Input.GetButton("Fire1") && !alreadyShooting)
        {
            //SailHelthBarFunction.SetHealthBarValue(SailHelthBarFunction.GetHealthBarValue() - 0.25f);
            alreadyShooting = true;
            ShootCannonRight();
        }
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");


        moveDirection = new Vector2(moveX, moveY);

        inputMagnitude = Mathf.Clamp01(moveDirection.magnitude);
        moveDirection.Normalize();
    }

    private void Move()
    {

       
        transform.Translate(moveDirection * moveSpeed * inputMagnitude * Time.deltaTime, Space.World);

        //Physics based movement https://answers.unity.com/questions/616195/how-to-make-an-object-go-the-direction-it-is-facin.html
        if (Input.GetButton("Horizontal"))
        {
            moveSpeed += acceleration;
            Debug.Log("Acceleration+\\" + moveSpeed + "moveDirection: " + moveDirection + "inputMagnitude: " + inputMagnitude);
            //transform.Translate(moveDirection * moveSpeed * inputMagnitude * Time.deltaTime, Space.World);
            rb.AddForce(moveDirection * acceleration);

        }

        //else if ((Input.GetButton("Vertical") || Input.GetButton("Horizontal")) && moveSpeed == maxSpeed)
        //{
        //    Debug.Log("Acceleration+\\" + moveSpeed + "moveDirection: " + moveDirection + "inputMagnitude: " + inputMagnitude);
        //    transform.Translate(moveDirection * moveSpeed * inputMagnitude * Time.deltaTime, Space.World);
        //}
        ////Deacceleration 
        //else if (moveSpeed > 0)
        //{
        //    moveSpeed += -deacceleration;
        //    //inputMagnitude = 1;
        //    Debug.Log("Deacceleration-//" + moveSpeed + "moveDirection: " + moveDirection + "inputMagnitude: " + inputMagnitude);
        //}

        //else
        //{
        //    transform.Translate(moveDirection * moveSpeed * inputMagnitude * Time.deltaTime, Space.World);
        //}


        //Move command
        //transform.Translate(moveDirection * moveSpeed * inputMagnitude * Time.deltaTime, Space.World);

        if (moveDirection != Vector2.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void ShootCannonRight()
    {
        
        GameObject[] rightCannonSpawn = GameObject.FindGameObjectsWithTag("RightBulletSpawn");

        //foreach (var _rightCannonSpawnPoint in rightCannonSpawn)
        //{

        //    var bulletInstance = Instantiate(ProjectilePrefab, _rightCannonSpawnPoint.transform.position, _rightCannonSpawnPoint.transform.rotation);
        //    Debug.Log(_rightCannonSpawnPoint);

        //    //Destroy(bulletInstance, 3.0f);

        //}

        Instantiate(ProjectilePrefab, LaunchOffset.position, transform.rotation);
        Invoke(nameof(ResetAttack), timeBetweenAttack);
        //Debug.Log(ProjectilePrefab + " :/: " + LaunchOffset.position + " :/: " + transform.rotation);

    }

    public void ResetAttack()
    {
        alreadyShooting = false;
    }

}

