namespace BlobLogWriterMinApi.Classes
{
    public class BlobServiceOptions
    {
        public string BlobConnectionString { get; set; }
        public string QueueConnectionString { get; set; }
        public string ContainerName { get; set; }
    }

}
