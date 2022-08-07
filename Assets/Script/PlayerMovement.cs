using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] public float moveSpeed = 1.5f;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] public float timeBetweenAttack = 0.2f;

    public ProjectileBehaviour ProjectilePrefab;
    public Transform LaunchOffset;

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
            ShootCannon();
        }
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY);
        inputMagnitude = Mathf.Clamp01(moveDirection.magnitude);
        moveDirection.Normalize();
    }

    private void Move()
    {
       
        transform.Translate(moveDirection * moveSpeed * inputMagnitude * Time.deltaTime, Space.World);

        if (moveDirection != Vector2.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void ShootCannon()
    {

        Instantiate(ProjectilePrefab, LaunchOffset.position, transform.rotation);
        Invoke(nameof(ResetAttack), timeBetweenAttack);
        Debug.Log(ProjectilePrefab + " :/: " + LaunchOffset.position + " :/: " + transform.rotation);

    }

    public void ResetAttack()
    {
        alreadyShooting = false;
    }

}

