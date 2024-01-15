namespace Antoine.Andre.Ocr.Console
{
    using System;
    
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Veuillez fournir les chemins complets des images en argument.");
                return;
            }

            var ocrProcessor = new Ocr();

            foreach (var imagePath in args)
            {
                try
                {
                    // Read the image file as bytes
                    var imageBytes = File.ReadAllBytes(imagePath);

                    // Process the image using OCR
                    var ocrResults = ocrProcessor.Read(new List<byte[]> { imageBytes });

                    Console.WriteLine($"Résultats pour l'image : {imagePath}");
                    foreach (var ocrResult in ocrResults)
                    {
                        Console.WriteLine($"Confidence : {ocrResult.Confidence}");
                        Console.WriteLine($"Texte : {ocrResult.Text}");
                    }

                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors du traitement de l'image {imagePath}: {ex.Message}");
                }
            }
        }
    }
}