namespace OctopusProjectBuilder.Model
{
    public class MachineHealthCheckScriptPolicy
    {
        private MachineHealthCheckScriptPolicy()
        {
        }

        private MachineHealthCheckScriptPolicy(MachineScriptPolicyRunType runType, string scriptBody)
        {
            RunType = runType;
            ScriptBody = scriptBody;
        }

        public static MachineHealthCheckScriptPolicy InheritFromDefault()
        {
            return new MachineHealthCheckScriptPolicy();
        }

        public static MachineHealthCheckScriptPolicy Inline(string scriptBody)
        {
            return new MachineHealthCheckScriptPolicy(MachineScriptPolicyRunType.Inline, scriptBody);
        }

        public MachineScriptPolicyRunType RunType { get; }
        public string ScriptBody { get; }
    }

    public enum MachineScriptPolicyRunType
    {
        Unspecified = -1,
        InheritFromDefault,
        Inline
    }
}