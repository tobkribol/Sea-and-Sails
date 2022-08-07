using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float playerHealth = 1f; 

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SailHelthBarFunction.SetHealthBarValue(1);

    }

    // Update is called once per frame
    void Update()
    {
        if (SailHelthBarFunction.GetHealthBarValue() < 0.01f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            SailHelthBarFunction.SetHealthBarValue(SailHelthBarFunction.GetHealthBarValue() - 0.25f);
            
            if (SailHelthBarFunction.GetHealthBarValue() < 0.01f)
            {
                Die();
            }

        }
    }

    private void Die()
    {
        rb.bodyType = RigidbodyType2D.Static;
        RestartLevel();
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
