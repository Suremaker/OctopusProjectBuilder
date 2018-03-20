using System;
using System.Linq;

namespace OctopusProjectBuilder.Uploader
{
	public class Identifier : IEquatable<Identifier>, IEquatable<string>
	{
		readonly string id;

		Identifier(string id)
		{
			this.id = id ?? throw new ArgumentNullException(nameof(id));
		}

		public bool Equals(Identifier other)
		{
			if (other == null)
				return false;
			return Equals(other.id);
		}

		public bool Equals(string other)
		{
			if (string.IsNullOrEmpty(other))
				return false;
			return other.Equals(id, StringComparison.InvariantCultureIgnoreCase);
		}

		public override int GetHashCode()
		{
			return id.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			var identifier = obj as Identifier;
			if (identifier == null)
				return false;
			return Equals(identifier);
		}

		public static implicit operator string(Identifier identifier)
		{
			return identifier.id;
		}

		public static implicit operator Identifier(string id)
		{
			return new Identifier(id);
		}

		public override string ToString()
		{
			return id;
		}
	}
}
