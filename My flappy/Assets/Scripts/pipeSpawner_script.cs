using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pipeSpawner_script : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float timer = 3;
    private float count = 0;
    public float range_offset = 2;
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
    // Update is called once per frame
    void Update()
    {
        //put in a if so it starts spawning after the first flap
        if(logic.gameBegan == true){
            if (count >= timer){
            SpawnOject();
            count = 0;
            } else{
                count += Time.deltaTime;
            }
        }
        // check for score to arrange timer
    }
    void SpawnOject()
    {
        float y_max = transform.position.y + range_offset;
        float y_min = transform.position.y - range_offset;
        float randomNumber = Random.Range(y_min, y_max);
        Vector3 vector = new Vector3 (transform.position.x, transform.position.y +randomNumber, 0 );
        GameObject newObject = Instantiate(objectToSpawn,vector, transform.rotation);
    }
}
