using UnityEngine;

/// <summary>
/// Base abstract state for all enemy states to derive from. Contains the necessary abstract functions for that define
/// and give functionality to enemy states
/// </summary>
public abstract class EnemyBaseState
{
    private bool _isRootState = false;
    private bool _canSwitch = true;
    private EnemyStateMachine _ctx;
    private EnemyStateFactory _factory;
    private EnemyBaseState _currentSuperState;
    private EnemyBaseState _currentSubState;
    private PlayerStateMachine _currentPlayerMachine;   // Added by Abdul
    
    /// <summary>
    /// Whether the current state will be stored in the context files currentState variable. DO NOT switch from a root
    /// state to a substate.
    /// </summary>
    protected bool IsRootState { set => _isRootState = value; }
    /// <summary>
    /// Whether a state is ready to be switched with another state. Defaults to true, but set false in constructor if
    /// you wish to use this
    /// </summary>
    public bool CanSwitch { get => _canSwitch; set => _canSwitch = value; }
    /// <summary>
    /// Context file, contains important information relevant to all states
    /// </summary>
    protected EnemyStateMachine Ctx { get => _ctx; }
    /// <summary>
    /// Factory class that allows for easy creation of new states
    /// </summary>
    protected EnemyStateFactory Factory { get => _factory; }
    /// <summary>
    /// Superstate of the current state
    /// </summary>
    public EnemyBaseState CurrentSuperState { get => _currentSuperState; set => _currentSuperState = value; }
    /// <summary>
    /// Substate of the current state
    /// </summary>
    public EnemyBaseState CurrentSubState { get => _currentSubState; set => _currentSubState = value; }
    /// <summary>
    /// PlayerStateMachine, added by Abdul
    /// </summary>
    public PlayerStateMachine CurrentPlayerMachine { get => _currentPlayerMachine; set => _currentPlayerMachine = value; }

    /// <summary>
    /// Default constructor for any enemy state
    /// </summary>
    /// <param name="currentContext">Context file for states to access relevant information from</param>
    /// <param name="enemyStateFactory">Factory class for creating new states</param>
    public EnemyBaseState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) {
        _ctx = currentContext;
        _factory = enemyStateFactory;
        _currentPlayerMachine = currentContext._currentPlayerMachine;
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
    /// Fixed update
    /// </summary>
    public abstract void FixedUpdateState();

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

    /// <summary>
    /// Enter state helper function. Calls the current states EnterState() and checks to see if any substates also need
    /// their EnterState() called
    /// </summary>
    public void EnterStates() {
        EnterState();
        if (_currentSubState != null) {
            _currentSubState.EnterStates();
        }
    }
    
    /// <summary>
    /// Update state helper function. Calls the current states UpdateState() and checks to see if any substates also need
    /// their UpdateState() called
    /// </summary>
    public void UpdateStates() {
        if (_currentSubState != null) {
            _currentSubState.UpdateStates();
        }
        UpdateState();
    }

    /// <summary>
    /// Fixed Update state helper function. Calls the current states FixedUpdateState() and checks to see if any substates also need
    /// their FixedUpdateState() called
    /// </summary>
    public void FixedUpdateStates() {
        if (_currentSubState != null) {
            _currentSubState.FixedUpdateStates();
        }
        FixedUpdateState();
    }

    /// <summary>
    /// Exit state helper function. Calls the current states ExitState() and checks to see if any substates also need
    /// their ExitState() called
    /// </summary>
    public void ExitStates() {
        // Exits the substate if this state contains one
        if (_currentSubState != null) {
            _currentSubState.ExitStates();
            _currentSubState = null;
        }
        ExitState();
    }
    
    /// <summary>
    /// Switches the active state. Swaps out the context files currentState if switching from a root state. Otherwise
    /// swaps out the superstates substate with the newState
    /// </summary>
    /// <param name="newState"></param>
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
    
    /// <summary>
    /// Added by Abdul to make it so sub states can switch to base states.
    /// </summary>
    /// <param name="newState" name="switchingFromSub"></param>
    protected void SwitchState(EnemyBaseState newState, bool switchingFromSub) {
        if (!switchingFromSub) {
            SwitchState(newState);
            return;
        }

        // Switching from a base state to a root state !!!
        if (_isRootState) {
            // Its just a root state to root state so regular switch
            SwitchState(newState);
            return;
        }
        // Switching from sub state to base state
        ExitStates();
        newState.EnterStates();
        _ctx.CurrentState = newState;
    }

    /// <summary>
    /// Sets the superstate of the current substate. This is more so a helper function, and you typically wouldn't call
    /// this manually
    /// </summary>
    /// <param name="newSuperState"></param>
    protected void SetSuperState(EnemyBaseState newSuperState) {
        _currentSuperState = newSuperState;
    }

    /// <summary>
    /// Sets the substate of the current state.
    /// </summary>
    /// <param name="newSubState"></param>
    protected void SetSubState(EnemyBaseState newSubState) {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
}
