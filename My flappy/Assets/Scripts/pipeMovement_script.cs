using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pipeMovement_script : MonoBehaviour
{
    public float movementSpeed = 5;
    // float randomNumber = Random.Range(-2f, 2f);

    // Start is called before the first frame update
    void Start()
    {
        //transform.position = new Vector3(transform.position.x, randomNumber, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + (Vector3.left * movementSpeed) * Time.deltaTime; 
        // time delta makes sure  you move based on real time and apparently somehow makes your code magically lover fps
    }
}
