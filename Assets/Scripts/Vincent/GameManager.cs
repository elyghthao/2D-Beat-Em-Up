using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private PlayerStateMachine playerRef;
    private List<EnemyStateMachine> enemyReferences;

    public PlayerStateMachine PlayerRef { get => playerRef; set => playerRef = value; }
    public List<EnemyStateMachine> EnemyReferences { get => enemyReferences; set => enemyReferences = value; }



    // Start is called before the first frame update
    void Awake() {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
