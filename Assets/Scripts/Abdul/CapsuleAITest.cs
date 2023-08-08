using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CapsuleAITest : MonoBehaviour {
    public NavMeshAgent agent;
    public Transform player;
    public float speed = 5;
    public Rigidbody body;

    // Update is called once per frame
    void Update() {
        agent.SetDestination(player.position);

        Vector3 goalPos = agent.steeringTarget;
        Vector3 vecToGoal = goalPos - transform.position;
        vecToGoal = vecToGoal.normalized * speed * 10f;
        
        body.AddForce(vecToGoal, ForceMode.Force);
    }

    public void SpeedControl() {
        Vector3 enemyVelocity = body.velocity;
        Vector3 flatVelocity = new Vector3(enemyVelocity.x, 0f, enemyVelocity.z);
      
        // limit velocity if needed
        if (flatVelocity.magnitude > speed) {
            Vector3 limitedVelocity = flatVelocity.normalized * speed;
            body.velocity = new Vector3(limitedVelocity.x, 0f, limitedVelocity.z);
        }
    }
}
