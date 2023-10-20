using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawncontroller : MonoBehaviour
{
    public GameObject zombitPrefab;
    public float nextSpawnCd;
    public float nextSpawn;

    public int enemyCounter;
    public int maxEnemies; //maximum enemy on screen

    public player[] theTarget;

    public GameObject spawnPoints;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (nextSpawnCd >= 0)
            nextSpawnCd -= Time.deltaTime;
        else
        {
            if(enemyCounter < maxEnemies)
            {
                SpawnZombit();
            }

            nextSpawnCd = nextSpawn;
        }
    }

    void SpawnZombit()
    {
        enemyCounter++;
        int rando = Random.Range(0, spawnPoints.transform.childCount);
        var inst = Instantiate(zombitPrefab, spawnPoints.transform.GetChild(rando).transform.position, Quaternion.identity);

        int rando2 = Random.Range(0, theTarget.Length);
        enemybasic sc = inst.GetComponent<enemybasic>();
        sc.myTarget = theTarget[rando2].transform;
        sc.spawnCont = this;
    }
}
