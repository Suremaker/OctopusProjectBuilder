using System;
using System.Collections.Generic;
using System.Linq;

namespace OctopusProjectBuilder.YamlReader.Model.Templates
{
    [Serializable]
    public class YamlTemplateParameter
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}