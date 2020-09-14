using System;

namespace _
{
    [AttributeUsage(AttributeTargets.All)]
    public class Alt : Attribute
    {
        private string val;

        public Alt(string val) { this.val = val; }

        public override string ToString() { return val; }
    }
}