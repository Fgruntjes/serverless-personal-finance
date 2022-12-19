using Microsoft.AspNetCore.DataProtection;

namespace App.Lib.Database;

public class EncryptedString
{
    private IDataProtector? _dataProtector;
    internal string? EncryptedValue { get; init; }
    internal string? DecryptedValue { get; set; }

    public static EncryptedString FromDecryptedValue(string decryptedValue)
    {
        return new EncryptedString
        {
            DecryptedValue = decryptedValue
        };
    }

    public static implicit operator string(EncryptedString value)
    {
        if (value == null)
        {
            return null;
        }
        
        if (value.DecryptedValue != null)
        {
            return value.DecryptedValue;
        }

        if (value.EncryptedValue == null)
        {
            return null;
        }

        if (value._dataProtector == null)
        {
            throw new ArgumentException("Value did not contain a decrypted value or _dataProtector to decrypt encrypted value.", nameof(value));
        }

        value.DecryptedValue = value._dataProtector.Unprotect(value.EncryptedValue);
        return value.DecryptedValue;
    }

    internal static EncryptedString FromEncryptedValue(IDataProtector dataProtector, string encryptedValue)
    {
        return new EncryptedString
        {
            _dataProtector = dataProtector,
            EncryptedValue = encryptedValue
        };
    }
}