using UnityEngine;
using System.Collections.Generic;

public delegate bool StateConditional(int arg);

public class State
{
    public string name { get; private set; }
    private Dictionary<string, bool> m_Tags;
    private StateConditional m_Validate;
    private State[] m_Nodes;
    
    public State(string name = "", string[] tags = null, StateConditional validate = null, State[] nodes = null)
    {
        HashSet<string> set = new HashSet<string>(tags);
        this.name = name;
        m_Tags = new Dictionary<string, bool>();
        foreach (string tag in set) m_Tags.Add(tag, true);
        if (validate == null) m_Validate = (int arg) => true;
        else m_Validate = validate;
        m_Nodes = nodes;
    }

    public bool HasTag(string tag) { return m_Tags.ContainsKey(tag); }

    public bool HasTags(string[] tags)
    {
        foreach (string tag in tags) if (!HasTag(tag)) return false;
        return true;
    }

    public State NextState(int arg)
    {
        foreach(State node in m_Nodes)
        {
            if (node.m_Validate(arg)) return node;
        }
        return null;
    }
}

public enum StateMachineBehaviour
{
    Default,
    ResetRoot,
    ResetOne,
    ThrowError
}

public class StateMachine
{
    public State CurrentState { get => m_States.Peek();}
    private State m_RootState;
    private State m_NextState;
    private State m_PrevState;
    private Stack<State> m_States = new Stack<State>();
    private Animator m_Animator;
    private StateMachineBehaviour m_ErrorBehaviour;

    public StateMachine(State rootState, Animator animator = null, StateMachineBehaviour errorBehaviour = StateMachineBehaviour.ThrowError) 
    { 
        m_RootState = rootState;
        m_States.Push(rootState);
        m_Animator = animator;
        m_ErrorBehaviour = errorBehaviour == StateMachineBehaviour.Default
            ? StateMachineBehaviour.ThrowError
            : errorBehaviour;
    }

    public void NextState(int arg, StateMachineBehaviour errorBehaviour = StateMachineBehaviour.Default)
    {
        m_NextState = CurrentState.NextState(arg);
        if (m_NextState != null)
        {
            m_PrevState = CurrentState;
            m_States.Push(m_NextState);
            UpdateAnimator();
            return;
        }
        if (errorBehaviour == StateMachineBehaviour.Default) errorBehaviour = m_ErrorBehaviour;
        switch (errorBehaviour)
        {
            case StateMachineBehaviour.ResetRoot:
                ResetState();
                return;
            case StateMachineBehaviour.ResetOne:
                PrevState();
                return;
            default:
            case StateMachineBehaviour.ThrowError:
                throw new System.ArgumentException($"Arg \"{arg}\" is invalid for method \"Next State\" of State \"{CurrentState.name}\".");
        }
    }

    public void PrevState()
    {
        if (CurrentState == m_RootState) return;
        m_PrevState = m_States.Pop();
        UpdateAnimator();
    }

    public void ResetState()
    {
        m_States.Clear();
        m_States.Push(m_RootState);
        m_PrevState = m_RootState;
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        if (m_Animator == null) return;
        m_Animator.SetBool(m_PrevState.name, false);
        m_Animator.SetBool(CurrentState.name, true);
    }
}
