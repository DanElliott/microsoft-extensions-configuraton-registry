namespace Microsoft.Extensions.Configuration.Registry.Tests
{
    using Microsoft.Extensions.Configuration;
    using System;
    using System.IO;
    using Xunit;

    public class RegistryConfigurationExtensionsTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void AddRegistrySettings_ThrowsIfKeyPathIsNullOrEmpty(string path)
        {
            // Arrange
            var configurationBuilder = new ConfigurationBuilder();

            // Act and Assert
            var ex = Assert.Throws<ArgumentException>(() => configurationBuilder.AddRegistrySettings(path));
            Assert.Equal(expected: "keyPath", actual: ex.ParamName);
            Assert.StartsWith(
                expectedStartString: "Key path must be a non-empty string.", 
                actualString: ex.Message);
        }

        [Fact]
        public void AddRegistrySettings_ThrowsIfSettingsDoesNotExistAtPath()
        {
            // Arrange
            var path = @"this\path\does\not\exist";

            // Act and Assert
            var ex = Assert.Throws<ArgumentException>(() => new ConfigurationBuilder().AddRegistrySettings(path).Build());
            Assert.StartsWith(
                expectedStartString: $"The key path '{path}' was not found and is not optional. The registry is '",
                actualString: ex.Message);
        }
    }
}
