using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public GameObject child;
    public GameObject EnemyRef;
    public float corruptionLevel;
    public bool isCorrupted;
    public bool isBeingDrained;

    // Start is called before the first frame update
    void Start()
    {
        child = Instantiate(EnemyRef, transform, true);
        corruptionLevel = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (isBeingDrained)
        {

        }
        else
        {
            if(corruptionLevel != 0)
            {

            }
        }
        if (corruptionLevel == 0)
        {
            isCorrupted = false;
        }
    }
}
