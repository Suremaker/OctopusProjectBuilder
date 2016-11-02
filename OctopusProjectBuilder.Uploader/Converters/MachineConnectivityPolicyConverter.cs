using System;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class MachineConnectivityPolicyConverter
    {
        public static MachineConnectivityPolicy ToModel(this Octopus.Client.Model.MachineConnectivityPolicy resource)
        {
            if (resource.MachineConnectivityBehavior == Octopus.Client.Model.MachineConnectivityBehavior.ExpectedToBeOnline)
                return new MachineConnectivityPolicy(MachineConnectivityBehavior.ExpectedToBeOnline);
            if (resource.MachineConnectivityBehavior == Octopus.Client.Model.MachineConnectivityBehavior.MayBeOfflineAndCanBeSkipped)
                return new MachineConnectivityPolicy(MachineConnectivityBehavior.MayBeOfflineAndCanBeSkipped);

            throw new InvalidOperationException($"Unsupported {nameof(Octopus.Client.Model.MachineConnectivityBehavior)}");
        }

        public static void UpdateWith(this Octopus.Client.Model.MachineConnectivityPolicy resource, MachineConnectivityPolicy model)
        {
            resource.MachineConnectivityBehavior = (Octopus.Client.Model.MachineConnectivityBehavior)model.MachineConnectivityBehavior;
        }
    }
}