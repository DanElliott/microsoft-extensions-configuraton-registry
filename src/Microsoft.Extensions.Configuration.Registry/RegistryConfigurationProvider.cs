namespace Microsoft.Extensions.Configuration.Registry
{
    using Microsoft.Win32;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class RegistryConfigurationProvider : ConfigurationProvider
    {
        private readonly RegistryConfigurationSource source;

        public RegistryConfigurationProvider(RegistryConfigurationSource source)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
        }

        public override void Load()
        {
            var registry = this.source.Registry.Invoke();

            if (registry is null)
            {
                throw new ArgumentOutOfRangeException(nameof(this.source), $"{nameof(RegistryConfigurationSource.Registry)} must return not null instance of {this.source.Registry.Method.ReturnType.Name}");
            }

            var subKey = registry.OpenSubKey(this.source.KeyPath);

            if (subKey is null)
            {
                throw new ArgumentException($"The key path '{this.source.KeyPath}' was not found and is not optional. The registry is '{registry.Name}'.");
            }

            var data = new Dictionary<string, string>();

            var stack = new Stack<string>();

            try
            {
                ParseKeyValuePairs(subKey, data, stack);
            }
            finally
            {
                if (subKey is { })
                {
                    subKey.Dispose();
                }
            }

            this.Data = data;
        }

        private void ParseKeyValuePairs(RegistryKey? subKey, Dictionary<string, string> data, Stack<string> stack)
        {
            if (subKey is null)
            {
                throw new ArgumentNullException(nameof(subKey));
            }

            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (stack is null)
            {
                throw new ArgumentNullException(nameof(stack));
            }

            foreach (var subkeyName in subKey.GetSubKeyNames())
            {
                stack.Push(subkeyName);

                using (var subkey = subKey.OpenSubKey(subkeyName))
                {
                    ParseKeyValuePairs(subkey, data, stack);
                }

                stack.Pop();
            }

            foreach (var valueName in subKey.GetValueNames())
            {
                stack.Push(valueName.Replace(".", string.Empty));

                string key = ConfigurationPath.Combine(stack.Reverse());
                data[key] = subKey.TryGetValue(valueName);

                stack.Pop();
            }
        }
    }
}
