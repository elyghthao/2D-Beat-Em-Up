using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CapsuleAITest : MonoBehaviour {
    
    public Transform player;
    public float speed = 5;
    public Rigidbody body;

    private GameObject fakeAI;
    private NavMeshAgent agent;

    void Start() {
        CreateFakeAI();
    }

    // Update is called once per frame
    void Update() {
        agent.SetDestination(player.position);

        Vector3 goalPos = agent.steeringTarget;
        Vector3 vecToGoal = goalPos - transform.position;
        vecToGoal = vecToGoal.normalized * speed * 10f;
        
        body.AddForce(vecToGoal, ForceMode.Force);
        SpeedControl();


        // body.isKinematic = true;
        // agent.enabled = true;

        // agent.SetDestination(player.position);

        // Vector3 goalPos = agent.steeringTarget;
        // Vector3 vecToGoal = goalPos - transform.position;
        // vecToGoal = vecToGoal.normalized * speed * 10f;

        // agent.enabled = false;
        // body.isKinematic = false;
        
        // body.AddForce(vecToGoal, ForceMode.Force);
        // SpeedControl();
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

    public void CreateFakeAI() {
        GameObject newObj = new GameObject("Fake_AI");
        newObj.AddComponent<NavMeshAgent>();
        newObj.layer = LayerMask.NameToLayer("Enemy");
        newObj.transform.position = transform.position;

        agent = newObj.GetComponent<NavMeshAgent>();

        fakeAI = newObj;
    }
}
