namespace Microsoft.Extensions.Configuration.Registry
{
    using System;
    using System.Globalization;
    using System.Linq;
    using Microsoft.Win32;

    public static class RegistryKeyExtensions
    {
        public static string? TryGetValue(this RegistryKey? subKey, string? name)
        {
            if (subKey is null || string.IsNullOrEmpty(name))
            {
                return string.Empty;
            }

            object? value = subKey.GetValue(name);
            RegistryValueKind? registryValueKind = subKey.GetValueKind(name);

            string? valueAsString = null;

            if (value is { })
            {
                switch (registryValueKind)
                {
                    case RegistryValueKind.Binary:
                        byte?[] byteArray = (byte?[])value;
                        valueAsString = string.Join(" ", byteArray.Select(b => b.ToString()));
                        break;

                    case RegistryValueKind.DWord:
                        int? dword = (int?)value;
                        valueAsString = Convert.ToString(value: (int)dword, toBase: 16).ToUpper();
                        break;

                    case RegistryValueKind.QWord:
                        long? qword = (long?)value;
                        valueAsString = Convert.ToString(value: (long)qword, toBase: 16).ToUpper();
                        break;

                    case RegistryValueKind.String:
                        valueAsString = (value ?? string.Empty).ToString();
                        break;

                    default:
                        valueAsString = string.Empty;
                        break;
                }
            }

            return valueAsString;
        }
    }
}
