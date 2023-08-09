using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerSmackedState : PlayerBaseState
{
    public PlayerSmackedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) :
        base(currentContext, playerStateFactory) {}
    
    public override void EnterState() {
        // Debug.Log("SUB: ENTERED SMACKED");
        if (!Ctx.KnockedDown) {
            Ctx.BaseMaterial.color = new Color(255, 68, 0, 255);
        }
        List<string> recievedAttackNames = Ctx.ApplyAttackStats();
        // Sets the stun timer to 0.5f, which is the default for any non-knockdown attack
        if (Ctx.StunTimer < 0.5f) {
            Ctx.StunTimer = 0.5f;
        }

        if (!recievedAttackNames.Contains("SlamAttack")) {
            GameObject smackedInstance = Ctx.InstantiatePrefab(GameManager.SmackedPrefabInstance);
            smackedInstance.transform.position = Ctx.transform.position;
            if (Ctx.KnockedDown) {
                smackedInstance.transform.position += new Vector3(0, 0, -0.05f);
            } else {
                smackedInstance.transform.position += new Vector3(0, 3, 0.05f);
            }
        }
        GameManager.Camera.DOShakePosition(0.5f, GameManager.Instance.cameraShakeStrength);
    }

    public override void UpdateState() {
        if (!Ctx.IsAttacked) {
            CheckSwitchStates();
        }
    }

    public override void FixedUpdateState() {
        
    }

    public override void ExitState() {
        // Debug.Log("SUB: EXITED SMACKED");
    }

    public override void CheckSwitchStates() {
        if (Ctx.KnockdownMeter <= 0) {
            SwitchState(Factory.KnockedDown());
            return;
        }
        SwitchState(Factory.Stunned());
    }

    public override void InitializeSubState() {
        throw new System.NotImplementedException();
    }
}
