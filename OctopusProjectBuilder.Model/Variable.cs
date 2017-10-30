using System;
using System.Collections.Generic;
using System.Linq;

namespace OctopusProjectBuilder.Model
{
    public enum VariableScopeType
    {
        Environment = 1,
        Machine = 2,
        Role = 3,
        Action = 5,
        Channel = 8,
        TenantTag = 9
    }

    public class Variable
    {
        public bool IsEditable { get; }
        public bool IsSensitive { get; }
        public string Name { get; }
        public string Value { get; }
        public VariablePrompt Prompt { get; }
        public IReadOnlyDictionary<VariableScopeType, IEnumerable<ElementReference>> Scope { get; }

        public Variable(string name, bool isEditable, bool isSensitive, string value, IReadOnlyDictionary<VariableScopeType, IEnumerable<ElementReference>> scope, VariablePrompt prompt)
        {
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));
            Name = name;
            IsEditable = isEditable;
            IsSensitive = isSensitive;
            Value = value;
            Scope = scope.ToDictionary(kv => kv.Key, kv => kv.Value.ToArray().AsEnumerable());
            Prompt = prompt;
        }

        public override string ToString()
        {
            return $"{Name}={Value}";
        }
    }
}