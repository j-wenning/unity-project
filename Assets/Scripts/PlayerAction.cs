using UnityEngine;
using System.Collections;

namespace _
{
    public class PlayerAction : MonoBehaviour
    {
        [SerializeField]
        private PlayerBase m_Base;
        [SerializeField]
        private float m_BasicAttackStartup;
        [SerializeField]
        private float m_BasicAttackDuration;
        [SerializeField]
        private float m_BasicAttackCooldown;
        [SerializeField]
        private float m_BasicAttackTolerance;

        private IEnumerator m_BasicAttackComboCooldownCB;
        private bool m_CanBasicAttack = true;
        private int m_BasicAttackCombo = 0;
        private int m_BasicAttackCount = 0;

        public void Init()
        {
            m_BasicAttackCount = StateQualifier.Player_Attack_Basic_End - StateQualifier.Player_Attack_Basic_0;
        }

        public void TryBasicAttack()
        {
            if (m_CanBasicAttack
            && Input.GetButton("Fire1")
            && m_Base.m_State.HasTag(StateTag.Interruptible))
            {
                StopCoroutine(m_BasicAttackComboCooldownCB);
                StartCoroutine(m_BasicAttackComboCooldownCB = ApplyBasicAttackComboCooldown());
            }
        }

        private IEnumerator ApplyBasicAttackComboCooldown()
        {
            m_CanBasicAttack = false;
            Debug.Log("Basic Attack Start");
            m_Base.m_Machine.SetQualifiers(StateQualifier.Player_Attack_Basic, StateQualifier.Player_Attack_Basic_0 + m_BasicAttackCombo);
            yield return new WaitForSeconds(m_BasicAttackStartup);
            // enable hitbox
            yield return new WaitForSeconds(m_BasicAttackDuration);
            // disable hitbox
            Debug.Log("Basic Attack End");
            m_Base.m_Machine.UnsetQualifiers(StateQualifier.Player_Attack_Basic, StateQualifier.Player_Attack_Basic_0 + m_BasicAttackCombo);
            yield return new WaitForSeconds(m_BasicAttackCooldown);
            m_CanBasicAttack = true;
            if (m_BasicAttackCombo++ < m_BasicAttackCount)
            {
                yield return new WaitForSeconds(m_BasicAttackTolerance);
            }
            m_BasicAttackCombo = 0;
        }
    }
}
