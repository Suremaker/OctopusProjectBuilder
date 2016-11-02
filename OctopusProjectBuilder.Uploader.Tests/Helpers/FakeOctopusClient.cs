using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Octopus.Client;
using Octopus.Client.Model;

namespace OctopusProjectBuilder.Uploader.Tests.Helpers
{
    internal class FakeOctopusClient : IOctopusClient
    {
        private readonly Dictionary<string, List<object>> _resources = new Dictionary<string, List<object>>();

        public void AddResource(string path, object resource)
        {
            List<object> list;

            if (!_resources.TryGetValue(path, out list))
                _resources.Add(path, list = new List<object>());

            list.Add(resource);
        }

        public void Paginate<TResource>(string path, Func<ResourceCollection<TResource>, bool> getNextPage)
        {
            List<object> resources;
            _resources.TryGetValue(path, out resources);

            getNextPage(new ResourceCollection<TResource>(resources?.Cast<TResource>() ?? Enumerable.Empty<TResource>(), new LinkCollection()));
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public ResourceCollection<TResource> List<TResource>(string path, object pathParameters = null)
        {
            throw new NotImplementedException();
        }

        public void Paginate<TResource>(string path, object pathParameters, Func<ResourceCollection<TResource>, bool> getNextPage)
        {
            throw new NotImplementedException();
        }

        public TResource Get<TResource>(string path, object pathParameters = null)
        {
            throw new NotImplementedException();
        }

        public TResource Create<TResource>(string path, TResource resource, object pathParameters = null)
        {
            throw new NotImplementedException();
        }

        public void Post<TResource>(string path, TResource resource, object pathParameters = null)
        {
            throw new NotImplementedException();
        }

        public TResponse Post<TResource, TResponse>(string path, TResource resource, object pathParameters = null)
        {
            throw new NotImplementedException();
        }

        public void Post(string path)
        {
            throw new NotImplementedException();
        }

        public void Put<TResource>(string path, TResource resource)
        {
            throw new NotImplementedException();
        }

        public TResource Update<TResource>(string path, TResource resource, object pathParameters = null)
        {
            throw new NotImplementedException();
        }

        public void Delete(string path, object pathParameters = null)
        {
            throw new NotImplementedException();
        }

        public Stream GetContent(string path)
        {
            throw new NotImplementedException();
        }

        public void PutContent(string path, Stream contentStream)
        {
            throw new NotImplementedException();
        }

        public Uri QualifyUri(string path, object parameters = null)
        {
            throw new NotImplementedException();
        }

        public RootResource RefreshRootDocument()
        {
            throw new NotImplementedException();
        }

        public RootResource RootDocument { get; }

#pragma warning disable CS0067
        public event Action<OctopusRequest> SendingOctopusRequest;

        public event Action<OctopusResponse> ReceivedOctopusResponse;
#pragma warning restore CS0067
    }
}