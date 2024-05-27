using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class bird_script : MonoBehaviour
{
    public Rigidbody2D myRigidBody;
    public float flapStrength = 5;      // to change flap play with this and gravity scale on rigid-body2d
    public logic_script logic;
    private bool bird_is_alive = true;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        // I had some errors with pipes before so thats why this looks overly complicated. It only gets logic script basically
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<logic_script>();
        audioSource = GetComponent<AudioSource>();
        while(logic == null){
            logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<logic_script>();
        }
        if (logic == null)
        {
            Debug.LogError("Logic manager not found!");
        }
        else
        {
            Debug.Log("Logic manager found: " + logic);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //FLAP FLAP FLAP
        if(Input.GetKeyDown(KeyCode.Space) == true && bird_is_alive){
            //this is so bird is hanging on air untill first space is pressed
            //and to take away the flap to start screen
            if(myRigidBody.gravityScale == 0){
                myRigidBody.gravityScale =1;
                logic.StartSceneDectivate();
            }
            myRigidBody.velocity = Vector2.up * flapStrength;
            // flap animation?
            //flap audio?
            audioSource.PlayOneShot(audioSource.clip ,1F);
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision2D){
        logic.GameOverSceneActivate();
        this.bird_is_alive = false;
    }
}
