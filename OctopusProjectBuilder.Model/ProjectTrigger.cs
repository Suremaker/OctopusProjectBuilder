namespace OctopusProjectBuilder.Model
{
    public class ProjectTrigger
    {
        public enum ProjectTriggerType
        {
            DeploymentTarget
        }

        public string Name { get;  }
        public ProjectTriggerType Type { get;  }
        public ProjectTriggerProperties Properties { get;  }

        public ProjectTrigger(string name, ProjectTriggerType type, ProjectTriggerProperties properties)
        {
            Name = name;
            Type = type;
            Properties = properties;
        }
    }
}