namespace Microsoft.Extensions.Configuration.Registry
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Win32;
    using System;

    public class RegistryConfigurationSource : IConfigurationSource
    {
        public RegistryConfigurationSource(string? keyPath, Func<RegistryKey> registry)
        {
            this.KeyPath = keyPath ?? throw new ArgumentNullException(nameof(keyPath));
            this.Registry = registry ?? throw new ArgumentNullException(nameof(registry));
        }

        public string KeyPath { get; set; }
        public Func<RegistryKey> Registry { get; set; }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new RegistryConfigurationProvider(this);
        }
    }
}
