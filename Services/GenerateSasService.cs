using System;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Configuration;

namespace UploadFileBlob
{
    public class GenerateSasService
    {
        public  Uri GetServiceSasUri(string containerName, string storedPolicyName = null)
        {
            var _blobService = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=sestoragestaging;AccountKey=1S9WfPptQ7uiR3MUE9NZQbMgSaDFbcnUdPljF2ujrwJg7Xzs8RvN/WYAW75FU/pLt7JKdoRUy3RmRzPZZ1fMyw==;EndpointSuffix=core.windows.net");
            var container = _blobService.GetBlobContainerClient(containerName);

            // Check whether this BlobClient object has been authorized with Shared Key.
            if (container.CanGenerateSasUri)
            {
                // Create a SAS token that's valid for one hour.
                BlobSasBuilder sasBuilder = new BlobSasBuilder()
                {
                    BlobContainerName = containerName,
                    Resource = "c"
                };

                if (storedPolicyName == null)
                {
                    sasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddHours(24);
                    sasBuilder.SetPermissions(BlobSasPermissions.Read |
                                              BlobSasPermissions.Write);
                }
                else
                {
                    sasBuilder.Identifier = storedPolicyName;
                }

                Uri sasUri = container.GenerateSasUri(sasBuilder);

                return sasUri;
            }
            else
            {
                return null;
            }
        }

    }
}