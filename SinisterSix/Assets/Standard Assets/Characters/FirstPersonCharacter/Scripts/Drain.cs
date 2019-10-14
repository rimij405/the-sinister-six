using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Drain : MonoBehaviour
{
    public GameObject child;
    public GameObject EnemyRef;
    public float corruptionLevel;
    public bool isCorrupted;
    public bool isBeingDrained;
    public float alpha = 0;


    // Start is called before the first frame update
    void Start()
    {
        child = Instantiate(EnemyRef, new Vector3(transform.position.x + 10f, transform.position.y, transform.position.z), Quaternion.identity);
        //child.GetComponent<ViewCone>().SetTarget();
        corruptionLevel = 2;
        isCorrupted = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCorrupted)
        {
            if (isBeingDrained && corruptionLevel > 0)
            {
                corruptionLevel -= 0.03f;
            }

            if (!isBeingDrained && corruptionLevel < 1)
            {
                corruptionLevel += 0.03f;
            }

            if (corruptionLevel < 0)
            {
                isCorrupted = false;
                Debug.Log(child.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).name);
                child.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().material.SetFloat("_IsDissolve", 0.0f);

                child.transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().material.SetFloat("_IsDissolve", 0.0f);

                child.transform.GetChild(0).transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().material.SetFloat("_IsDissolve", 0.0f);
            }
        }
        else
        {
            if (alpha < 1)
            {
                alpha += 0.01f;
                //child.transform.GetChild(0).GetComponent<Renderer>().material.SetFloat("_DissolveAmount", Mathf.Lerp(-1, 2, alpha));
                child.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().material.SetFloat("_DissolveAmount", Mathf.Lerp(-1, 2, alpha));

                child.transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().material.SetFloat("_DissolveAmount", Mathf.Lerp(-1, 2, alpha));

                child.transform.GetChild(0).transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().material.SetFloat("_DissolveAmount", Mathf.Lerp(-1, 2, alpha));
            }
        }

        if (alpha > 1)
        {
            Destroy(child, 1.0f);
        }
    }
}
