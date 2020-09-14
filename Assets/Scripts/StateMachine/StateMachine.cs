using System.Reflection;
using System.Collections.Generic;

namespace _
{
    public class StateMachine
    {
        public State CurrentState { get => m_States.Peek(); }
        private HashSet<StateQualifier> m_StateQualifiers = new HashSet<StateQualifier>();
        private HashSet<StateQualifier> m_RemovedQualifiers = new HashSet<StateQualifier>();
        private State m_NextState;
        private Stack<State> m_States = new Stack<State>();
        private UnityEngine.Animator m_Animator;

        public StateMachine(State initialState, UnityEngine.Animator animator = null)
        {
            m_States.Push(initialState);
            SetQualifiers(initialState.StateQualifiers);
            m_Animator = animator;
        }

        public State CheckForUpdate()
        {
            m_NextState = CurrentState.NextState(m_StateQualifiers);
            if (m_NextState != null)
            {
                m_States.Push(m_NextState);
            }
            else if (!CurrentState.ValidateState(m_StateQualifiers))
            {
                m_States.Pop();
            }
            if (m_Animator != null)
            {
                foreach (StateQualifier qualifier in m_StateQualifiers)
                {
                    m_Animator.SetBool(ParseQualifier(qualifier), true);
                }
                foreach (StateQualifier qualifier in m_RemovedQualifiers)
                {
                    m_Animator.SetBool(ParseQualifier(qualifier), false);
                }
            }
            m_RemovedQualifiers.Clear();
            return CurrentState;
        }

        private string ParseQualifier(StateQualifier qualifier)
        {
            return (typeof(StateQualifier)
                    .GetMember(qualifier.ToString())[0]
                    .GetCustomAttribute(typeof(Alt)) as object ?? qualifier)
                    .ToString();
        }

        public void ToggleQualifier(StateQualifier qualifier, bool enabled)
        {
            if (enabled) m_StateQualifiers.Add(qualifier);
            else
            {
                m_StateQualifiers.Remove(qualifier);
                m_RemovedQualifiers.Add(qualifier);
            }
        }

        public void SetQualifiers(params StateQualifier[] qualifiers)
        {
            foreach (StateQualifier qualifier in qualifiers) ToggleQualifier(qualifier, true);
        }

        public void UnsetQualifiers(params StateQualifier[] qualifiers)
        {
            foreach (StateQualifier qualifier in qualifiers) ToggleQualifier(qualifier, false);
        }
    }
}
