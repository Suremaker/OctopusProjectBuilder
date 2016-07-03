using System.Collections.Generic;
using System.Linq;

namespace OctopusProjectBuilder.Model
{
    public class VariableSet
    {
        public IEnumerable<Variable> Variables { get; }

        public VariableSet(IEnumerable<Variable> variables)
        {
            Variables = variables.ToArray();
        }
    }
}