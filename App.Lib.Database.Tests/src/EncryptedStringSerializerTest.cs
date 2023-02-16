using Microsoft.AspNetCore.DataProtection;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using Moq;
using BsonDeserializationContext = MongoDB.Bson.Serialization.BsonDeserializationContext;
using BsonSerializationArgs = MongoDB.Bson.Serialization.BsonSerializationArgs;
using BsonSerializationContext = MongoDB.Bson.Serialization.BsonSerializationContext;

namespace App.Lib.Database.Tests;

public class EncryptedStringSerializerTest
{
    private const string DecryptedTestValue = "test";
    private const string EncryptedTestValue = "dGVzdA";

    private readonly EncryptedStringSerializer _serializer;
    private readonly BsonSerializationContext _serializeContext;
    private readonly Mock<IBsonWriter> _writerMock;
    private readonly Mock<IBsonReader> _readerMock;
    private readonly BsonDeserializationContext _deserializeContext;

    public EncryptedStringSerializerTest()
    {
        _serializer = new EncryptedStringSerializer(DataProtectorProviderMock.Create().Object);
        _writerMock = new Mock<IBsonWriter>();
        _readerMock = new Mock<IBsonReader>();
        _serializeContext = BsonSerializationContext.CreateRoot(_writerMock.Object);
        _deserializeContext = BsonDeserializationContext.CreateRoot(_readerMock.Object);
    }

    [Fact]
    public void EncryptDecryptedValue()
    {
        _serializer.Serialize(
            _serializeContext,
            new BsonSerializationArgs(),
            EncryptedString.FromDecryptedValue(DecryptedTestValue)
        );
        _writerMock.Verify(writer => writer.WriteString(EncryptedTestValue), Times.Once());
    }

    [Fact]
    public void DecryptEncryptedValue()
    {
        _readerMock.Setup(reader => reader.ReadString()).Returns(EncryptedTestValue);

        var val = _serializer.Deserialize(_deserializeContext, new BsonDeserializationArgs());

        Assert.Equal(DecryptedTestValue, val);
    }
}