using DashRender.Class;
using SkiaSharp;

namespace DashRender.Draw
{
    public class BitMap
    {
        public static SKBitmap GenerateDashboardBitmap(DashboardDefinition dashboard)
        {
            int totalCharts = dashboard.Charts.Count;

            int chartsPerRow = totalCharts switch
            {
                1 => 1,
                2 => 2,
                4 => 2,
                _ => 3
            };

            int chartWidth = 800;
            int chartHeight = 500;
            int spacingX = 50;
            int spacingY = 80;

            int columns = chartsPerRow;
            int rows = (int)Math.Ceiling(totalCharts / (float)columns);

            int totalWidth = chartWidth * columns + spacingX * (columns + 1);
            int totalHeight = 300 + chartHeight * rows + spacingY * (rows + 1);

            var bitmap = new SKBitmap(totalWidth, totalHeight);
            using var canvas = new SKCanvas(bitmap);
            canvas.Clear(SKColors.White);

            using var titlePaint = new SKPaint
            {
                Color = SKColors.Black,
                TextSize = 64,
                IsAntialias = true,
                TextAlign = SKTextAlign.Center
            };

            using var subTitlePaint = new SKPaint
            {
                Color = SKColors.Gray,
                TextSize = 42,
                IsAntialias = true,
                TextAlign = SKTextAlign.Center
            };

            canvas.DrawText(dashboard.Title, totalWidth / 2f, 80, titlePaint);
            canvas.DrawText(dashboard.Subtitle, totalWidth / 2f, 150, subTitlePaint);

            for (int index = 0; index < totalCharts; index++)
            {
                var chart = dashboard.Charts[index];
                var colors = dashboard.Colors;
                int row = index / columns;
                int col = index % columns;

                int offsetX = spacingX + col * (chartWidth + spacingX);
                int offsetY = 200 + spacingY + row * (chartHeight + spacingY);

                Layout.DrawChart(canvas, chart, colors, offsetX, offsetY, chartWidth, chartHeight);
            }

            return bitmap;
        }
    }
}
