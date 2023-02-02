using System;
using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class FSM_StateTransition
{
    public FSM_IState To { get; }
    public Func<bool> Condition { get; }

    public FSM_StateTransition(FSM_IState to, Func<bool> condition)
    {
        To = to;
        Condition = condition;
    }
}
