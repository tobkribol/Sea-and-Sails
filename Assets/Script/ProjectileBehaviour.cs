using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField] float speed = 12f;
    float destroyTime = 0.35f;

    [SerializeField] public PlayerLife playerLife;

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
            SailHelthBarFunction.SetHealthBarValue(SailHelthBarFunction.GetHealthBarValue() - 0.25f);
            Destroy(collision.gameObject);
            playerLife.SetHealthAnimationValue(); //FIX https://forum.unity.com/threads/need-help-with-accessing-variables-from-clones.1036504/

        }

        else if (collision.gameObject.CompareTag("Player"))
        {
            SailHelthBarFunction.SetHealthBarValue(SailHelthBarFunction.GetHealthBarValue() - 0.25f);
            playerLife.SetHealthAnimationValue();
        }

        Destroy(gameObject);
    }

}
