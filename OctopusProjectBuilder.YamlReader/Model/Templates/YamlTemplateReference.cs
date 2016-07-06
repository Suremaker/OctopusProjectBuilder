using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Yaml;
using System.Yaml.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model.Templates
{
    [Serializable]
    public class YamlTemplateReference
    {
        public string Name { get; set; }
        [DefaultValue(null)]
        public YamlTemplateParameter[] Parameters { get; set; }
    }
}