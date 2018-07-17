using System;
using System.Linq;

namespace OctopusProjectBuilder.Model
{
	public class ProjectChannel
	{
		public ElementIdentifier Identifier { get; }
		public string Description { get; }
	
		public bool IsDefault { get;  }

		public ProjectChannel(ElementIdentifier identifier, string description, bool isDefault)
		{
			Description = description;
			IsDefault = isDefault;

			Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
		}
		public override string ToString()
		{
			return Identifier.ToString();
		}
	}
}