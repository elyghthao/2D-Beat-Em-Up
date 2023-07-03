using UnityEngine;

public abstract class EnemyBaseState
{
    private bool _isRootState = false;
    private bool _canSwitch = true;
    private EnemyStateMachine _ctx;
    private EnemyStateFactory _factory;
    private EnemyBaseState _currentSuperState;
    private EnemyBaseState _currentSubState;
    
    protected bool IsRootState { set => _isRootState = value; }
    public bool CanSwitch { get => _canSwitch; set => _canSwitch = value; }
    protected EnemyStateMachine Ctx { get => _ctx; }
    protected EnemyStateFactory Factory { get => _factory; }
    public EnemyBaseState CurrentSuperState { get => _currentSuperState; set => _currentSuperState = value; }
    public EnemyBaseState CurrentSubState { get => _currentSubState; set => _currentSubState = value; }

    public EnemyBaseState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) {
        _ctx = currentContext;
        _factory = enemyStateFactory;
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
    
    protected void SwitchState(EnemyBaseState newState) {
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

    protected void SetSuperState(EnemyBaseState newSuperState) {
        _currentSuperState = newSuperState;
    }

    protected void SetSubState(EnemyBaseState newSubState) {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
}
