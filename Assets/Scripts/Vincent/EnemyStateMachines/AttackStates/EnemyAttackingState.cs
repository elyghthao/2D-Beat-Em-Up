using System.Threading;
using UnityEngine;

/// <summary>
/// Root state for when the enemy is hurt
/// </summary>
public class EnemyAttackingState : EnemyBaseState {
   public EnemyAttackingState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) : base(currentContext, enemyStateFactory) {
      IsRootState = true;
      InitializeSubState();
   }

   public override void EnterState() {
      // Debug.Log("ENEMY ROOT: ENTERED ATTACKING");
      Ctx.Attacking = true;
      Ctx.BaseMaterial.color = Color.red;
      Ctx.Rigidbody.velocity = Vector3.zero;
      Ctx.LastAttacked = Time.deltaTime;
   }

   public override void UpdateState() {
      CheckSwitchStates();
   }

   public override void FixedUpdateState() {
      
   }

   public override void ExitState() {
      // Debug.Log("ENEMY ROOT: EXITED MOVING");
      Ctx.Attacking = false;
   }

   public override void CheckSwitchStates() {
      // Checking to see if the enemy got hurt
      if (Ctx.IsAttacked && !Ctx.isBlocking) {
         SwitchState(Factory.Hurt());
      }

      // Checking only if we are not attacking anymore (that is turned off in substates unless this state is forced out)
      if (!Ctx.Attacking) {
         // Checking first if there is no player or the player is too far
         if (CurrentPlayerMachine == null) {
            SwitchState(Factory.Idle());
         } else {
            float dist = Vector3.Distance(Ctx.gameObject.transform.position, CurrentPlayerMachine.gameObject.transform.position); // 
            if (dist > Ctx.attackDistance) { // too far, go back to idle
               SwitchState(Factory.Idle());
            } 
         }
      }
   }

   public override void InitializeSubState() {
      // If a heavy enemy, can do heavy and medium attacks
      if (Ctx.enemyType == EnemyStateMachine.EnemyType.Heavy) {
         int attackNumber = Random.Range(1, 101); 
         if (attackNumber <= 30) { //30%
            SetSubState(Factory.MediumAttack());
         } else if (attackNumber <= 100) { //70%
            SetSubState(Factory.HeavyAttack());
         }
      } else if (Ctx.enemyType == EnemyStateMachine.EnemyType.Medium) {
         int attackNumber = Random.Range(1, 101); 
         if (attackNumber <= 30) {//30%
            SetSubState(Factory.LightAttack());
         } else if (attackNumber <= 100) {//70%
            SetSubState(Factory.MediumAttack());
         }



         
      }  else if (Ctx.enemyType == EnemyStateMachine.EnemyType.Light) {
         int attackNumber = Random.Range(1, 3); // 1 for light, 2 for medium
         SetSubState(Factory.LightAttack());
      }  else if (Ctx.enemyType == EnemyStateMachine.EnemyType.Boss) {


            // SetSubState(Factory.LightAttack());
            // SetSubState(Factory.MediumAttack());
            // SetSubState(Factory.HeavyAttack());
            // SetSubState(Factory.Block());



            int attackNumber = Random.Range(1, 101); 
            // Debug.Log("attackNumber: " + attackNumber);
            if (attackNumber <= 40) {//40%
               SetSubState(Factory.LightAttack());
               // Debug.Log("light attack");
            } else if (attackNumber <= 80) {//40%
               SetSubState(Factory.MediumAttack());
               // Debug.Log("medium attack");
            }else if (attackNumber <= 90) { //10%
               SetSubState(Factory.Block());
               // Debug.Log("block");
            }else if (attackNumber <= 100) { //10%
               SetSubState(Factory.HeavyAttack());
               // Debug.Log("heavy attack");
            }
            
      } 
   }
}
