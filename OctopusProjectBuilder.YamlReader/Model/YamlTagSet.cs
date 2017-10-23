using System;
using System.ComponentModel;
using System.Linq;
using OctopusProjectBuilder.Model;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Serializable]
    [Description("TagSet model.")]
    public class YamlTagSet : YamlNamedElement
    {
        [Description("List of tags")]
        [YamlMember(Order = 3)]
        public string[] Tags { get; set; }

        public TagSet ToModel()
        {
            return new TagSet(ToModelName(), Tags);
        }

        public static YamlTagSet FromModel(TagSet model)
        {
            return new YamlTagSet
            {
                 Tags = model.Tags.ToArray(),
                 Name = model.Identifier.Name,
                 RenamedFrom = model.Identifier.RenamedFrom
            };
        }
    }
}
