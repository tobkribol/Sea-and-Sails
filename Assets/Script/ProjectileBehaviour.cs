using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    //Inputdata 
    [SerializeField] float speed = 12f;
    [SerializeField] float destroyTime = 0.35f;
    [SerializeField] private Rigidbody2D rb;

    //Get Player data
    public GameObject Player;
    PlayerLife PlayerLifeScript;
    PlayerMovement PlayerMovement;


    // Update is called once per frame

    private void Start()
    {
        //Get Player data without turning the player into a prefab
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerLifeScript = Player.GetComponent<PlayerLife>();
        PlayerMovement = Player.GetComponent<PlayerMovement>();
        Vector2 projectileDirection = Player.transform.right.normalized;

        if (PlayerMovement.shootSide)
        {
            rb.AddForce(projectileDirection * (4 + (Random.value * 2)), ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(-projectileDirection * (4 + (Random.value * 2)), ForceMode2D.Impulse);
        }
    }


    void Update()
    {
        Destroy(gameObject, destroyTime); //Destroy if it does not hit anything
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            SailHelthBarFunction.SetHealthBarValue(SailHelthBarFunction.GetHealthBarValue() - 0.25f);
            Destroy(collision.gameObject);
            PlayerLifeScript.SetHealthAnimationValue(); //Testing
            Destroy(gameObject);
        }

        else if (collision.gameObject.CompareTag("Player"))
        {
            SailHelthBarFunction.SetHealthBarValue(SailHelthBarFunction.GetHealthBarValue() - 0.25f);
            PlayerLifeScript.SetHealthAnimationValue();
            Destroy(gameObject);
        }

        //Destroy(gameObject);
    }

}
