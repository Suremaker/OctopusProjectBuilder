﻿using System.Threading.Tasks;
using Octopus.Client;
using Octopus.Client.Model;
using Octopus.Client.Repositories.Async;

namespace OctopusProjectBuilder.Uploader
{
    public class FakeOctopusRepository : IOctopusAsyncRepository
    {
        public FakeOctopusRepository()
        {
            var fakeVariableSetRepository = new FakeVariableSetRepository();
            var fakeDeploymentProcessRepository = new FakeDeploymentProcessRepository();
            var fakeProjectTriggersRepository = new FakeProjectTriggersRepository();
            var fakeOctopusClient = new FakeOctopusClient();

            MachinePolicies = new FakeMachinePolicyRepository();
            DeploymentProcesses = fakeDeploymentProcessRepository;
            ProjectGroups = new FakeProjectGroupRepository();
            VariableSets = fakeVariableSetRepository;
            LibraryVariableSets = new FakeLibraryVariableSetRepository(fakeVariableSetRepository);
            Projects = new FakeProjectRepository(fakeVariableSetRepository, fakeDeploymentProcessRepository, fakeProjectTriggersRepository);
            Lifecycles = new FakeLifecycleRepository();
            Environments = new FakeEnvironmentRepository();
            MachineRoles = new FakeMachineRoleRepository();
            Machines = new FakeMachineRepository();
            UserRoles = new FakeUserRolesRepository();
            Teams = new FakeTeamsRepository();
            Users = new FakeUsersRepository();
            ProjectTriggers = fakeProjectTriggersRepository;
            Channels = new FakeChannelRepository();
            TagSets = new FakeTagSetsRepository();
            Tenants = new FakeTenantsRepository();
            Client = fakeOctopusClient;
        }

        public IUserInvitesRepository UserInvites { get; }
        public IOctopusAsyncClient Client { get; }
        public RepositoryScope Scope { get; }
        public IArtifactRepository Artifacts { get; }
        public IOctopusSpaceAsyncBetaRepository Beta { get; }
        public IActionTemplateRepository ActionTemplates { get; }
        public IBuildInformationRepository BuildInformationRepository { get; }
        public ICertificateRepository Certificates { get; }
        public ICertificateConfigurationRepository CertificateConfiguration { get; }
        public IBackupRepository Backups { get; }
        public IBuiltInPackageRepositoryRepository BuiltInPackageRepository { get; }
        public IUserTeamsRepository UserTeams { get; }
        public ICommunityActionTemplateRepository CommunityActionTemplates { get; }
        public IConfigurationRepository Configuration { get; }
        public IDashboardConfigurationRepository DashboardConfigurations { get; }
        public IDashboardRepository Dashboards { get; }
        public IDeploymentProcessRepository DeploymentProcesses { get; }
        public IDeploymentSettingsRepository DeploymentSettings { get; }
        public IDeploymentRepository Deployments { get; }
        public IEnvironmentRepository Environments { get; }
        public Task<bool> HasLink(string name)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> HasLinkParameter(string linkName, string parameterName)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> Link(string name)
        {
            throw new System.NotImplementedException();
        }

        public IEventRepository Events { get; }
        public IFeaturesConfigurationRepository FeaturesConfiguration { get; }
        public IFeedRepository Feeds { get; }
        public IInterruptionRepository Interruptions { get; }
        public ILibraryVariableSetRepository LibraryVariableSets { get; }
        public ILifecyclesRepository Lifecycles { get; }
        public IMachineRepository Machines { get; }
        public IMachineRoleRepository MachineRoles { get; }
        public IPackageMetadataRepository PackageMetadataRepository { get; }
        public IMigrationRepository Migrations { get; }
        public ILicensesRepository Licenses { get; }
        public IMachinePolicyRepository MachinePolicies { get; }
        public IPerformanceConfigurationRepository PerformanceConfiguration { get; }
        public IProjectGroupRepository ProjectGroups { get; }
        public IProjectRepository Projects { get; }
        public IRunbookRepository Runbooks { get; }
        public IRunbookProcessRepository RunbookProcesses { get; }
        public IRunbookSnapshotRepository RunbookSnapshots { get; }
        public IRunbookRunRepository RunbookRuns { get; }
        public IReleaseRepository Releases { get; }
        public IProxyRepository Proxies { get; }
        public IServerStatusRepository ServerStatus { get; }
        public ISpaceRepository Spaces { get; }

        public Task<RootResource> LoadRootDocument()
        {
            throw new System.NotImplementedException();
        }

        public ISchedulerRepository Schedulers { get; }
        public ISubscriptionRepository Subscriptions { get; }
        public ITaskRepository Tasks { get; }
        public ITeamsRepository Teams { get; }
        public IScopedUserRoleRepository ScopedUserRoles { get; }
        public IUserPermissionsRepository UserPermissions { get; }
        public ITagSetRepository TagSets { get; }
        public ITenantRepository Tenants { get; }
        public ITenantVariablesRepository TenantVariables { get; }
        public IUserRepository Users { get; }
        public IUserRolesRepository UserRoles { get; }
        public IUpgradeConfigurationRepository UpgradeConfiguration { get; }
        public IVariableSetRepository VariableSets { get; }
        public IWorkerPoolRepository WorkerPools { get; }
        public IWorkerRepository Workers { get; }
        public IChannelRepository Channels { get; }
        public IProjectTriggerRepository ProjectTriggers { get; }
        public Task<SpaceRootResource> LoadSpaceRootDocument()
        {
            throw new System.NotImplementedException();
        }

        public IAccountRepository Accounts { get; }
        public IRetentionPolicyRepository RetentionPolicies { get; }
        public IDefectsRepository Defects { get; }
        public IOctopusServerNodeRepository OctopusServerNodes { get; }
    }
}