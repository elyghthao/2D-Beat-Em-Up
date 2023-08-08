using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Default substate of EnemyHurtState. When nothing is happening to the enemy other than recovering from having been
/// hit
/// </summary>
public class EnemyChaseState : EnemyBaseState
{
   private NavMeshAgent agent;

   public EnemyChaseState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) : base(currentContext, enemyStateFactory) {
   
   }

   public override void EnterState() {
      agent = Ctx.Enemy.GetComponent<NavMeshAgent>();
      // Debug.Log("ENEMY SUB: ENTERED CHASE");
   }

   public override void UpdateState() {
      agent.updateRotation = false;
      agent.updateRotation = false;
      agent.updateUpAxis = false;
      
      agent.SetDestination(Ctx.CurrentPlayerMachine.transform.position);

      Vector3 goalPos = agent.steeringTarget;
      Vector3 vecToGoal = goalPos - Ctx.gameObject.transform.position;
      vecToGoal = vecToGoal.normalized * Ctx.movementSpeed * 10f;
        
      Ctx.Rigidbody.AddForce(vecToGoal, ForceMode.Force);

      Ctx.SpeedControl();

      Vector3 enemyScale = Ctx.transform.localScale;
      Vector3 vecToPlayer = Ctx.MovingGoal.position - Ctx.gameObject.transform.position;
      if (vecToPlayer.x > 0) {
         // Ctx.transform.localEulerAngles = new Vector3(0, 0, 0);
         Ctx.transform.localScale = new Vector3(Mathf.Abs(enemyScale.x), enemyScale.y, enemyScale.z);
      } else {
         // Ctx.transform.localEulerAngles = new Vector3(0, -180, 0);
         Ctx.transform.localScale = new Vector3(-Mathf.Abs(enemyScale.x), enemyScale.y, enemyScale.z);
      }
   }

   public override void FixedUpdateState() {

   }

   public override void ExitState() {
      // Debug.Log("ENEMY SUB: EXITED CHASE");
   }

   public override void CheckSwitchStates() {

   }

   public override void InitializeSubState() {
      throw new System.NotImplementedException();
   }
}

/*

OLD SCRIPT BELOW

using UnityEngine;

/// <summary>
/// Default substate of EnemyHurtState. When nothing is happening to the enemy other than recovering from having been
/// hit
/// </summary>
public class EnemyChaseState : EnemyBaseState
{
   public EnemyChaseState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) : base(currentContext, enemyStateFactory) {
   
   }

   public override void EnterState() {
      // Debug.Log("ENEMY SUB: ENTERED CHASE");
   }

   public override void UpdateState() {
      //needs functionality to alter the speed of enemy, right now its proportional to distance
      Vector3 directionToPlayer = CurrentPlayerMachine.gameObject.transform.position - Ctx.gameObject.transform.position;
      directionToPlayer = directionToPlayer.normalized * 10; //this value affects speed
      Ctx.Rigidbody.AddForce(directionToPlayer, ForceMode.Force);
      // Debug.Log(directionToPlayer.x);


      //make it so the right of enemy will always face player when chasing
      Vector3 enemyScale = Ctx.transform.localScale;
      if(directionToPlayer.x > 0) {
         Ctx.transform.localScale = new Vector3(Mathf.Abs(enemyScale.x),enemyScale.y,enemyScale.z);
      }else {
         Ctx.transform.localScale = new Vector3(-Mathf.Abs(enemyScale.x),enemyScale.y,enemyScale.z);
      }

      CheckSwitchStates();
   }

   public override void ExitState() {
      // Debug.Log("ENEMY SUB: EXITED CHASE");
   }

   public override void CheckSwitchStates() {

   }

   public override void InitializeSubState() {
      throw new System.NotImplementedException();
   }
}

*/