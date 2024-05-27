using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class objectDespawn_script : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   void OnTriggerEnter2D(Collider2D other)
    {
         // Check if the collided object has a parent
        if (other.transform.parent != null)
        {
            // Destroy the parent object
            Destroy(other.transform.parent.gameObject);
        }
        else
        {
            // Destroy the collided object if it doesn't have a parent
            Destroy(other.gameObject);
        }
    }
    // void OnTriggerEnter(Collider collision){
    //     Destroy(collision.gameObject);
    // }
}
