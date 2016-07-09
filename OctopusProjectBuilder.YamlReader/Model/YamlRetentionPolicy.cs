using System;
using System.ComponentModel;
using OctopusProjectBuilder.Model;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Serializable]
    public class YamlRetentionPolicy
    {
        [YamlMember(Order = 1)]
        [DefaultValue(-1)] // To force rendering 0
        public int QuantityToKeep { get; set; }
        [YamlMember(Order = 2)]
        public RetentionPolicy.RetentionUnit Unit { get; set; }

        public static YamlRetentionPolicy FromModel(RetentionPolicy model)
        {
            if (model == null)
                return null;

            return new YamlRetentionPolicy
            {
                Unit = model.Unit,
                QuantityToKeep = model.QuantityToKeep
            };
        }

        public RetentionPolicy ToModel()
        {
            return new RetentionPolicy(QuantityToKeep, Unit);
        }
    }
}