using System;
using System.ComponentModel;
using OctopusProjectBuilder.Model;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Description("Machine connectivity policy.")]
    [Serializable]
    public class YamlMachineConnectivityPolicy
    {
        [Description("Connectivty behaviour.")]
        [YamlMember(Order = 1)]
        [DefaultValue(-1)]
        public MachineConnectivityBehavior ConnectivityBehavior { get; set; }

        public YamlMachineConnectivityPolicy()
        {
        }

        public YamlMachineConnectivityPolicy(MachineConnectivityBehavior connectivityBehavior)
        {
            ConnectivityBehavior = connectivityBehavior;
        }

        public MachineConnectivityPolicy ToModel()
        {
            if (ConnectivityBehavior == MachineConnectivityBehavior.Unspecified)
                return new MachineConnectivityPolicy(MachineConnectivityBehavior.ExpectedToBeOnline);
            return new MachineConnectivityPolicy(ConnectivityBehavior);
        }

        public static YamlMachineConnectivityPolicy FromModel(MachineConnectivityPolicy connectivityPolicy)
        {
            return new YamlMachineConnectivityPolicy(connectivityPolicy.MachineConnectivityBehavior);
        }
    }
}