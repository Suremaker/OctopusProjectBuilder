using System.Collections.Generic;
using System.Linq;
using Octopus.Client.Repositories;

namespace OctopusProjectBuilder.Uploader.Tests.Helpers
{
    internal class FakeMachineRoleRepository : IMachineRoleRepository
    {
        private readonly List<string> _roles = new List<string>();

        public List<string> GetAllRoleNames()
        {
            return _roles.ToList();
        }

        public void Add(string name)
        {
            _roles.Add(name);
        }
    }
}