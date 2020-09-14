using UnityEngine;

namespace _
{
    public class PlayerBase : MonoBehaviour
    {
        public PlayerState m_PlayerState;
        public PlayerMovement m_Movement;
        public PlayerAction m_Action;
        public Rigidbody2D m_Rigidbody;
        public Collider2D m_Collider;
        public Animator m_Animator;

        [HideInInspector]
        public StateMachine m_Machine;
        [HideInInspector]
        public State m_State;
        [HideInInspector]
        public Vector2 m_LocalPos;
        [HideInInspector]
        public Vector2 m_MousePos;
        [HideInInspector]
        public float m_DeltaTime;
        [HideInInspector]
        public float m_FixedTime;

        private void Awake()
        {
            m_PlayerState.Init();
            m_Machine = m_PlayerState.Machine;
        }

        private void FixedUpdate()
        {
            m_FixedTime = Time.fixedTime;
            m_DeltaTime = Time.fixedDeltaTime;
            m_LocalPos = transform.localPosition;
            m_MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            m_State = m_PlayerState.Machine.CheckForUpdate();
            m_Movement.GetMoveInput();
            m_Movement.Move();
            m_Action.TryBasicAttack();

        }
    }
}
