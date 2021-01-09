namespace Microsoft.Extensions.Configuration.Registry
{
    using System;

    public static class RegistryConfigurationExtensions
    {
        /// <summary>
        ///     Adds the JSON configuration provider at <paramref name="keyPath"/> to <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">
        ///     The <see cref="IConfigurationBuilder"/> to add to.
        /// </param>
        /// <param name="keyPath">
        ///     Path relative to the base path stored in <see cref="IConfigurationBuilder.Properties"/>
        ///     of <paramref name="builder"/>.
        /// </param>
        /// <returns>
        ///     The <see cref="IConfigurationBuilder"/>.
        /// </returns>
        public static IConfigurationBuilder AddRegistrySettings(
            this IConfigurationBuilder builder,
            string keyPath)
        {
            if (keyPath is null || string.IsNullOrWhiteSpace(keyPath))
            {
                throw new ArgumentException("Key path must be a non-empty string.", nameof(keyPath));
            }

            var source = new RegistryConfigurationSource(keyPath, () => Win32.Registry.CurrentUser);

            return builder.Add(source);
        }

        /// <summary>
        ///     Adds the JSON configuration provider at <paramref name="keyPath"/> to <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">
        ///     The <see cref="IConfigurationBuilder"/> to add to.
        /// </param>
        /// <param name="keyPath">
        ///     Path relative to the base path stored in <see cref="IConfigurationBuilder.Properties"/>
        ///     of <paramref name="builder"/>.
        /// </param>        ///
        /// <param name="registry">
        ///     A top level system key as <see cref="Win32.RegistryKey"/>
        /// </param>
        /// <returns>
        ///     The <see cref="IConfigurationBuilder"/>.
        /// </returns>
        public static IConfigurationBuilder AddRegistrySettings(
            this IConfigurationBuilder builder,
            string keyPath,
            Func<Win32.RegistryKey> registry)
        {
            if (keyPath is null || string.IsNullOrWhiteSpace(keyPath))
            {
                throw new ArgumentException("Key path must be a non-empty string.", nameof(keyPath));
            }

            var source = new RegistryConfigurationSource(keyPath, registry);

            return builder.Add(source);
        }
    }
}
