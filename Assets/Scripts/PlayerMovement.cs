using UnityEngine;
using System.Collections;

namespace _
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField]
        private PlayerBase m_Base;

        [SerializeField]
        private float m_MoveSpeed;
        [SerializeField, Tooltip("Minimum distance for a player to select to move (prevents jittering)")]
        private float m_MoveThreshold;
        [SerializeField, Range(0, 180), Tooltip("Maximum automatic change in trajectory before stopping the player")]
        private int m_StopAngle;
        [SerializeField, Tooltip("Minimum speed to not be considered \"stuck\"")]
        private float m_StuckTolerance;
        [SerializeField, Tooltip("Number of frames at \"Stuck Tolerance\" required to be determined as \"stuck\"")]
        private int m_StuckFrames;
        [SerializeField]
        private float m_DashSpeed;
        [SerializeField]
        private float m_DashLength;
        [SerializeField]
        private float m_DashCooldown;

        private Vector2 m_NewPos;
        private Vector2 m_PosDiff;
        private Vector2 m_OldPosDiff;
        private int m_StuckCount = 0;
        private bool m_IsMoving = false;
        private bool m_IsDashing = false;
        private bool m_CanDash = true;

        public void Init()
        {
            m_NewPos = m_Base.m_LocalPos;
        }

        public void Move()
        {
            m_PosDiff = m_NewPos - m_Base.m_LocalPos;
            m_IsMoving = Vector2.Angle(m_PosDiff, m_OldPosDiff) < m_StopAngle
                        && m_PosDiff.magnitude > Mathf.Epsilon;
            CheckStuck();
            m_IsDashing = m_IsDashing && m_IsMoving;
            m_OldPosDiff = m_PosDiff;
            if (!m_IsMoving || m_Base.m_State.HasTag(StateTag.Interruptor)) Halt();
            m_Base.m_Rigidbody.velocity = m_PosDiff.normalized *
                        (m_IsMoving ? (m_IsDashing ? m_DashSpeed : m_MoveSpeed) : 0);
            m_Base.m_Machine.ToggleQualifier(StateQualifier.Player_Walk, m_IsMoving && !m_IsDashing);
            m_Base.m_Machine.ToggleQualifier(StateQualifier.Player_Dash, m_IsDashing);
        }

        public void Halt()
        {
            m_NewPos = m_Base.m_LocalPos;
        }

        private void CheckStuck()
        {
            if (m_Base.m_Rigidbody.velocity.magnitude > m_StuckTolerance)
            {
                m_StuckCount = 0;
            }
            else if (++m_StuckCount > m_StuckFrames)
            {
                m_StuckCount = 0;
                m_IsMoving = false;
            }
        }

        public void GetMoveInput()
        {
            if (!m_Base.m_State.HasTag(StateTag.Interruptible)) return;
            if (m_CanDash && Input.GetButton("Fire3"))
            {
                m_Base.StartCoroutine(ApplyDashCooldown());
            }
            else if (Input.GetButton("Fire2") && (m_Base.m_MousePos - m_Base.m_LocalPos).magnitude > m_MoveThreshold)
            {
                m_NewPos = m_Base.m_MousePos;
            }
        }

        private IEnumerator ApplyDashCooldown()
        {
            m_NewPos = m_Base.m_LocalPos + (m_Base.m_MousePos - m_Base.m_LocalPos).normalized * m_DashLength;
            m_PosDiff = m_OldPosDiff = Vector2.zero;
            m_IsDashing = true;
            m_CanDash = false;
            yield return new WaitForSeconds(m_DashCooldown);
            m_CanDash = true;
        }
    }
}
