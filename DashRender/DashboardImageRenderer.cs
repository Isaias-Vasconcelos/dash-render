using DashRender.Class;
using DashRender.Draw;
using DashRender.Example;
using SkiaSharp;

namespace DashRender
{
    public class DashboardImageRenderer
    {
        public static string ExportDashboardImageAsBase64(DashboardDefinition dashboard)
        {
            using var bitmap = BitMap.GenerateDashboardBitmap(dashboard);
            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);

            return Convert.ToBase64String(data.ToArray());
        }

        public static void ExportDashboardImageToFile(DashboardDefinition dashboard, string outputFilePath)
        {
            using var bitmap = BitMap.GenerateDashboardBitmap(dashboard);
            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);

            Directory.CreateDirectory(Path.GetDirectoryName(outputFilePath)!);
            using var stream = File.OpenWrite(outputFilePath);
            data.SaveTo(stream);
        }

        public static DashboardDefinition GetDataTest() => DashboardDataTest.GetData();
    }
}