using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// public class pipe_script : MonoBehaviour
// {
//     public logic_script logic;

//     void Start(){
//         print("find logic");
//         logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<logic_script>();
//         print(logic);
//     }
//     public void OnTriggerEnter2D(Collider2D collision){
//         logic.incrementCount();
//         print("detected collision properly");
//         print("text2");

//     }
// }

public class pipe_script : MonoBehaviour
{
    public logic_script logic;

    void Start (){
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<logic_script>();
        while(logic == null){
            logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<logic_script>();
        }
        if (logic == null)
        {
            Debug.LogError("Logic manager not found!");
        }
        else
        {
           // Debug.Log("Logic manager found: " + logic);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (logic != null)
        {
            //Debug.Log("Collision detected properly");
            if(collision.gameObject.tag.Equals("Player") == true){
                logic.IncrementCount();
                //Debug.Log("other IS player confirmed");
            }
            
        }
        else
        {
            Debug.Log("Logic manager is null!");
        }
    }
}
