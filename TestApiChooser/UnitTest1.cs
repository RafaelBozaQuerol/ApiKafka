using ApiChooser;
using ApiChooser.Interfaces;
using Moq;
using Moq.Protected;
using System.Net;
using FluentAssertions;


namespace TestApiChooser
{
    public class UnitTest1
    {
        [Fact]
        public async Task TestGetAsync()
        {
            var expectedContent = "Hello, Kafka!";
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(expectedContent),
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var apiClient = new ApiClient(httpClient);

            var result = await apiClient.GetAsync("https://api.kafka.com");

            Assert.Equal(expectedContent, result);
        }


        [Fact]
        public async Task TestExecuteApi()
        {
            var mockConfigurationService = new Mock<IConfigurationService>();
            var mockApiClient = new Mock<IApiClient>();
            
            var endpoints = new Dictionary<string, string?>
            {
                { "endpoint1", "/v1/endpoint1_resource" },
                { "endpoint2", "/v1/endpoint1_resource_extended" }
            };

            mockConfigurationService.Setup(cs => cs.Endpoints).Returns(endpoints);
            mockConfigurationService.Setup(cs => cs.BaseUrl).Returns("https://api.kafka.com");
            mockConfigurationService.Setup(cs => cs.Token).Returns("token_string");

            var apiExecutor = new ApiExecutor(mockConfigurationService.Object, mockApiClient.Object);

            await apiExecutor.ExecuteApi(1);

            string expectedUrl = "https://api.kafka.com/v1/endpoint1_resource?token=token_string";

            mockApiClient.Verify(ac => ac.GetAsync(expectedUrl), Times.Once);
        }        
    }
}