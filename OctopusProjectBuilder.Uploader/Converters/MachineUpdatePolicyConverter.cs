using System;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class MachineUpdatePolicyConverter
    {
        public static MachineUpdatePolicy ToModel(this Octopus.Client.Model.MachineUpdatePolicy resource)
        {
            return new MachineUpdatePolicy(
                CalamariToModel(resource.CalamariUpdateBehavior), 
                TentacleToModel(resource.TentacleUpdateBehavior));
        }

        public static void UpdateWith(this Octopus.Client.Model.MachineUpdatePolicy resource, MachineUpdatePolicy model)
        {
            resource.CalamariUpdateBehavior = (Octopus.Client.Model.CalamariUpdateBehavior)model.CalamariUpdateBehavior;
            resource.TentacleUpdateBehavior = (Octopus.Client.Model.TentacleUpdateBehavior)model.TentacleUpdateBehavior;
        }

        private static CalamariUpdateBehavior CalamariToModel(Octopus.Client.Model.CalamariUpdateBehavior calamari)
        {
            if (calamari == Octopus.Client.Model.CalamariUpdateBehavior.UpdateOnDeployment)
                return CalamariUpdateBehavior.UpdateOnDeployment;
            if (calamari == Octopus.Client.Model.CalamariUpdateBehavior.UpdateOnNewMachine)
                return CalamariUpdateBehavior.UpdateOnNewMachine;
            if (calamari == Octopus.Client.Model.CalamariUpdateBehavior.UpdateAlways)
                return CalamariUpdateBehavior.UpdateAlways;
            throw new InvalidOperationException($"Unsupported {nameof(Octopus.Client.Model.CalamariUpdateBehavior)}");
        }

        private static TentacleUpdateBehavior TentacleToModel(Octopus.Client.Model.TentacleUpdateBehavior tentacle)
        {
            if (tentacle == Octopus.Client.Model.TentacleUpdateBehavior.NeverUpdate)
                return TentacleUpdateBehavior.NeverUpdate;
            if (tentacle == Octopus.Client.Model.TentacleUpdateBehavior.Update)
                return TentacleUpdateBehavior.Update;
            throw new InvalidOperationException($"Unsupported {nameof(Octopus.Client.Model.TentacleUpdateBehavior)}");
        }
    }
}