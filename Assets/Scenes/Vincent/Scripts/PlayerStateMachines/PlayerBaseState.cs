using UnityEngine;

public abstract class PlayerBaseState {

    private bool _isRootState = false;
    private bool _canSwitch = true;
    private PlayerStateMachine _ctx;
    private PlayerStateFactory _factory;
    private PlayerBaseState _currentSuperState;
    private PlayerBaseState _currentSubState;
    
    protected bool IsRootState { set => _isRootState = value; }
    public bool CanSwitch { get => _canSwitch; set => _canSwitch = value; }
    protected PlayerStateMachine Ctx { get => _ctx; }
    protected PlayerStateFactory Factory { get => _factory; }
    public PlayerBaseState CurrentSuperState { get => _currentSuperState; set => _currentSuperState = value; }
    public PlayerBaseState CurrentSubState { get => _currentSubState; set => _currentSubState = value; }

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
    /// Creates the initial substate of this state.
    /// </summary>
    public abstract void InitializeSubState();

    public void EnterStates() {
        EnterState();
        if (_currentSubState != null) {
            _currentSubState.EnterStates();
        }
    }
    
    /// <summary>
    /// Updates substates
    /// </summary>
    public void UpdateStates() {
        UpdateState();
        if (_currentSubState != null) {
            _currentSubState.UpdateStates();
        }
    }

    public void ExitStates() {
        ExitState();
        if (_currentSubState != null) {
            _currentSubState.ExitStates();
        }
    }
    
    protected void SwitchState(PlayerBaseState newState) {
        ExitStates();
        newState.EnterStates();
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
