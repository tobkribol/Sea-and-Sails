using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileBehaviour : MonoBehaviour
{
    //Inputdata
    [SerializeField] public float shootForceSpread = 0.6f;
    [SerializeField] float speed = 12f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ParticleSystem PS;
    private EnemyBehaveour enemyBehaveour;


    //Get Player data
    private GameObject Player;
    PlayerLife PlayerLifeScript;
    PlayerMovement PlayerMovement;
    //[SerializeField] public Text killText; //need UnityEngine.UI


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
            rb.AddForce(projectileDirection * (PlayerMovement.shootForce + (Random.Range(-shootForceSpread, shootForceSpread))), ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(-projectileDirection * (PlayerMovement.shootForce + (Random.Range(-shootForceSpread, shootForceSpread))), ForceMode2D.Impulse);
        }
    }

    void Update()
    {
        
        Destroy(gameObject, PlayerMovement.GunFireDisance); //Destroy if it does not hit anything

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            int debreeRotation = -1;
            if (PlayerMovement.shootSide)
            {
                debreeRotation = 1;
            }
            Instantiate(PS, transform.position, transform.rotation * Quaternion.Euler(new Vector3(0, 0, debreeRotation* 90f)));
            enemyBehaveour = collision.gameObject.GetComponent<EnemyBehaveour>();
            enemyBehaveour.health -= PlayerMovement.currentAmmoType;
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
