using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class spawncontroller : MonoBehaviour
{
    public GameObject zombitPrefab;
    public float nextSpawnCd; //spawning interval cd
    public float nextSpawn; //spawning interval

    public int enemyCounter;
    public int maxEnemies; //maximum enemy on screen

    public player[] theTarget;

    public GameObject spawnPoints;

    public TextMeshProUGUI currentScore; //killcount
    public TextMeshProUGUI timerCount; //min:sec
    public TextMeshProUGUI cdToStart; //countdown to stage start
    public TextMeshProUGUI timesUp; //TIME'S UP

    public float countDownToStart; //countdown to start spawning
    public float missionDuration_min;
    public int killCount;

    public bool startSpawn;
    public bool stageEnded;
    public GameObject spawnParent; //parent all spawns to this
    // Start is called before the first frame update
    void Start()
    {
        missionDuration_min *= 60; //change to seconds
        timesUp.text = "";
        timerCount.text = "";
        currentScore.text = killCount.ToString();
        cdToStart.text = Mathf.CeilToInt(countDownToStart).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(!startSpawn && !stageEnded)
        {
            if (countDownToStart > 0)
            {
                countDownToStart -= Time.deltaTime;
                cdToStart.text = Mathf.CeilToInt(countDownToStart).ToString();
            }
            else
            {
                cdToStart.text = "";
                startSpawn = true;
                int minutes = Mathf.FloorToInt(missionDuration_min / 60);
                int seconds = Mathf.FloorToInt(missionDuration_min % 60);

                string niceTime = string.Format("{0:00}:{1:00}", minutes, seconds);
                timerCount.text = niceTime;
                //timerCount.text = Mathf.FloorToInt(missionDuration_min / 60).ToString() + ":" + Mathf.FloorToInt(missionDuration_min % 60).ToString();
            }
        }

        if(startSpawn)
        {
            if (missionDuration_min > 0)
            {
                missionDuration_min -= Time.deltaTime;
                int minutes = Mathf.FloorToInt(missionDuration_min / 60);
                int seconds = Mathf.FloorToInt(missionDuration_min % 60);

                string niceTime = string.Format("{0:00}:{1:00}", minutes, seconds);
                timerCount.text = niceTime;
            }
            else
            {
                stageEnded = true;
                timesUp.text = "TIME'S UP!";
                timerCount.text = "";
                startSpawn = false;
                DestroyAllSpawns();
            }

            if (nextSpawnCd >= 0)
                nextSpawnCd -= Time.deltaTime;
            else
            {
                if (enemyCounter < maxEnemies)
                {
                    SpawnZombit();
                }

                nextSpawnCd = nextSpawn;
            }
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

        inst.transform.SetParent(spawnParent.transform);
    }

    void DestroyAllSpawns()
    {
        for(int i = 0;i<spawnParent.transform.childCount;i++)
        {
            enemybasic sc = spawnParent.transform.GetChild(i).GetComponent<enemybasic>();
            if(!sc.isKO)
            {
                sc.DoKoStuff();
            }
        }
    }
}
