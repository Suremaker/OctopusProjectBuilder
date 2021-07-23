﻿using System.Collections.Generic;

namespace OctopusProjectBuilder.Model
{
    public class SystemModelBuilder
    {
        private readonly List<MachinePolicy> _machinePolicies = new List<MachinePolicy>();
        private readonly List<Environment> _environments = new List<Environment>();
        private readonly List<ProjectGroup> _projectGroups = new List<ProjectGroup>();
        private readonly List<Project> _projects = new List<Project>();
        private readonly List<Channel> _channels = new List<Channel>();
        private readonly List<Lifecycle> _lifecycles = new List<Lifecycle>();
        private readonly List<LibraryVariableSet> _libraryVariableSets = new List<LibraryVariableSet>();
        private readonly List<UserRole> _userRoles = new List<UserRole>();
        private readonly List<Team> _teams = new List<Team>();
        private readonly List<Tenant> _tenants = new List<Tenant>();
        private readonly List<TagSet> _tagSets = new List<TagSet>();
        private readonly List<Runbook> _runbooks = new List<Runbook>();

        public SystemModelBuilder AddProjectGroup(ProjectGroup group)
        {
            _projectGroups.Add(group);
            return this;
        }

        public SystemModelBuilder AddEnvironment(Environment environment)
        {
            _environments.Add(environment);
            return this;
        }

        public SystemModelBuilder AddProject(Project project)
        {
            _projects.Add(project);
            return this;
        }

        public SystemModelBuilder AddChannel(Channel channel)
        {
            _channels.Add(channel);
            return this;
        }

        public SystemModelBuilder AddLifecycle(Lifecycle lifecycle)
        {
            _lifecycles.Add(lifecycle);
            return this;
        }

        public SystemModelBuilder AddLibraryVariableSet(LibraryVariableSet libraryVariableSet)
        {
            _libraryVariableSets.Add(libraryVariableSet);
            return this;
        }

        public SystemModelBuilder AddUserRole(UserRole userRole)
        {
            _userRoles.Add(userRole);
            return this;
        }

        public SystemModelBuilder AddTeam(Team team)
        {
            _teams.Add(team);
            return this;
        }

        public SystemModelBuilder AddMachinePolicy(MachinePolicy machinePolicy)
        {
            _machinePolicies.Add(machinePolicy);
            return this;
        }

        public SystemModelBuilder AddTenant(Tenant tenant)
        {
            _tenants.Add(tenant);
            return this;
        }

        public SystemModelBuilder AddTagSet(TagSet tagSet)
        {
            _tagSets.Add(tagSet);
            return this;
        }

        public SystemModelBuilder AddRunbook(Runbook runbook)
        {
            _runbooks.Add(runbook);
            return this;
        }

        public SystemModel Build()
        {
            return new SystemModel(
                _machinePolicies,
                _lifecycles,
                _projectGroups,
                _libraryVariableSets,
                _projects,
                _channels,
                _environments,
                _userRoles,
                _teams,
                _tenants, 
                _tagSets,
                _runbooks);
        }
    }
}