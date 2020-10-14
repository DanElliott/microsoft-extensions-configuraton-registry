namespace Microsoft.Extensions.Configuration.Registry
{
    using Microsoft.Win32;
    using System;
    using System.Linq;

    public static class RegistryKeyExtensions
    {
        public static string TryGetValue(this RegistryKey? subKey, string? name)
        {
            if (subKey is null || string.IsNullOrEmpty(name))
            {
                return string.Empty;
            }

            object value = subKey.GetValue(name);
            RegistryValueKind registryValueKind = subKey.GetValueKind(name);

            string valueAsString;

            switch (registryValueKind)
            {
                case RegistryValueKind.Binary:
                    byte[] byteArray = (byte[])value;
                    valueAsString = string.Join(" ", byteArray.Select(b => b.ToString()));
                    break;

                case RegistryValueKind.DWord:
                    int dword = (int)value;
                    valueAsString = Convert.ToString(dword, 16).ToUpper();
                    break;

                case RegistryValueKind.QWord:
                    long qword = (long)value;
                    valueAsString = Convert.ToString(qword, 16).ToUpper();
                    break;

                case RegistryValueKind.String:
                    valueAsString = (value ?? string.Empty).ToString();
                    break;

                default:
                    valueAsString = string.Empty;
                    break;
            }

            return valueAsString;
        }
    }
}
