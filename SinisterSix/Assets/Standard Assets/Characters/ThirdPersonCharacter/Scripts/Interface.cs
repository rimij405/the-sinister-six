using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine;

public class Interface : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartChasing()
    {
        GetComponentInParent<AICharacterControl>().IsChasing = true;
    }

    public void StopChasing()
    {
        GetComponentInParent<AICharacterControl>().IsChasing = false;
    }
}
