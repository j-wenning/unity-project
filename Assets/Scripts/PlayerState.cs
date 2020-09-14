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
        private State m_Attack_Basic = new State("Attack_Basic", new StateTag[] { StateTag.Interruptor }, new StateQualifier[] { StateQualifier.Player_Attack_Basic });
        private State m_Attack_Basic_0 = new State("Attack_Basic_0", null, new StateQualifier[] { StateQualifier.Player_Attack_Basic_0 });
        private State m_Attack_Basic_1 = new State("Attack_Basic_1", null, new StateQualifier[] { StateQualifier.Player_Attack_Basic_1 });
        private State m_Attack_Basic_2 = new State("Attack_Basic_2", null, new StateQualifier[] { StateQualifier.Player_Attack_Basic_2 });

        public void Init()
        {
            m_Idle.LinkNodes(m_Walk, m_Dash, m_Attack_Basic);
            m_Walk.LinkNodes(m_Dash, m_Attack_Basic);
            m_Attack_Basic.LinkNodes(m_Attack_Basic_0, m_Attack_Basic_1, m_Attack_Basic_2);
            m_Attack_Basic_0.LinkNode(m_Attack_Basic_1);
            m_Attack_Basic_1.LinkNode(m_Attack_Basic_2);

            Machine = new StateMachine(m_Idle, m_Base.m_Animator);
        }
    }
}
