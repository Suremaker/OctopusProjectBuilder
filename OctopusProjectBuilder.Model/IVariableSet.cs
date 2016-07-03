using System.Collections.Generic;

namespace OctopusProjectBuilder.Model
{
    public interface IVariableSet
    {
        IEnumerable<Variable> Variables { get; }
    }
}