using Microsoft.AspNetCore.DataProtection;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace App.Lib.Database;

public class EncryptedStringSerializer : SerializerBase<EncryptedString>
{
    private const string EncryptionPurpose = "DatabaseEncryptedString";

    private readonly IDataProtector _dataProtector;

    public EncryptedStringSerializer(IDataProtectionProvider dataProtectionProvider)
    {
        _dataProtector = dataProtectionProvider.CreateProtector(EncryptionPurpose);
    }

    public override EncryptedString Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var encryptedValue = context.Reader.ReadString();

        return EncryptedString.FromEncryptedValue(_dataProtector, encryptedValue);
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, EncryptedString value)
    {
        var encryptedString = value.EncryptedValue ?? _dataProtector.Protect(value);
        context.Writer.WriteString(encryptedString);
    }
}