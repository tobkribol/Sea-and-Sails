using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaveour : MonoBehaviour
{
    public float health = 100.0f;
    PlayerLife PlayerLifeScript;
    private GameObject Player;


    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerLifeScript = Player.GetComponent<PlayerLife>();
    }

    // Update is called once per frame
    void Update()
    {
        if(health < 0.1f)
        {
            PlayerLifeScript.killCount += 1;
            Debug.Log("Kills: " + PlayerLifeScript.killCount);
            Destroy(gameObject);
        }
    }
}
