using System;
using System.ComponentModel;
using OctopusProjectBuilder.Model;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Description("Machine health check script policy.")]
    [Serializable]
    public class YamlMachineHealthCheckScriptPolicy
    {
        [Description("Script policy run type.")]
        [YamlMember(Order = 1)]
        [DefaultValue(-1)]
        public MachineScriptPolicyRunType RunType { get; set; }

        [Description("Scritp body.")]
        [YamlMember(Order = 2)]
        public string ScriptBody { get; set; }

        public YamlMachineHealthCheckScriptPolicy()
        {
        }

        private YamlMachineHealthCheckScriptPolicy(MachineScriptPolicyRunType runType, string scriptBody = null)
        {
            RunType = runType;
            ScriptBody = scriptBody;
        }

        public static YamlMachineHealthCheckScriptPolicy FromModel(MachineHealthCheckScriptPolicy scriptPolicy)
        {
            return new YamlMachineHealthCheckScriptPolicy(scriptPolicy.RunType, scriptPolicy.ScriptBody);
        }

        public MachineHealthCheckScriptPolicy ToModel()
        {
            if (RunType == MachineScriptPolicyRunType.Unspecified || RunType == MachineScriptPolicyRunType.InheritFromDefault)
                return MachineHealthCheckScriptPolicy.InheritFromDefault();
            return MachineHealthCheckScriptPolicy.Inline(ScriptBody);
        }
    }
}