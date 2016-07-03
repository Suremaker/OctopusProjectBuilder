using Octopus.Client;
using Octopus.Client.Repositories;

namespace OctopusProjectBuilder.Uploader.Tests.Helpers
{
    class FakeOctopusRepository : IOctopusRepository
    {
        public FakeOctopusRepository()
        {
            var fakeVariableSetRepository = new FakeVariableSetRepository();
            var fakeDeploymentProcessRepository = new FakeDeploymentProcessRepository();

            DeploymentProcesses = fakeDeploymentProcessRepository;
            ProjectGroups = new FakeProjectGroupRepository();
            VariableSets = fakeVariableSetRepository;
            LibraryVariableSets = new FakeLibraryVariableSetRepository(fakeVariableSetRepository);
            Projects = new FakeProjectRepository(fakeVariableSetRepository, fakeDeploymentProcessRepository);
            Lifecycles = new FakeLifecycleRepository();
            Environments = new FakeEnvironmentRepository();
            MachineRoles = FakeMachineRoles = new FakeMachineRoleRepository();
            Machines = new FakeMachineRepository();
        }

        public IOctopusClient Client { get; }
        public IArtifactRepository Artifacts { get; }
        public ICertificateRepository Certificates { get; }
        public IBackupRepository Backups { get; }
        public IBuiltInPackageRepositoryRepository BuiltInPackageRepository { get; }
        public IDashboardConfigurationRepository DashboardConfigurations { get; }
        public IDashboardRepository Dashboards { get; }
        public IDeploymentProcessRepository DeploymentProcesses { get; }
        public IDeploymentRepository Deployments { get; }
        public IEnvironmentRepository Environments { get; }
        public IEventRepository Events { get; }
        public IFeedRepository Feeds { get; }
        public IInterruptionRepository Interruptions { get; }
        public ILibraryVariableSetRepository LibraryVariableSets { get; }
        public ILifecyclesRepository Lifecycles { get; }
        public IMachineRepository Machines { get; }
        public IMachineRoleRepository MachineRoles { get; }
        public IProjectGroupRepository ProjectGroups { get; }
        public IProjectRepository Projects { get; }
        public IReleaseRepository Releases { get; }
        public IServerStatusRepository ServerStatus { get; }
        public ITaskRepository Tasks { get; }
        public ITeamsRepository Teams { get; }
        public IUserRepository Users { get; }
        public IUserRolesRepository UserRoles { get; }
        public IVariableSetRepository VariableSets { get; }
        public IChannelRepository Channels { get; }
        public IAccountRepository Accounts { get; }
        public IRetentionPolicyRepository RetentionPolicies { get; }
        public IDefectsRepository Defects { get; }
        public IOctopusServerNodeRepository OctopusServerNodes { get; }
        public FakeMachineRoleRepository FakeMachineRoles { get; }
    }
}
