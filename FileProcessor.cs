using System.Text.RegularExpressions;

namespace ParallelismCodeChallenge2
{
    public class FileProcessor
    {
        // Constants
        const string PATTERN = @"[\+\-\\*\/p]";
        const string ADD = "+";
        const string SUBTRACT = "-";
        const string MULTIPLY = "*";
        const string DIVIDE = "/";
        const string POWER = "p";

        // Calculator operations
        // Add
        private readonly Func<double, double, double> add = (num1, num2) => num1 + num2;
        // Subtract
        // TODO: Create subtract Lambda
        private readonly Func<double, double, double> subtract = (num1, num2) => num1 - num2;
        // Multiply
        // TODO: Create multiply Lambda
        private readonly Func<double, double, double> multiply = (num1, num2) => num1 * num2;
        // Divide
        // TODO: Create divide Lambda
        private readonly Func<double, double, double> divide = (num1, num2) => num1 / num2;
        // Power
        // TODO: Create power Lambda
        private readonly Func<double, double, double> power = (num1, num2) => Math.Pow(num1, num2);

        private double CalculateLineOperation(string line)
        {
            // TODO: Complete the method implementation
            // 1. Feel free to use the existing class constants
            // 2. This method receives the file text line and should identify the operation and perform it, returning the final result
            // 3. Remember to use the lambda functions to complete the arithmetic calculation

            var match = Regex.Match(line, PATTERN);
            string[] numbers;
            switch (match.Groups[0].Value)
            {
                case ADD:
                    numbers = line.Split(ADD);
                    return add(Convert.ToDouble(numbers[0]), Convert.ToDouble(numbers[1]));
                case SUBTRACT:
                    numbers = line.Split(SUBTRACT);
                    return subtract(Convert.ToDouble(numbers[0]), Convert.ToDouble(numbers[1]));
                case MULTIPLY:
                    numbers = line.Split(MULTIPLY);
                    return multiply(Convert.ToDouble(numbers[0]), Convert.ToDouble(numbers[1]));
                case DIVIDE:
                    numbers = line.Split(DIVIDE);
                    return divide(Convert.ToDouble(numbers[0]), Convert.ToDouble(numbers[1]));
                case POWER:
                    numbers = line.Split(POWER);
                    return power(Convert.ToDouble(numbers[0]), Convert.ToDouble(numbers[1]));
                default:
                    return 0;
            }
        }
        public async Task ProcessFilesAsync(List<string> filePaths, Func<string, string> lineOperation)
        {
            // Create tasks for processing each file concurrently
            var tasks = filePaths.Select(filePath => ProcessFileAsync(filePath, lineOperation));

            // Await all tasks to complete
            await Task.WhenAll(tasks);
        }

        public async Task ProcessFileAsync(string filePath, Func<string, string> lineOperation)
        {
            string[] lines;
            try
            {
                // Read all lines from the file asynchronously
                lines = await File.ReadAllLinesAsync(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file '{filePath}': {ex.Message}");
                throw;
            }

            // Process each line concurrently using the specified operation
            // TODO: You have to pass the following expression to the lineOperation lambda --> e.g. "Expression: 200*300 --> Result: 60000"
            // Then when opening the new file you will find something like this --> e.g. "Line calculation: Expression: 200*300 --> Result: 60000"
            // You need to use AsParrallel method to read all file lines in parallel and process each one with the corresponding operation
            // Resources:
            // https://learn.microsoft.com/en-us/dotnet/api/system.linq.parallelenumerable.asparallel?view=net-6.0
            // https://www.csharptutorial.net/csharp-linq/csharp-asparallel/

            var processedLines = lines.AsParallel().Select(l => {
                var exp = $"Expression: {l} --> Result: {CalculateLineOperation(l)}";
                return lineOperation(exp);
            });

            // Write the processed lines back to the file
            try
            {
                await File.WriteAllLinesAsync($"calculated_{filePath}", processedLines);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to file '{filePath}': {ex.Message}");
                throw;
            }
        }
    }
}
