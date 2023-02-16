using Microsoft.AspNetCore.DataProtection;
using Moq;

namespace App.Lib.Database.Tests;

public static class DataProtectorProviderMock
{
    public static Mock<IDataProtectionProvider> Create()
    {
        var dataProtectorMock = new Mock<IDataProtector>();
        dataProtectorMock
            .Setup(protector => protector.Protect(It.IsAny<byte[]>()))
            .Returns<byte[]>(plaintext => plaintext);

        dataProtectorMock
            .Setup(protector => protector.Unprotect(It.IsAny<byte[]>()))
            .Returns<byte[]>(plaintext => plaintext);

        var dataProtectionProviderMock = new Mock<IDataProtectionProvider>();
        dataProtectionProviderMock
            .Setup(provider => provider.CreateProtector(It.IsAny<string>()))
            .Returns(dataProtectorMock.Object);

        return dataProtectionProviderMock;
    }
}