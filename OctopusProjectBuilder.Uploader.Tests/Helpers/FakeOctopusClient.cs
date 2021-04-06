using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Octopus.Client;
using Octopus.Client.Model;

namespace OctopusProjectBuilder.Uploader.Tests.Helpers
{
    internal class FakeOctopusClient : IOctopusAsyncClient
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<ResourceCollection<TResource>> List<TResource>(string path, object pathParameters = null)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<TResource>> ListAll<TResource>(string path, object pathParameters = null)
        {
            throw new NotImplementedException();
        }

        public Task Paginate<TResource>(string path, Func<ResourceCollection<TResource>, bool> getNextPage)
        {
            throw new NotImplementedException();
        }

        public Task Paginate<TResource>(string path, object pathParameters, Func<ResourceCollection<TResource>, bool> getNextPage)
        {
            throw new NotImplementedException();
        }

        public Task<TResource> Get<TResource>(string path, object pathParameters = null)
        {
            throw new NotImplementedException();
        }

        public Task<TResource> Create<TResource>(string path, TResource resource, object pathParameters = null)
        {
            throw new NotImplementedException();
        }

        public Task Post<TResource>(string path, TResource resource, object pathParameters = null)
        {
            throw new NotImplementedException();
        }

        public Task<TResponse> Post<TResource, TResponse>(string path, TResource resource, object pathParameters = null)
        {
            throw new NotImplementedException();
        }

        public Task Post(string path)
        {
            throw new NotImplementedException();
        }

        public Task Put<TResource>(string path, TResource resource)
        {
            throw new NotImplementedException();
        }

        public Task Put(string path)
        {
            throw new NotImplementedException();
        }

        public Task Put<TResource>(string path, TResource resource, object pathParameters = null)
        {
            throw new NotImplementedException();
        }

        public Task<TResource> Update<TResource>(string path, TResource resource, object pathParameters = null)
        {
            throw new NotImplementedException();
        }

        public Task Delete(string path, object pathParameters = null, object resource = null)
        {
            throw new NotImplementedException();
        }

        public Task Delete(string path, object pathParameters = null)
        {
            throw new NotImplementedException();
        }

        public Task<Stream> GetContent(string path, object pathParameters = null)
        {
            throw new NotImplementedException();
        }

        public Task PutContent(string path, Stream contentStream)
        {
            throw new NotImplementedException();
        }

        public Uri QualifyUri(string path, object parameters = null)
        {
            throw new NotImplementedException();
        }

        public Task SignIn(LoginCommand loginCommand)
        {
            throw new NotImplementedException();
        }

        public Task SignOut()
        {
            throw new NotImplementedException();
        }

        public IOctopusSpaceAsyncRepository ForSpace(SpaceResource space)
        {
            throw new NotImplementedException();
        }

        public IOctopusSystemAsyncRepository ForSystem()
        {
            throw new NotImplementedException();
        }

        public Task<RootResource> RefreshRootDocument()
        {
            throw new NotImplementedException();
        }

        public RootResource RootDocument { get; }
        public IOctopusAsyncRepository Repository { get; }
        public bool IsUsingSecureConnection { get; }
        public event Action<OctopusRequest> SendingOctopusRequest;
        public event Action<OctopusResponse> ReceivedOctopusResponse;
        public event Action<HttpRequestMessage> BeforeSendingHttpRequest;
        public event Action<HttpResponseMessage> AfterReceivedHttpResponse;
    }
}