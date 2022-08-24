using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileBehaviour : MonoBehaviour
{
    //Inputdata
    [SerializeField] public float shootForce = 4f;
    [SerializeField] public float shootForceSpread = 1f;
    [SerializeField] float speed = 12f;
    [SerializeField] float destroyTime = 0.35f;
    [SerializeField] private Rigidbody2D rb;

    //Get Player data
    public GameObject Player;
    PlayerLife PlayerLifeScript;
    PlayerMovement PlayerMovement;
    [SerializeField] public Text killText; //need UnityEngine.UI


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
            rb.AddForce(projectileDirection * (shootForce + (Random.Range(-shootForceSpread / 2, shootForceSpread / 2))), ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(-projectileDirection * (shootForce + (Random.Range(-shootForceSpread / 2, shootForceSpread / 2))), ForceMode2D.Impulse);
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
            //SailHelthBarFunction.SetHealthBarValue(SailHelthBarFunction.GetHealthBarValue() - 0.25f);
            //PlayerLifeScript.SetHealthAnimationValue(); //Testing
            Destroy(collision.gameObject);
            PlayerLifeScript.killCount += 1;
            Debug.Log("Kills: " + PlayerLifeScript.killCount);
            //killText.text = "Kills: " + PlayerLifeScript.killCount.ToString(); //Not displaying 
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
