using UnityEngine;

namespace _
{
    public class PlayerState : MonoBehaviour
    {
        [SerializeField]
        private PlayerBase m_Base;
    
        public StateMachine Machine { get; private set; }
        private State m_Idle = new State("Idle", new StateTag[] { StateTag.Interruptible }, new StateQualifier[] { StateQualifier.Player_Idle });
        private State m_Walk = new State("Walk", new StateTag[] { StateTag.Interruptible }, new StateQualifier[] { StateQualifier.Player_Walk });
        private State m_Dash = new State("Dash", null, new StateQualifier[] { StateQualifier.Player_Dash });

        public void Init()
        {
            m_Idle.LinkNodes(m_Walk, m_Dash);
            m_Walk.LinkNode(m_Dash);
            Machine = new StateMachine(m_Idle, m_Base.m_Animator);
        }
    }
}
