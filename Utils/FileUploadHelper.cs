using System.IO;

public static class FileUploadHelper
{
    private static readonly string UploadCMDBFilesDirectory = 
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "CMDBFiles");
    private static readonly string UploadUDZipFilesDirectory = 
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "UDZipFiles");
    private static readonly string UploadAppTemplateFilesDirectory = 
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "AppTemplateFiles");
    
    private static readonly string UploadSSOTFilesDirectory = 
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "SSOTFiles");
    private static readonly Random Random = new Random();

    /// <summary>
    /// Returns a random single file path from the upload files directory
    /// </summary>
   private static string GetSingleRandomFile(string directory)
    {
        if (!Directory.Exists(directory))
            throw new DirectoryNotFoundException($"Directory not found: {directory}");

        var files = Directory.GetFiles(directory);
        if (files.Length == 0)
            throw new FileNotFoundException($"No files found in directory: {directory}");

        int index = Random.Next(files.Length);
        return files[index];
    }

    public static string GetRandomCMDBFile()
    {
        return GetSingleRandomFile(UploadCMDBFilesDirectory);
    }
    public static string GetRandomUDZipFile()
    {
        return GetSingleRandomFile(UploadUDZipFilesDirectory);
    }

    public static string GetRandomAppTemplateFile()
    {
        return GetSingleRandomFile(UploadAppTemplateFilesDirectory);
    }

    public static string GetRandomSSOTFile()
    {
        return GetSingleRandomFile(UploadSSOTFilesDirectory);
    }
}