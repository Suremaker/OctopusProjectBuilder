using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class TagSetConverter
    {
        public static TagSetResource UpdateWith(this TagSetResource resource, TagSet model, IOctopusRepository repository)
        {
            resource.Name = model.Identifier.Name;

            foreach (var tag in model.Tags)
            {
                resource.AddOrUpdateTag(tag);
            }

            return resource;
        }


        public static TagSet ToModel(this TagSetResource resource, IOctopusRepository repository)
        {
            return new TagSet(new ElementIdentifier(resource.Name), resource.Tags.Select(x => x.Name));
        }
    }
}