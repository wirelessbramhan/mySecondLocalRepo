using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GameStateKey
{
    menu,
    gameplay,
    results,
    exit
}

[System.Serializable]
public class GameState
{
    public string Name;
    public GameStateKey Key;
    public bool _active, _enableUpdate;

    public UnityEvent OnStateEnter, OnStateExit;

    public virtual void EnterState()
    {
        _active = true;
        OnStateEnter.Invoke();
    }

    public virtual void UpdateState()
    {
        
    }

    public virtual void ExitState()
    {
        _active = false;
        OnStateExit.Invoke();
    }
}

public class GameStateManager : MonoBehaviour
{
    [SerializeField]
    private List<GameState> _allStates;
    [SerializeField]
    private GameState _current;
    [SerializeField]
    private bool _changingStates;
    [SerializeField]
    private int _currentIndex;

    private void OnValidate()
    {
        if (_allStates.Count > 0 && _allStates[_currentIndex] != null)
        {
            foreach (GameState state in _allStates)
            {
                state.Name = state.Key.ToString();
            }
        }
    }

    private void Start()
    {
        _currentIndex = 0;

        if (_allStates.Count > 0 && _allStates[_currentIndex] != null)
        {
            _current = _allStates[_currentIndex];
        }

        _current.EnterState();
    }

    private void Update()
    {
        if (!_changingStates)
        {
            if (_current._enableUpdate)
            {
                _current.UpdateState();
            }
        }
    }

    public void ChangeStateNext()
    {
        _currentIndex++;
        ChangeState(_currentIndex);
    }

    private void ChangeState(int index)
    {
        if (_allStates[index] != null)
        {
            _changingStates = true;

            _current.ExitState();
            _current = _allStates[index];
            _current.EnterState();

            _changingStates = false;
        }

        Debug.Log("Gamestate changed to : " + _current);
    }
}
