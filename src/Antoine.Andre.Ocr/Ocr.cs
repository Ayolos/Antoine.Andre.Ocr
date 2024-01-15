using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Tesseract;

namespace Antoine.Andre.Ocr
{
    public class Ocr
    {
        private static string GetExecutingPath()
        {
            var executingAssemblyPath = Assembly.GetExecutingAssembly().Location;
            var executingPath = Path.GetDirectoryName(executingAssemblyPath);
            return executingPath;
        }

        public List<OcrResult> Read(IList<byte[]> images)
        {
            var executingPath = GetExecutingPath();
            var tessdataPath = Path.Combine(executingPath, @"tessdata");
            
            using var engine = new TesseractEngine(tessdataPath, "fra", EngineMode.Default);
            
            var ocrTasks = images.Select(image => Task.Run(() => ProcessImage(engine, image))).ToArray();
            
            Task.WaitAll(ocrTasks);

            return ocrTasks.Select(t => t.Result).ToList();
        }

        private OcrResult ProcessImage(TesseractEngine engine, byte[] image)
        {
            using var pix = Pix.LoadFromMemory(image);
            using var page = engine.Process(pix);
            
            var text = page.GetText();
            var confidence = page.GetMeanConfidence();

            return new OcrResult { Text = text, Confidence = confidence };
        }
    }
}