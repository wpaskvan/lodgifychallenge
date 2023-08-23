using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SuperPanel.Tests.Fakes
{
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly Func<Task<HttpResponseMessage>> _fakeAsyncOperation;

        public FakeHttpMessageHandler(Func<Task<HttpResponseMessage>> fakeAsyncOperation) 
        {
            _fakeAsyncOperation = fakeAsyncOperation;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return await _fakeAsyncOperation.Invoke();
        }
    }
}
