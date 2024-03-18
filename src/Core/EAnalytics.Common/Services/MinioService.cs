using EAnalytics.Common.Configurations;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace EAnalytics.Common.Services
{
    public interface IMinioService
    {
        Task<byte[]> Download(string bucketName, string link, CancellationToken cancellationToken = default);
        Task<string> Upload(string bucketName, string fileName, string path, MemoryStream stream, CancellationToken cancellationToken = default, bool withOriginalName = false);
    }

    public class MinioService : IMinioService
    {
        private readonly IMinioClientFactory _minioClientFactory;
        public MinioService(IMinioClientFactory minioClientFactory)
        {
            _minioClientFactory = minioClientFactory;
        }

        public async Task<string> Upload(string bucketName, string fileName, string path, MemoryStream stream, CancellationToken cancellationToken = default, bool withOriginalName = false)
        {
            if (string.IsNullOrWhiteSpace(bucketName))
                throw new ArgumentNullException(nameof(bucketName));

            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException(nameof(fileName));

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (string.IsNullOrWhiteSpace(Path.GetExtension(fileName)))
                throw new ArgumentException("File name must be with extension!");

            using var client = new MinioClient()
                                    .WithEndpoint(_minioOptions.Value.Endpoint)
                                    .WithCredentials(_minioOptions.Value.AccessKey, _minioOptions.Value.SecretKey)
                                    .WithSSL()
                                    .Build();

            var beArgs = new BucketExistsArgs()
                .WithBucket(bucketName);
            var foung = await client
                .BucketExistsAsync(beArgs, cancellationToken)
                .ConfigureAwait(false);

            if (!foung)
            {
                var mbArgs = new MakeBucketArgs()
                    .WithBucket(bucketName);
                await client.MakeBucketAsync(mbArgs, cancellationToken);
            }

            var fullName = GetObjectName(path, fileName, withOriginalName);

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fullName)
                .WithStreamData(stream)
                .WithObjectSize(stream.Length)
                .WithContentType("application/octet-stream");

            await client.PutObjectAsync(putObjectArgs, cancellationToken).ConfigureAwait(false);

            return fullName;
        }

        public async Task<byte[]> Download(string bucketName, string link, CancellationToken cancellationToken = default)
        {
            using var client = new MinioClient()
                                    .WithEndpoint(_minioOptions.Value.Endpoint)
                                    .WithCredentials(_minioOptions.Value.AccessKey, _minioOptions.Value.SecretKey)
                                    .Build();

            using var stream = new MemoryStream();

            var getObjectArgs = new GetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(link)
                .WithCallbackStream(st => st.CopyTo(stream));

            await client.GetObjectAsync(getObjectArgs, cancellationToken).ConfigureAwait(false);

            if (stream is null || stream.Length == 0)
                throw new FileNotFoundException("File not found!");

            stream.Position = 0;
            return stream.ToArray();
        }

        private string GetObjectName(string path, string name, bool withOriginalName = false)
        {
            var guid = "";
            var ext = "";

            if (withOriginalName)
            {
                guid = Path.GetFileNameWithoutExtension(name);
                ext = Path.GetExtension(name);
            }
            else
            {
                guid = Guid.NewGuid().ToString();
                ext = Path.GetExtension(name);
            }

            if (string.IsNullOrWhiteSpace(path))
                return guid + ext;

            if (path.EndsWith("/"))
                return path + guid + ext;

            return $"{path}/{guid}{ext}";
        }
    }
}
