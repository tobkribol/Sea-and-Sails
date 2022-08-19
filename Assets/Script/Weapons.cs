using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{

    [SerializeField] public float timeBetweenAttack = 0.2f;
    public ProjectileBehaviour ProjectilePrefab;
    public Transform LaunchOffset;

    bool alreadyShooting = false;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Cherry"))
    //    {
    //        collectionSoundEffect.Play();
    //        Destroy(collision.gameObject);
    //        Items.cherries += 2;
    //        cherriesText.text = "Cherries: " + Items.cherries;
    //        Debug.Log(Items.cherries);
    //    }
    //}
