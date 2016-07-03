using System.ComponentModel;
using System.Linq;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.YamlReader.Helpers;

namespace OctopusProjectBuilder.YamlReader.Model
{
    public class YamlVariableSet
    {
        [DefaultValue(null)]
        public YamlVariable[] Variables { get; set; }

        public static YamlVariableSet FromModel(VariableSet model)
        {
            return new YamlVariableSet
            {
                Variables = model.Variables.Select(YamlVariable.FromModel).ToArray().NullIfEmpty()
            };
        }

        public VariableSet ToModel()
        {
            return new VariableSet(Variables.EnsureNotNull().Select(v => v.ToModel()));
        }
    }
}