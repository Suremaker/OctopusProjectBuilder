using System;
using System.ComponentModel;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model.Templates
{
    [Serializable]
    public class YamlTemplateReference
    {
        [YamlMember(Order = 1)]
        public string Name { get; set; }
        [YamlMember(Order = 2)]
        public YamlTemplateParameter[] Parameters { get; set; }
    }
}