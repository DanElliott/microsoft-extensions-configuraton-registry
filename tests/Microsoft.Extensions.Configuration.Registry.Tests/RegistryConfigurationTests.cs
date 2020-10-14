namespace Microsoft.Extensions.Configuration.Registry.Test
{
    using Microsoft.Extensions.Configuration.Registry;
    using Xunit;

    public class RegistryConfigurationTests
    {
        [Fact]
        public void TryGet_ExistingKey_ShouldReturn()
        {
            // Arrange
            var target = new RegistryConfigurationProvider(new RegistryConfigurationSource(
                @"SOFTWARE\Microsoft\Shell",
                () => Microsoft.Win32.Registry.LocalMachine));

            target.Load();

            // Act
            target.TryGet("USB:NotifyOnUsbErrors", out string actual);

            // Assert
            Assert.Equal("1", actual);
        }
    }
}
