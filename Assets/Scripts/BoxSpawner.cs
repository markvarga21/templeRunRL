using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject boxPrefab;
    [SerializeField]
    private GameObject doubleBox;
    [SerializeField]
    private GameObject emptyBox;
    private float delay = 3.0f;
    float nextTimeToSpawn = 0f;

    [SerializeField]
    private GameObject[] spawnObjects;

    private string[] spawnPatters = { 
        "s--", "--s",
        "ss-", "-ss",
        "sss",
        "s-s",
        "---",
        "sd-", "-ds",
        "s-d", "d-s",
        "ssd", "dss",
        "sdd", "dds",
        "d-s", "-sd",
        "d--", "--d",
        "dd-", "-dd",
        "d-d"
    };

    private GameObject GetObstacleForPatternLetter(char letter)
    {
        GameObject gameObject = null;
        switch (letter)
        {
            case 's':
                gameObject = boxPrefab;
                break;
            case 'd':
                gameObject = doubleBox;
                break;
            case '-':
                gameObject = emptyBox;
                break;
        }
        return gameObject;
    }

    private Vector3 GetPosition(int position, char boxType)
    {
        float yPos = boxType == 's' ? 0.12f : 0.5f;
        return new Vector3(
            spawnObjects[position].transform.position.x,
            yPos,
            spawnObjects[position].transform.position.z
        );
    }

    private GameObject SpawnObstacleRow(int position, string pattern)
    {
        Vector3 tempPos = GetPosition(position, pattern[position]);
        GameObject temp = GetObstacleForPatternLetter(pattern[position]);
        return Instantiate(temp, tempPos, Quaternion.identity);
    }
    
    void Start()
    {
        nextTimeToSpawn = Time.time;
    }

    
    void Update()
    {
        if (Time.time > nextTimeToSpawn)
        {
            nextTimeToSpawn = Time.time + delay;
            string pattern = spawnPatters[Random.Range(0, spawnPatters.Length)];
            GameObject left = SpawnObstacleRow(0, pattern);
            GameObject middle = SpawnObstacleRow(1, pattern);
            GameObject right = SpawnObstacleRow(2, pattern);
            left.AddComponent<Move>();
            middle.AddComponent<Move>();
            right.AddComponent<Move>();
        }
    }
}

public class Move : MonoBehaviour
{
    private float speed = 3.0f;

    private void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime);
    }
}
