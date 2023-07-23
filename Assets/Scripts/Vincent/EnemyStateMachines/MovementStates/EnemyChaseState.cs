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
      Ctx.MovingGoal = Ctx.CurrentPlayerMachine.transform;
      Ctx.MovingGoalOffset = new Vector2(0, 0);
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