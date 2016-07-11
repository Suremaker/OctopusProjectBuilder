using System;
using System.ComponentModel;
using OctopusProjectBuilder.Model;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Serializable]
    [Description("Project versioning strategy.")]
    public class YamlVersioningStrategy
    {
        [Description("Versioning template")]
        [YamlMember(Order = 1)]
        public string Template { get; set; }

        public static YamlVersioningStrategy FromModel(VersioningStrategy model)
        {
            return model == null
                ? null
                : new YamlVersioningStrategy { Template = model.Template };
        }

        public VersioningStrategy ToModel()
        {
            return new VersioningStrategy(Template);
        }
    }
}