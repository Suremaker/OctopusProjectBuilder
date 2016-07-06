using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.YamlReader.Helpers;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Serializable]
    public class YamlVariableScope
    {
        [DefaultValue(null)]
        public string[] RoleRefs { get; set; }
        [DefaultValue(null)]
        public string[] MachineRefs { get; set; }
        [DefaultValue(null)]
        public string[] EnvironmentRefs { get; set; }
        [DefaultValue(null)]
        public string[] ChannelRefs { get; set; }
        [DefaultValue(null)]
        public string[] ActionRefs { get; set; }

        public IReadOnlyDictionary<VariableScopeType, IEnumerable<ElementReference>> ToModel()
        {
            var result = new Dictionary<VariableScopeType, IEnumerable<ElementReference>>();
            Add(result, VariableScopeType.Role, RoleRefs);
            Add(result, VariableScopeType.Machine, MachineRefs);
            Add(result, VariableScopeType.Environment, EnvironmentRefs);
            Add(result, VariableScopeType.Channel, ChannelRefs);
            Add(result, VariableScopeType.Action, ActionRefs);
            return result;
        }

        private void Add(Dictionary<VariableScopeType, IEnumerable<ElementReference>> result, VariableScopeType type, string[] values)
        {
            if (values == null || values.Length == 0)
                return;
            result.Add(type, values.Select(name => new ElementReference(name)).ToArray());
        }

        public static YamlVariableScope FromModel(IReadOnlyDictionary<VariableScopeType, IEnumerable<ElementReference>> model)
        {
            if (!model.Any())
                return null;
            return new YamlVariableScope
            {
                ActionRefs = model.Where(kv => kv.Key == VariableScopeType.Action).SelectMany(kv => kv.Value).Select(r => r.Name).ToArray().NullIfEmpty(),
                ChannelRefs = model.Where(kv => kv.Key == VariableScopeType.Channel).SelectMany(kv => kv.Value).Select(r => r.Name).ToArray().NullIfEmpty(),
                EnvironmentRefs = model.Where(kv => kv.Key == VariableScopeType.Environment).SelectMany(kv => kv.Value).Select(r => r.Name).ToArray().NullIfEmpty(),
                MachineRefs = model.Where(kv => kv.Key == VariableScopeType.Machine).SelectMany(kv => kv.Value).Select(r => r.Name).ToArray().NullIfEmpty(),
                RoleRefs = model.Where(kv => kv.Key == VariableScopeType.Role).SelectMany(kv => kv.Value).Select(r => r.Name).ToArray().NullIfEmpty(),
            };
        }
    }
}