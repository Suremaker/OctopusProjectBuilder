using System;
using System.IO;
using Octopus.Client;
using Octopus.Client.Model;

namespace OctopusProjectBuilder.Uploader.Tests.Helpers
{
    internal class FakeOctopusClient : IOctopusClient
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public ResourceCollection<TResource> List<TResource>(string path, object pathParameters = null)
        {
            throw new NotImplementedException();
        }

        public void Paginate<TResource>(string path, Func<ResourceCollection<TResource>, bool> getNextPage)
        {
            getNextPage(new ResourceCollection<TResource>(new TResource[0], new LinkCollection()));
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
        public event Action<OctopusRequest> SendingOctopusRequest;
        public event Action<OctopusResponse> ReceivedOctopusResponse;
    }
}