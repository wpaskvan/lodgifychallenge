using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace SuperPanel.Tests.Fakes
{
    public class FakeHttpClientFactory : IHttpClientFactory
    {
        private Func<HttpClient> _httpclienBuilder;

        public FakeHttpClientFactory(Func<HttpClient> httpclienBuilder)
        {
            _httpclienBuilder = httpclienBuilder;
        }

        public HttpClient CreateClient(string name)
        {
            return _httpclienBuilder.Invoke();
        }
    }
}
