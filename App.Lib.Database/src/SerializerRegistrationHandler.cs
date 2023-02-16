using Microsoft.AspNetCore.DataProtection;
using MongoDB.Bson.Serialization;

namespace App.Lib.Database;

public static class SerializerRegistrationHandler
{
    private static Boolean SerializersRegistered;
    private static object SerializeREgisterLock = new();

    public static void RegisterSerializer(IDataProtectionProvider dataProtectorProvider)
    {
        lock (SerializeREgisterLock)
        {
            if (SerializersRegistered) return;

            BsonSerializer.RegisterSerializer(typeof(EncryptedString), new EncryptedStringSerializer(dataProtectorProvider));
            SerializersRegistered = true;
        }
    }
}
