using System;
using System.ComponentModel;


using OctopusProjectBuilder.Model;

using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
	[Description("Octopus Project channel model.")]
	[Serializable]
	public class YamlProjectChannel : YamlNamedElement
	{
		[Description("Description")]
		[YamlMember(Order = 3)]
		public string Description { get; set; }

		[Description("IsDefault")]
		[YamlMember(Order = 4)]
		public bool IsDefault { get; set; }

		public ProjectChannel ToModel()
		{
			return new ProjectChannel(ToModelName(), Description, IsDefault);
		}

		public static YamlProjectChannel FromModel(ProjectChannel model)
		{
			return new YamlProjectChannel
			{
				Name = model.Identifier.Name,
				Description =  model.Description,
				IsDefault = model.IsDefault
			};
		}
	}
}
