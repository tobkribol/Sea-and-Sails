using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private PlayerMovement move;
    //private Animation animation;

    [SerializeField] private float playerHealth = 1.0f;
    
    //private float health = SailHelthBarFunction.GetHealthBarValue();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        move = GetComponent<PlayerMovement>();
        //animation = GetComponent<Animation>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            SailHelthBarFunction.SetHealthBarValue(SailHelthBarFunction.GetHealthBarValue() - 0.21f);
            Debug.Log("health: " + SailHelthBarFunction.GetHealthBarValue());
            SetHealthAnimationValue();

            if (SailHelthBarFunction.GetHealthBarValue() < 0.01f)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        anim.speed = 0f; //Set the animation to pause
        anim.Play("ship_standard_death", 0, 0.99f); //set the animation to the last frame
        Debug.Log("anim health 1.0");

        GetComponent<PlayerMovement>().enabled = false;
        rb.bodyType = RigidbodyType2D.Static;
        rb.velocity = Vector2.zero;
        Invoke(nameof(RestartLevel), 2f);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetHealthAnimationValue()
    {
        //Toggle different animation for different health state
        anim.speed = 0f; //Set all animations to pause
        if (SailHelthBarFunction.GetHealthBarValue() <= 0.2f)
        {
            anim.Play("ship_standard_death", 0, 0.79f); //Change damage sprite
            //move.moveSpeed = 0.2f;                      //Reduce player speed

            if (SailHelthBarFunction.GetHealthBarValue() < 0.01f)
            {
                Die();
            }
        }

        else if (SailHelthBarFunction.GetHealthBarValue() <= 0.4f)
        {
            anim.Play("ship_standard_death", 0, 0.59f); //Change damage sprite
            //move.moveSpeed = 0.8f;                      //Reduce player speed
        }

        else if (SailHelthBarFunction.GetHealthBarValue() <= 0.6f)
        {
            anim.Play("ship_standard_death", 0, 0.39f); //Change damage sprite
            //move.moveSpeed = 1.2f;                      //Reduce player speed
        }

        else if (SailHelthBarFunction.GetHealthBarValue() <= 0.8f)
        {
            anim.Play("ship_standard_death", 0, 0.19f); //Change damage sprite
            //move.moveSpeed = 1.4f;                      //Reduce player speed
        }

        else
        {
            anim.Play("ship_standard_idle", 0, 0.0f);
        }
    }
}
