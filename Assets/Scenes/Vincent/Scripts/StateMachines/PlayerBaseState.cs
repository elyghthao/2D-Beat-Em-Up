using UnityEngine;

public abstract class PlayerBaseState {

    private bool _isRootState = false;
    private PlayerStateMachine _ctx;
    private PlayerStateFactory _factory;
    private PlayerBaseState _currentSuperState;
    private PlayerBaseState _currentSubState;

    protected bool IsRootState { set => _isRootState = value; }
    protected PlayerStateMachine Ctx { get => _ctx; }
    protected PlayerStateFactory Factory { get => _factory; }

    public PlayerBaseState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) {
        _ctx = currentContext;
        _factory = playerStateFactory;
    }
    
    /// <summary>
    /// On state entry
    /// </summary>
    public abstract void EnterState();

    /// <summary>
    /// Update every CPU cycle
    /// </summary>
    public abstract void UpdateState();

    /// <summary>
    /// On state exit
    /// </summary>
    public abstract void ExitState();

    /// <summary>
    /// Checks conditionals for if it's time to switch states, and what state to switch to
    /// </summary>
    public abstract void CheckSwitchStates();

    /// <summary>
    /// Initializes any substates of this root state. NOT IMPLEMENTED YET
    /// </summary>
    public abstract void InitializeSubState();

    /// <summary>
    /// Updates substates
    /// </summary>
    public void UpdateStates() {
        UpdateState();
        if (_currentSubState != null) {
            _currentSubState.UpdateStates();
        }
    }
    
    protected void SwitchState(PlayerBaseState newState) {
        ExitState();
        newState.EnterState();
        if (_isRootState) {
            // Switches superstates
            _ctx.CurrentState = newState;
        } else if (_currentSuperState != null) {
            // Switches substates
            _currentSuperState.SetSubState(newState);
        }
    }

    protected void SetSuperState(PlayerBaseState newSuperState) {
        _currentSuperState = newSuperState;
    }

    protected void SetSubState(PlayerBaseState newSubState) {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
}
