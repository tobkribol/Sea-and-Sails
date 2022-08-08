using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    //Inputdata 
    [SerializeField] float speed = 12f;
    [SerializeField] float destroyTime = 0.35f;

    //Get Player data
    public GameObject Player;
    PlayerLife PlayerLifeScript;


    // Update is called once per frame

    private void Start()
    {
        //Get Player data without turning the player into a prefab
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerLifeScript = Player.GetComponent<PlayerLife>();
    }


    private void Update()
    {
        //After launch
        transform.position += transform.right * Time.deltaTime * speed; //Move projectile forward     
        Destroy(gameObject, destroyTime); //Destroy if it does not hit anything
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            SailHelthBarFunction.SetHealthBarValue(SailHelthBarFunction.GetHealthBarValue() - 0.25f);
            Destroy(collision.gameObject);
            PlayerLifeScript.SetHealthAnimationValue(); //Testing
        }

        else if (collision.gameObject.CompareTag("Player"))
        {
            SailHelthBarFunction.SetHealthBarValue(SailHelthBarFunction.GetHealthBarValue() - 0.25f);
            PlayerLifeScript.SetHealthAnimationValue();
        }

        Destroy(gameObject);
    }

}
