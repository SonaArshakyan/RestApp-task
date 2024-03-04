using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Net;
using System.Threading.Tasks;
using ThirdParty;
using RestApp.Interfaces;

namespace RestApp.Tests
{
    [TestClass]
    public class RestAppClassThatUsesRestClientTests
    {
        private Mock<IRestClient> _mockRestClient;
        private Mock<ILogger> _mockLogger;
        private RestAppClassThatUsesRestClient _restAppClass;
        private AppSettings _appSettings;

        [TestInitialize]
        public void Initialize()
        {
            _mockRestClient = new Mock<IRestClient>();
            _mockLogger = new Mock<ILogger>();
            _appSettings = new AppSettings { RetryCount = 3, RetryMiliseconds = 1000 };
            _restAppClass = new RestAppClassThatUsesRestClient(_mockRestClient.Object, _mockLogger.Object, _appSettings);
        }

        [TestMethod]
        public async Task GetSomething_RetriesOnWebException_ReturnsNullAfterMaxRetries()
        {
            // Arrange
            var url = "http://test.com";
            _mockRestClient.SetupSequence(x => x.Get<string>(url))
                           .Throws(new WebException())
                           .Throws(new WebException())
                           .Throws(new WebException())
                           .Throws(new WebException());

            // Act
            var result = await _restAppClass.GetSomething<string>(url);

            // Assert
            Assert.IsNull(result);
            _mockRestClient.Verify(x => x.Get<string>(url), Times.Exactly(_appSettings.RetryCount + 1));
            _mockLogger.Verify(x => x.Error(It.IsAny<WebException>()), Times.Once());
        }

        [TestMethod]
        public async Task GetSomething_ThrowsNonWebException_DoesNotRetry()
        {
            // Arrange
            var url = "http://test.com";
            _mockRestClient.SetupSequence(x => x.Get<string>(url))
                                            .Throws(new InvalidOperationException());

            // Act
            var result = await _restAppClass.GetSomething<string>(url);

            // Assert
            Assert.IsNull(result);
            _mockRestClient.Verify(x => x.Get<string>(url), Times.Once());
            _mockLogger.Verify(x => x.Error(It.IsAny<Exception>()), Times.Once());
        }

        [TestMethod]
        public async Task GetSomething_SuccessfulCall_DoesNotRetry()
        {
            // Arrange
            var url = "http://test.com";
            var expected = "Success";
            _mockRestClient.Setup(x => x.Get<string>(url)).ReturnsAsync(expected);

            // Act
            var result = await _restAppClass.GetSomething<string>(url);

            // Assert
            Assert.AreEqual(expected, result);
            _mockRestClient.Verify(x => x.Get<string>(url), Times.Once());
            _mockLogger.Verify(x => x.Error(It.IsAny<Exception>()), Times.Never());
        }
    }
}
