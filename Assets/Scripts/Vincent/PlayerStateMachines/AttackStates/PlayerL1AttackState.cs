using UnityEngine;

public class PlayerL1AttackState : PlayerBaseState {
    // Handles timing of the attack for startup, active, and recovery frames
    private float _animationTime;
    private float _currentFrame = 1;
    private float _timePerFrame;
    // 0 == startup, 1 == active, 2 == recovery, 3 == finished
    private int _currentFrameState;
    
    public PlayerL1AttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) {
        CanSwitch = false;
    }

    public override void EnterState() {
        //Debug.Log("SUB: ENTERED LIGHT 1");
        _timePerFrame = (Ctx.framesPerSecond / 60f)/60f;
        Ctx.lightFirstFollowupAttackBounds.SetActive(true);
        Ctx.FollowupTimer = Ctx.attackFollowupThreshold;
        Ctx.MostRecentAttack = ToString();
        Ctx.StaminaRegenAllowed = false;
        Ctx.Stamina -= Ctx.LightFirstFollowupBounds.staminaDrain;
    }

    public override void UpdateState() {
        _animationTime += Time.deltaTime;
        _currentFrame = _animationTime / _timePerFrame;

        _currentFrameState = Ctx.FrameState(Ctx.LightFirstFollowupBounds, _currentFrame, Ctx.light1StartupFrames,
            Ctx.light1ActiveFrames, Ctx.light1RecoveryFrames);
        //Debug.Log("CurrentFrameState for LightAttack 1: " + _currentFrameState);
        if (Ctx.InputSys.IsLightAttackPressed && _currentFrameState >= 2 && !Ctx.InputSys.IsActionHeld
            && Ctx.Stamina >= Ctx.LightSecondFollowupBounds.staminaDrain) {
            Ctx.QueuedAttack = Factory.LightSecondFollowupAttack();
            //Debug.Log("LightAttack 2 Queued");
        }
        if (_currentFrameState == 3) {
            CanSwitch = true;
            CheckSwitchStates();
        }
    }

    public override void ExitState() {
        //Debug.Log("SUB: EXITED LIGHT 1");
        Ctx.lightFirstFollowupAttackBounds.SetActive(false);
    }

    public override void CheckSwitchStates() {
        if (Ctx.QueuedAttack != null) {
            SwitchState(Ctx.QueuedAttack);
            Ctx.ResetAttackQueue();
        } else {
            SwitchState(Factory.Idle(), true); // TEMP FIX for action not ending because the action is being held down
        }
    }

    public override void InitializeSubState() {
        throw new System.NotImplementedException();
    }
}
