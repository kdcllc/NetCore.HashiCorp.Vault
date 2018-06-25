using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.HashiCorpVault;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTests
{
    public class ConfigurationProviderTests
    {
        [Fact]
        public void Template()
        {
            // Assign
            // Act
            // Assert
        }

        [Fact]
        public void LoadsAllSecretsFromVault()
        {
            // Assign
            var path = "secret";
            var value = "some_secret";
            var secrets = new[] { "group1/key"};

            var client = new Mock<IHashiCorpVaultClient>(MockBehavior.Strict);
            client.Setup(c => c.GetSecretAsync(It.IsAny<string>())).Returns(Task.FromResult(value));
            
            // Act
            var provider = new HashiCorpVaultConfigurationProvider(client.Object, path, secrets);
            provider.Load();

            // Assert
            client.VerifyAll();
            Assert.Equal("some_secret", provider.Get("group1:key"));

        }

        [Fact]
        public void ConstructorThrowsForNullClient()
        {
            Assert.Throws<ArgumentNullException>(() => new HashiCorpVaultConfigurationProvider(null, "", new[] { "test" } ));
        }

        [Fact]
        public void ConstructorThrowsForNullKeyPrefix()
        {
            Assert.Throws<ArgumentNullException>(() => new HashiCorpVaultConfigurationProvider(Mock.Of<IHashiCorpVaultClient>(), null, new[] { "test" }));
        }

        [Fact]
        public void ConstructorThrowsForNullSecrets()
        {
            Assert.Throws<ArgumentNullException>(() => new HashiCorpVaultConfigurationProvider(Mock.Of<IHashiCorpVaultClient>(), "secret", null));
        }

        [Fact]
        public void Return_BasePath_WithIncorrectSuffix()
        {
            // Assign
            var link = @"test";
            var obj = new HashiCorpVaultConfigurationProvider(Mock.Of<IHashiCorpVaultClient>(), link, new[] { "key" });

            // Act
            var result = obj.GetBasePath(link);

            // Assert
            Assert.NotEqual(link, result);
        }


        [Fact]
        public void Return_BasePath_WithCorrectSuffix()
        {
            // Assign
            var link = @"test/";
            var obj = new HashiCorpVaultConfigurationProvider(Mock.Of<IHashiCorpVaultClient>(), link, new[] {"key" });

            // Act
            var result = obj.GetBasePath(link);

            // Assert
            Assert.Equal(link, result);
        }
    }

    public static class ConfigurationProviderExtensions
    {
        public static string Get(this IConfigurationProvider provider, string key)
        {
            string value;

            if (!provider.TryGet(key, out value))
            {
                throw new InvalidOperationException("Key not found");
            }

            return value;
        }
    }
}
