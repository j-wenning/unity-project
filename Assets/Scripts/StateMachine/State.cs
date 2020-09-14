using System.Linq;
using System.Collections.Generic;

namespace _
{
    public class State
    {
        public StateQualifier[] StateQualifiers { get => m_StateQualifiers.ToArray(); }
        public string Name { get; private set; }
        private HashSet<StateTag> m_Tags;
        private HashSet<StateQualifier> m_StateQualifiers;
        private HashSet<State> m_Nodes;
        public State(string name = "", StateTag[] tags = null, StateQualifier[] stateQualifiers = null, params State[] nodes)
        {
            Name = name;
            m_Tags = new HashSet<StateTag>(tags ?? new StateTag[] { });
            m_StateQualifiers = new HashSet<StateQualifier>(stateQualifiers ?? new StateQualifier[] { });
            m_Nodes = new HashSet<State>(nodes);
        }

        public void LinkNode(State node)
        {
            m_Nodes.Add(node);
        }

        public void LinkNodes(params State[] nodes)
        {
            foreach (State node in nodes) LinkNode(node);
        }

        public bool HasTag(StateTag tag)
        {
            return m_Tags.Contains(tag);
        }

        public bool HasTags(StateTag[] tags)
        {
            foreach (StateTag tag in tags)
            {
                if (!HasTag(tag)) return false;
            }
            return true;
        }

        public State NextState(HashSet<StateQualifier> stateQualifiers)
        {
            foreach (State node in m_Nodes)
            {
                if (node.ValidateState(stateQualifiers)) return node;
            }
            return null;
        }

        public bool ValidateState(HashSet<StateQualifier> stateQualifiers)
        {
            return m_StateQualifiers.IsSubsetOf(stateQualifiers);
        }

        public override string ToString()
        {
            return $"{GetType()} ({Name})";
        }
    }
}
