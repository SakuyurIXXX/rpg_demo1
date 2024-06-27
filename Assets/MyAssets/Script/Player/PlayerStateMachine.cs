using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    /// <summary>
    /// ÓÃÀ´ÇÐ»»×´Ì¬
    /// </summary>
    public PlayerState currentState { get; private set; }

    public void Initialize(PlayerState _startState)
    {
        currentState = _startState;
        currentState.Enter();

    }

    public void ChangeState(PlayerState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }

    public PlayerState getCurrentState()
    {
        return currentState;
    }
}
