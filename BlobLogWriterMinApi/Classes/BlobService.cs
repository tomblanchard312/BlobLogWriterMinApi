using Azure.Storage.Blobs.Models;
namespace BlobLogWriterMinApi.Classes
{
    using Azure;
    using Azure.Storage.Blobs;
    using Azure.Storage.Blobs.Specialized;

    using CsvHelper;

    using System.Formats.Asn1;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Service for interacting with Azure Blob Storage.
    /// </summary>
    public class BlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

        /// <summary>
        /// Initializes a new instance of the BlobService class.
        /// </summary>
        /// <param name="options">Options containing connection string and container name.</param>
        public BlobService(BlobServiceOptions options)
        {
            _containerName = options.ContainerName;
            _blobServiceClient = new BlobServiceClient(options.BlobConnectionString);
        }
        /// <summary>
        /// Appends a log entry to an append blob in the specified Azure Blob Storage container.
        /// </summary>
        /// <param name="logContent">The content of the log entry.</param>
        public async Task LogAsync(string logContent)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var fileName = DateTime.Now.ToString("yyyy-MM-dd") + ".csv";
            var blobClient = containerClient.GetAppendBlobClient(fileName);

            if (!await containerClient.ExistsAsync())
            {
                await containerClient.CreateAsync(PublicAccessType.BlobContainer);
            }

            if (!await blobClient.ExistsAsync())
            {
                await blobClient.CreateAsync();
                // For a new file, write a header line.
                string blobHeaderLine = "Message,MessageType,MessageDateTime\n";
                var headerContentStream = new MemoryStream(Encoding.UTF8.GetBytes(blobHeaderLine));
                await blobClient.AppendBlockAsync(headerContentStream);
            }

            // Append the log content with a newline character to the specified append blob.
            string logEntry = logContent + "\n";
            var logEntryContentStream = new MemoryStream(Encoding.UTF8.GetBytes(logEntry));
            await blobClient.AppendBlockAsync(logEntryContentStream);
        }


        /// <summary>
        /// Lists blob files in the container, sorted by creation date.
        /// </summary>
        /// <param name="orderByDescending">Set to true for descending order; false for ascending order.</param>
        /// <returns>A collection of blob items sorted by creation date.</returns>
        public IEnumerable<BlobItem> ListBlobs(bool orderByDescending)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobItems = containerClient.GetBlobs();

            if (orderByDescending)
            {
                // Sort the blob items by creation date in descending order.
                return blobItems.OrderByDescending(item => item.Properties.CreatedOn);
            }
            else
            {
                // Sort the blob items by creation date in ascending order.
                return blobItems.OrderBy(item => item.Properties.CreatedOn);
            }
        }
        /// <summary>
        /// Download the content of a blob.
        /// </summary>
        /// <param name="blobName">Name of the blob to download.</param>
        /// <returns>The content of the blob as a byte array, or null if the blob does not exist.</returns>
        public string ReadBlobData(string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            try
            {
                if (!blobClient.Exists())
                {
                    return null; // Blob does not exist.
                }


                BlobDownloadResult downloadResult = blobClient.DownloadContent();
                string blobContents = downloadResult.Content.ToString();
                return blobContents;
            }
            catch (RequestFailedException ex)
            {
                // Log the exception for debugging.
                Console.WriteLine($"Error downloading blob: {ex.Message}");
                return null;
            }
        }
        public List<LogMessage> ReadBlobDataSerialized(string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            if (!blobClient.Exists())
            {
                return null; // CSV file not found.
            }

            var response = blobClient.OpenRead();
            using var reader = new StreamReader(response);
            using var csv = new CsvReader(reader, new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture));

            return csv.GetRecords<LogMessage>().ToList();
        }
    }
}
