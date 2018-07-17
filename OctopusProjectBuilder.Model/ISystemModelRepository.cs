using System.IO;

namespace OctopusProjectBuilder.Model
{
    public interface ISystemModelRepository
    {
        SystemModel Load(string modelDirectory, SearchOption searchOption = SearchOption.AllDirectories);
        void Save(SystemModel model, string modelDirectory);
    }
}
