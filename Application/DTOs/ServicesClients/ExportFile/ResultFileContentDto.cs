namespace Application.DTOs.ServicesClients.ExportFile
{
    public class ResultFileContentDto
    {
        public string FileName { get; set; }
        public string MimeType { get; set; }
        public byte[] Content { get; set; }
    }
}
