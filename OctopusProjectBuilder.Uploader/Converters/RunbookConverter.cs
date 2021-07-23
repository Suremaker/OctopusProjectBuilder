using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;
using GuidedFailureMode = Octopus.Client.Model.GuidedFailureMode;
using RunbookEnvironmentScope = Octopus.Client.Model.RunbookEnvironmentScope;
using TenantedDeploymentMode = Octopus.Client.Model.TenantedDeploymentMode;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class RunbookConverter
    {
        
        public static async Task<RunbookResource> UpdateWith(this RunbookResource resource, Runbook model,
            IOctopusAsyncRepository repository)
        {
            if (model.Identifier != null)
            {
                resource.Name = model.Identifier.Name;
            }

            resource.Description = model.Description;
            
            resource.ProjectId = (await repository.Projects.FindByName(model.ProjectName)).Id;
            
            resource.Environments.UpdateWith(await Task.WhenAll(model.EnvironmentRefs
                .Select(r => repository.Environments.ResolveResourceId(r))));

            if (model.EnvironmentScope.HasValue)
                resource.EnvironmentScope = (RunbookEnvironmentScope) model.EnvironmentScope.Value;

            if (model.MultiTenancyMode.HasValue)
                resource.MultiTenancyMode = (TenantedDeploymentMode) model.MultiTenancyMode.Value;

            if (model.RunRetentionPolicy != null)
                resource.RunRetentionPolicy = model.RunRetentionPolicy.FromModel();

            if (model.GuidedFailureMode != null)
                resource.DefaultGuidedFailureMode = (GuidedFailureMode) model.GuidedFailureMode;

            if (model.ConnectivityPolicy != null)
                await resource.ConnectivityPolicy.UpdateWith(model.ConnectivityPolicy, repository);

            return resource;
        }

        public static async Task<Runbook> ToModel(this RunbookResource resource, IOctopusAsyncRepository repository)
        {
            var projectResource = await repository.Projects.Get(resource.ProjectId);
            var runbookProcess = await repository.RunbookProcesses.Get(resource.RunbookProcessId);

            return new Runbook()
            {
                Description = resource.Description,
                EnvironmentScope = (Model.RunbookEnvironmentScope?) resource.EnvironmentScope,
                Identifier = new ElementIdentifier(resource.Name),
                MultiTenancyMode = (Model.TenantedDeploymentMode?) resource.MultiTenancyMode,
                ProjectName = projectResource.Name,
                RunRetentionPolicy = resource.RunRetentionPolicy.ToModel(),
                Process = await runbookProcess.ToModel(repository),
                GuidedFailureMode = (Model.GuidedFailureMode?) resource.DefaultGuidedFailureMode,
                EnvironmentRefs = await Task.WhenAll(resource.Environments.ToModel(repository.Environments))
            };
        }
    }
}