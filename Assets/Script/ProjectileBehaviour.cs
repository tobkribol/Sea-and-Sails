using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField] float speed = 12f;
    [SerializeField] float destroyTime = 3.0f;

    // Update is called once per frame
    private void Update()
    {
        transform.position += transform.right * Time.deltaTime * speed;

        Destroy(gameObject, destroyTime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);

        }

        Destroy(gameObject);
    }

    


}