// See https://aka.ms/new-console-template for more information
using ParallelismCodeChallenge2;

Console.WriteLine("Parallelism Code Challenge 2!");

List<string> filePaths = new List<string>
        {
            "file1.txt",
            "file2.txt",
            "file3.txt",
            "file4.txt",
            "file5.txt",
            "file6.txt",
            "file7.txt",
            "file8.txt",
            "file9.txt",
            "file10.txt"
        };

// Example line operation: Convert each line to uppercase
Func<string, string> lineOperationResult = lineResult => $"Line calculation: {lineResult}";

try
{
    FileProcessor processor = new FileProcessor();
    await processor.ProcessFilesAsync(filePaths, lineOperationResult);
    Console.WriteLine("File processing successfully completed.");
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}
