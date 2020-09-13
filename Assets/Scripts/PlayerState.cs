using UnityEngine;

public enum PlayerStateList
{
    Idle,
    Walk,
    Dash
}

public class PlayerState : MonoBehaviour
{
    [SerializeField]
    private PlayerBase m_Base;

    public StateMachine Machine { get; private set; }

    void Awake()
    {
        Machine = new StateMachine(
            new State ("Idle", new string[] { "interruptible" }, null, new State[]
                {
                    new State ("Walk", new string[] { "interruptible" }, (int arg) => arg == (int)PlayerStateList.Walk),
                    new State ("Dash", null, (int arg) => arg == (int)PlayerStateList.Dash),

                }
            ), m_Base.m_Animator);
    }
}
