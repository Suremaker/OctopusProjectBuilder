namespace OctopusProjectBuilder.Model
{
    public interface ISystemModelRepository
    {
        SystemModel Load(string modelDirectory);
        void Save(SystemModel model, string modelDirectory);
    }
}
