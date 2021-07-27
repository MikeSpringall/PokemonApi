using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PokemonAPI.Domain.Models;
using PokemonAPI.Domain.Services;
using PokemonAPI.Domain.Wrappers;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PokemonAPI.Tests.Unit.Services
{
    [TestClass]
    public class TranslationServiceTests
    {
        private Mock<IHttpClient> _httpClient;
        private Mock<ILogger<TranslationService>> _logger;
        private TranslationService _translationService;

        [TestInitialize]
        public void Setup()
        {
            _httpClient = new Mock<IHttpClient>();
            _logger = new Mock<ILogger<TranslationService>>();
            _translationService = new TranslationService(_httpClient.Object, _logger.Object);
        }

        [TestMethod]
        public async Task GetTranslation_should_make_expected_request()
        {
            var translationType = TranslationType.Shakespeare;
            var description = "pokemon description";
            var expectedUrl = $"/translate/{translationType}.json";
            var expectedContent = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("text", description) });
            _httpClient.Setup(x => x.PostAsync(expectedUrl, expectedContent)).Returns(Task.FromResult(new HttpResponseMessage()));

            await _translationService.GetTranslation(translationType, description);

            _httpClient.Verify(x => x.PostAsync(expectedUrl, It.Is<FormUrlEncodedContent>(x => x.ReadAsStringAsync().Result == expectedContent.ReadAsStringAsync().Result)), Times.Once);
        }

        [TestMethod]
        public async Task GetTranslation_should_handle_null_response_and_log_error()
        {
            var description = "pokemon description";
            _httpClient.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<FormUrlEncodedContent>())).Returns(Task.FromResult<HttpResponseMessage>(null));

            var result = await _translationService.GetTranslation(TranslationType.Shakespeare, description);

            result.ShouldBe(description);
            VerifyLogMessage(LogLevel.Error, "Error Deserializing translation response.");
        }

        [TestMethod]
        public async Task GetTranslation_should_handle_empty_response_and_log_info()
        {
            var description = "pokemon description";
            var translationType = TranslationType.Shakespeare;
            var expectedLogMessage = $"Description was not translated. TranslationType: {translationType}. Description: {description}";
            _httpClient.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<FormUrlEncodedContent>())).Returns(Task.FromResult(new HttpResponseMessage()));

            var result = await _translationService.GetTranslation(translationType, description);

            result.ShouldBe(description);
            VerifyLogMessage(LogLevel.Information, expectedLogMessage);
        }

        [TestMethod]
        public async Task GetTranslation_should_handle_response_without_translation()
        {
            var description = "pokemon description";
            var translationType = TranslationType.Shakespeare;
            var expectedLogMessage = $"Description was not translated. No successful translations. TranslationType: {translationType}. Description: {description}";
            var emptyResponse = "{ \"success\": { \"total\": 0 } }";
            _httpClient.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<FormUrlEncodedContent>())).Returns(Task.FromResult(new HttpResponseMessage { Content = new StringContent(emptyResponse) }));

            var result = await _translationService.GetTranslation(translationType, description);

            result.ShouldBe(description);
            VerifyLogMessage(LogLevel.Information, expectedLogMessage);
        }

        [TestMethod]
        public async Task GetTranslation_should_handle_malformed_response_without_translation()
        {
            var description = "pokemon description";
            var emptyResponse = "{ \"success\": { \"total\": 1 } }";
            _httpClient.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<FormUrlEncodedContent>())).Returns(Task.FromResult(new HttpResponseMessage { Content = new StringContent(emptyResponse) }));

            var result = await _translationService.GetTranslation(TranslationType.Shakespeare, description);

            result.ShouldBe(description);
        }

        [TestMethod]
        public async Task GetTranslation_should_return_expected_translation()
        {
            var description = "pokemon description";
            var translatedDescription = "pokemon description translated";
            var emptyResponse = $"{{ \"success\": {{ \"total\": 1 }}, \"contents\": {{ \"translated\": \"{translatedDescription}\" }} }}";
            _httpClient.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<FormUrlEncodedContent>())).Returns(Task.FromResult(new HttpResponseMessage { Content = new StringContent(emptyResponse) }));

            var result = await _translationService.GetTranslation(TranslationType.Yoda, description);

            result.ShouldBe(translatedDescription);
        }

        private void VerifyLogMessage(LogLevel expectedLogLevel, string expectedMessage)
        {
            _logger.Verify(x => x.Log(It.Is<LogLevel>(logLevel => logLevel == expectedLogLevel),
                          It.IsAny<EventId>(),
                          It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == expectedMessage),
                          It.IsAny<Exception>(),
                          It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }
    }
}
