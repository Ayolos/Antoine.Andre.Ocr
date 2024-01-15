namespace Antoine.Andre.Ocr.Tests;

using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

public class OcrUnitTest
{
    [Fact]
    public async Task ImagesShouldBeReadCorrectly() {
        var executingPath = GetExecutingPath();
        var images = new List<byte[]>();
        foreach (var imagePath in
                 Directory.EnumerateFiles(Path.Combine(executingPath, "images"))) {
            var imageBytes = await File.ReadAllBytesAsync(imagePath);
            images.Add(imageBytes);
        }
        var ocrResults = new Ocr().Read(images);
        Assert.Equal(ocrResults[0].Text, @"ampleur. La pénurie n'est pas générale, il faut le rappeler. Certains profils sont plus recherchés que d'autres."); 
        Assert.Equal(ocrResults[0].Confidence, 1); 
        Assert.Equal(ocrResults[1].Text, @"Dans de nombreuses technologies, il existe des certifications. Le monde Microsoft en propose de nombreuses pour"); 
        Assert.Equal(ocrResults[1].Confidence, 1); 
        Assert.Equal(ocrResults[2].Text, @"Certaines le sont depuis cet été, d'autres prendront fin en janvier 2021. Liste complète des certifications et examens retirés :"); 
        Assert.Equal(ocrResults[2].Confidence, 1);
    }
    private static string GetExecutingPath() {
        var executingAssemblyPath = Assembly.GetExecutingAssembly().Location;
        var executingPath = Path.GetDirectoryName(executingAssemblyPath);
        return executingPath;
    }
}