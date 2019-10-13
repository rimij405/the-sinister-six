using System;
using UnityEngine;
using UnityEngine.AI;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class AICharacterControl : MonoBehaviour
    {
        public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
        public Transform target;                                    // target to aim for

        public float wanderRadius;
        public float wanderTime;
        private float timer;

        public bool isBound;
        public float boundTime;
        private float boundTimer;

        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();

	        agent.updateRotation = false;
	        agent.updatePosition = true;

            //set material atribytes
            character.transform.GetChild(0).GetComponent<Renderer>().material.SetFloat("_StasisAmount", 0.0f);
            character.transform.GetChild(0).GetComponent<Renderer>().material.SetFloat("_NoiseAmount", 0.0f);

            //set timers
            timer = wanderTime;
            boundTimer = 0;
        }


        private void Update()
        {
            //if (target != null)
            //    agent.SetDestination(target.position);

            if (agent.remainingDistance > agent.stoppingDistance && !isBound)
            {
                character.Move(agent.desiredVelocity, false, false);
            }
            else
            {
                character.Move(Vector3.zero, false, false);
            }

            timer += Time.deltaTime;

            //bound cooldown
            if (isBound)
            {
                boundTimer += Time.deltaTime;

                if (boundTimer >= boundTime)
                {
                    isBound = false;
                    character.transform.GetChild(0).GetComponent<Renderer>().material.SetFloat("_StasisAmount", 0.0f);
                    character.transform.GetChild(0).GetComponent<Renderer>().material.SetFloat("_NoiseAmount", 0.0f);
                    boundTimer = 0;
                }
            }

            //new wander pos cooldown
            if (timer >= wanderTime && !isBound)
            {
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);

                agent.SetDestination(newPos);

                Debug.Log("target updated");

                timer = 0;
            }
        }


        public void SetTarget(Transform target)
        {
            this.target = target;
        }

        public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
        {
            Vector3 randDirection = UnityEngine.Random.insideUnitSphere * dist;

            randDirection += origin;

            NavMeshHit navHit;

            NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

            return navHit.position;
        }
    }
}
