using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointFinalLevelScript : MonoBehaviour
{

    public GameManager gameManagerScript;
    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other){
        if (other.gameObject.transform.parent.gameObject.CompareTag("Player"))
        {
            gameManagerScript.spawnAtCheckPoint = true;
        }
    }



}
