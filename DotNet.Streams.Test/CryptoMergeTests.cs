using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace DotNet.Streams.Test
{
    public class CryptoMergeTests
    {
        private readonly ITestOutputHelper _output;

        public CryptoMergeTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [ClassData(typeof(SingleToManyTestStreams))]
        public async Task CanEncryptAndDecryptMergedStream(MergedStream mergedStream, string expectedData)
        {
            using (var aes = Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.GenerateKey();
                aes.GenerateIV();

                using (var encryptor = aes.CreateEncryptor())
                using (var encryptedStream = new CryptoStream(mergedStream, encryptor, CryptoStreamMode.Read))
                using (var toBase64Transform = new ToBase64Transform())
                using (var encodedStream = new CryptoStream(encryptedStream, toBase64Transform, CryptoStreamMode.Read))
                using (var fromBase64Transform = new FromBase64Transform())
                using (var decodedStream = new CryptoStream(encodedStream, fromBase64Transform, CryptoStreamMode.Read))
                using (var decryptor = aes.CreateDecryptor())
                using (var decryptedStream = new CryptoStream(encryptedStream, decryptor, CryptoStreamMode.Read))
                using (var streamReader = new StreamReader(decryptedStream))
                {
                    var data = await streamReader.ReadToEndAsync();
                    Assert.Equal(expectedData, data);
                }
            }
        }

        [Theory]
        [ClassData(typeof(SingleToManyTestStreams))]
        public async Task Base64EncodeAndDecodeMergedStream(MergedStream mergedStream, string expectedData)
        {
            using (var toBase64Transform = new ToBase64Transform())
            using (var encodedStream = new CryptoStream(mergedStream, toBase64Transform, CryptoStreamMode.Read))
            using (var memoryStream = new MemoryStream())
            {
                await encodedStream.CopyToAsync(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                using (var streamReader = new StreamReader(memoryStream, Encoding.ASCII, true, 1024, true))
                    _output.WriteLine(await streamReader.ReadToEndAsync());
                memoryStream.Seek(0, SeekOrigin.Begin);
                
                using (var fromBase64Transform = new FromBase64Transform())
                using (var decodedStream = new CryptoStream(memoryStream, fromBase64Transform, CryptoStreamMode.Read))
                using (var streamReader = new StreamReader(decodedStream))
                {
                    var data = await streamReader.ReadToEndAsync();
                    Assert.Equal(expectedData, data);
                }
            }
        }
    }
}
