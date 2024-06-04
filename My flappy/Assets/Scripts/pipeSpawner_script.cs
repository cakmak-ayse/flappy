using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pipeSpawner_script : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float initialTimer = 1f; // Initial spawn timer
    private float timer;
    private float count = 0;
    public float range_offset = 2f;
    public logic_script logic;

    // Parameters for difficulty scaling
    public int scoreThreshold = 1; // Increase difficulty every point
    public float spawnReduction = 0.05f; // Reduce spawn interval by 0.05 seconds
    public float minSpawnInterval = 0.5f; // Minimum interval to prevent spawning too fast

    // Parameters for movement speed scaling
    public float initialSpeed = 4f; // Initial movement speed
    public float speedIncrease = 0.5f; // Increase speed by 0.5 units per score
    public float maxSpeed = 40f; // Maximum movement speed

    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<logic_script>();
        while (logic == null)
        {
            logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<logic_script>();
        }
        if (logic == null)
        {
            Debug.LogError("Logic manager not found!");
        }
        else
        {
            timer = initialTimer; // Initialize the spawn timer
        }
    }

    void Update()
    {
        // Start spawning after the first flap
        if (logic.gameBegan == true)
        {
            AdjustSpawnTimer();

            if (count >= timer)
            {
                SpawnObject();
                count = 0;
            }
            else
            {
                count += Time.deltaTime;
            }
        }
    }

    void AdjustSpawnTimer()
    {
        int currentScore = logic.count;
        int difficultyLevel = currentScore / scoreThreshold;

        // Adjust the spawn timer, but ensure it doesn't go below the minimum spawn interval
        timer = Mathf.Max(initialTimer - difficultyLevel * spawnReduction, minSpawnInterval);
        //Debug.Log($"Adjusted Spawn Timer: {timer}");
    }

    void SpawnObject()
    {
        float y_max = transform.position.y + range_offset;
        float y_min = transform.position.y - range_offset;
        float randomNumber = Random.Range(y_min, y_max);
        Vector3 vector = new Vector3(transform.position.x, randomNumber, 0);
        GameObject newObject = Instantiate(objectToSpawn, vector, transform.rotation);

        // Set the movement speed of the newly spawned pipe
        float currentSpeed = CalculateCurrentSpeed();
        pipeMovement_script pipeMovement = newObject.GetComponent<pipeMovement_script>();
        if (pipeMovement != null)
        {
            pipeMovement.SetMovementSpeed(currentSpeed);
            Debug.Log($"Pipe speed set to: {currentSpeed}");
        }
        else
        {
            Debug.Log("pipeMovement_script was null, so nothing was set");
        }
    }

    float CalculateCurrentSpeed()
    {
        int currentScore = logic.count;
        int difficultyLevel = currentScore / scoreThreshold;

        // Calculate the current speed, but ensure it doesn't exceed the maximum speed
        
        float calculatedSpeed = Mathf.Min(initialSpeed + (difficultyLevel * speedIncrease), maxSpeed);
        Debug.Log($"Current Score: {currentScore}, Difficulty Level: {difficultyLevel}, Calculated Speed: {calculatedSpeed}");
        return calculatedSpeed;
    }
}
