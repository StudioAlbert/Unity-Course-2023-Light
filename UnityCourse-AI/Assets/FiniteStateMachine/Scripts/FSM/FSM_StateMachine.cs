using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class FSM_StateMachine
    {
        private readonly Dictionary<Type,  List<FSM_StateTransition>> _allTransitions = new Dictionary<Type, List<FSM_StateTransition>>();
        private List<FSM_StateTransition> _currentTypeTransitions = new List<FSM_StateTransition>();
        private readonly List<FSM_StateTransition> _emptyTransitions = new List<FSM_StateTransition>(0);
        
        private FSM_IState _currentState;
        
        public void OnUpdate()
        {
            FSM_StateTransition transition = GetValidTransition();
            if (transition != null)
                SetState(transition.To);

            if(_currentState != null)
                _currentState.OnUpdate();
            
        }

        private FSM_StateTransition GetValidTransition()
        {
            foreach (var t in _currentTypeTransitions)
                if (t.Condition()) 
                    return t;

            return null;

        }

        public void SetState(FSM_IState newState)
        {
            if (newState != _currentState)
            {
                _currentState?.OnExit();
                _currentState = newState;

                if(_allTransitions.TryGetValue(newState.GetType(), out _currentTypeTransitions) == false)
                    _currentTypeTransitions = _emptyTransitions;
                    
                _currentState?.OnEnter();
            }
        }

        public void AddTransition(FSM_IState from, FSM_IState to, Func<bool> condition)
        {
            if (_allTransitions.TryGetValue(from.GetType(), out List<FSM_StateTransition> transitionsToAdd) == false)
            {
                // First call for this type => create a new list, and add it in the dictionnary
                transitionsToAdd = new List<FSM_StateTransition>();
                _allTransitions[from.GetType()] = transitionsToAdd;
            }
            
            // Add to the list of the type
            transitionsToAdd.Add(new FSM_StateTransition(to, condition));
        }
        
    }
}
