using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drain : MonoBehaviour
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
        if (isCorrupted)
        {
            if (isBeingDrained && corruptionLevel > 0)
            {
                corruptionLevel -= 0.1f;
            }

            if (!isBeingDrained && corruptionLevel < 1)
            {
                corruptionLevel += 0.1f;
            }

            if (corruptionLevel == 0)
            {
                isCorrupted = false;
                Debug.Log("Object purified");
            }
        }
    }
}
