using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public interface FSM_IState
    {

        public void OnEnter();
        public void OnExit();
        public void OnUpdate();
    }
}
