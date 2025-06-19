using DashRender.Class;
using DashRender.Enum;
using SkiaSharp;

namespace DashRender
{
    public class DashboardImageRenderer
    {
        public static string ExportDashboardImageAsBase64(DashboardDefinition dashboard)
        {
            using var bitmap = GenerateDashboardBitmap(dashboard);
            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);

            return Convert.ToBase64String(data.ToArray());
        }

        public static void ExportDashboardImageToFile(DashboardDefinition dashboard, string outputFilePath)
        {
            using var bitmap = GenerateDashboardBitmap(dashboard);
            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);

            Directory.CreateDirectory(Path.GetDirectoryName(outputFilePath)!);
            using var stream = File.OpenWrite(outputFilePath);
            data.SaveTo(stream);
        }

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

            int totalWidth = (chartWidth * columns) + (spacingX * (columns + 1));
            int totalHeight = 300 + (chartHeight * rows) + (spacingY * (rows + 1));

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
                int row = index / columns;
                int col = index % columns;

                int offsetX = spacingX + col * (chartWidth + spacingX);
                int offsetY = 200 + spacingY + row * (chartHeight + spacingY);

                DrawChart(canvas, chart, offsetX, offsetY, chartWidth, chartHeight);
            }

            return bitmap;
        }

        public static void DrawChart(SKCanvas canvas, ChartDefinition chart, int offsetX, int offsetY, int width, int height)
        {
            using var titlePaint = new SKPaint
            {
                Color = SKColors.Black,
                TextSize = 28,
                IsAntialias = true,
                TextAlign = SKTextAlign.Center
            };

            canvas.DrawText(chart.Title ?? "", offsetX + width / 2f, offsetY + 30, titlePaint);

            switch (chart.ChartType)
            {
                case ChartType.Pie:
                    DrawPieChart(canvas, chart, offsetX, offsetY + 50, width, height - 50);
                    break;
                case ChartType.VerticalBar:
                    DrawVerticalBars(canvas, chart, offsetX, offsetY + 50, width, height - 50);
                    break;
                case ChartType.HorizontalBar:
                    DrawSideBars(canvas, chart, offsetX, offsetY + 50, width, height - 50);
                    break;
            }
        }

        public static void DrawPieChart(SKCanvas canvas, ChartDefinition chart, int offsetX, int offsetY, int width, int height)
        {
            decimal total = chart.DataPoints.Sum(e => e.Value);

            float centerX = chart.DisplayLegend ? offsetX + width / 3f : offsetX + width / 2f;
            float centerY = offsetY + height / 2f;
            float radius = Math.Min(width, height) / 3f;

            SKColor[] colors = { SKColors.SteelBlue, SKColors.OrangeRed, SKColors.SeaGreen, SKColors.Goldenrod, SKColors.MediumPurple };

            float startAngle = 0;
            int colorIndex = 0;

            using var textPaint = new SKPaint
            {
                Color = SKColors.White,
                TextSize = 18,
                IsAntialias = true,
                TextAlign = SKTextAlign.Center
            };

            foreach (var entity in chart.DataPoints)
            {
                float sweepAngle = (float)(entity.Value / total) * 360f;

                using var paint = new SKPaint
                {
                    Color = colors[colorIndex % colors.Length],
                    IsAntialias = true,
                    Style = SKPaintStyle.Fill
                };

                using var path = new SKPath();
                path.MoveTo(centerX, centerY);
                path.ArcTo(new SKRect(centerX - radius, centerY - radius, centerX + radius, centerY + radius), startAngle, sweepAngle, false);
                path.Close();

                canvas.DrawPath(path, paint);

                float midAngle = startAngle + sweepAngle / 2;
                double radians = (Math.PI / 180) * midAngle;
                float textX = centerX + (float)(Math.Cos(radians) * radius * 0.6f);
                float textY = centerY + (float)(Math.Sin(radians) * radius * 0.6f);

                string label = chart.DisplayLegend
                    ? FormatValueForLabel(entity.Value, entity.Unit)
                    : $"{entity.Label}: {FormatValueForLabel(entity.Value, entity.Unit)}";

                textPaint.Color = paint.Color.Red + paint.Color.Green + paint.Color.Blue > 600 ? SKColors.Black : SKColors.White;
                canvas.DrawText(label, textX, textY, textPaint);

                startAngle += sweepAngle;
                colorIndex++;
            }

            if (chart.DisplayLegend)
                DrawLegendVertical(canvas, chart, offsetX + (width * 2 / 3) + 20, offsetY + 20, colors);
        }

        public static void DrawVerticalBars(SKCanvas canvas, ChartDefinition chart, int offsetX, int offsetY, int width, int height)
        {
            decimal max = chart.DataPoints.Max(e => e.Value);
            DrawVerticalScale(canvas, offsetX, offsetY, width, height, max);

            float barWidth = (width - 100) / (chart.DataPoints.Count * 2f);
            SKColor[] colors = { SKColors.SteelBlue, SKColors.OrangeRed, SKColors.SeaGreen };

            using var paint = new SKPaint { IsAntialias = true };
            using var textPaint = new SKPaint
            {
                Color = SKColors.Black,
                TextSize = 16,
                IsAntialias = true,
                TextAlign = SKTextAlign.Center
            };

            for (int i = 0; i < chart.DataPoints.Count; i++)
            {
                float barHeight = (float)(chart.DataPoints[i].Value / max) * (height - 50);
                float x = offsetX + 50 + i * (barWidth * 2);
                float y = offsetY + height - barHeight - 20;

                paint.Color = colors[i % colors.Length];
                canvas.DrawRect(x, y, barWidth, barHeight, paint);

                string label = chart.DisplayLegend
                    ? FormatValueForLabel(chart.DataPoints[i].Value, chart.DataPoints[i].Unit)
                    : $"{chart.DataPoints[i].Label}\n{FormatValueForLabel(chart.DataPoints[i].Value, chart.DataPoints[i].Unit)}";

                DrawMultiLineText(canvas, label, x + barWidth / 2, y - 10, textPaint);
            }

            if (chart.DisplayLegend)
            {
                float legendX = offsetX + 50;
                float legendY = offsetY + height + 20;
                DrawLegendHorizontal(canvas, chart, legendX, legendY, colors);
            }
        }

        public static void DrawSideBars(SKCanvas canvas, ChartDefinition chart, int offsetX, int offsetY, int width, int height)
        {
            decimal max = chart.DataPoints.Max(e => e.Value);
            DrawHorizontalScale(canvas, offsetX, offsetY, width, height, max);

            float barHeight = (height - 50) / (chart.DataPoints.Count * 1.5f);
            SKColor[] colors = { SKColors.SteelBlue, SKColors.OrangeRed, SKColors.SeaGreen };

            using var paint = new SKPaint { IsAntialias = true };
            using var textPaint = new SKPaint
            {
                Color = SKColors.Black,
                TextSize = 16,
                IsAntialias = true,
                TextAlign = SKTextAlign.Left
            };

            for (int i = 0; i < chart.DataPoints.Count; i++)
            {
                float barWidth = (float)(chart.DataPoints[i].Value / max) * (width - 200);
                float x = offsetX + 150;
                float y = offsetY + i * (barHeight * 1.5f);

                paint.Color = colors[i % colors.Length];
                canvas.DrawRect(x, y, barWidth, barHeight, paint);

                string label = chart.DisplayLegend
                    ? FormatValueForLabel(chart.DataPoints[i].Value, chart.DataPoints[i].Unit)
                    : $"{chart.DataPoints[i].Label}: {FormatValueForLabel(chart.DataPoints[i].Value, chart.DataPoints[i].Unit)}";

                float textWidth = textPaint.MeasureText(label);
                canvas.DrawText(label, x - 5 - textWidth, y + barHeight / 1.5f, textPaint);
            }

            if (chart.DisplayLegend)
            {
                float legendX = offsetX + 150;
                float legendY = offsetY + height + 20;
                DrawLegendHorizontal(canvas, chart, legendX, legendY, colors);
            }
        }

        public static void DrawLegendVertical(SKCanvas canvas, ChartDefinition chart, float startX, float startY, SKColor[] colors)
        {
            using var paint = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill
            };

            using var textPaint = new SKPaint
            {
                Color = SKColors.Black,
                TextSize = 18,
                IsAntialias = true,
                TextAlign = SKTextAlign.Left
            };

            float boxSize = 20;
            float padding = 8;
            float lineHeight = textPaint.TextSize + 10;

            for (int i = 0; i < chart.DataPoints.Count; i++)
            {
                var entity = chart.DataPoints[i];
                paint.Color = colors[i % colors.Length];

                float y = startY + i * lineHeight;

                canvas.DrawRect(startX, y, boxSize, boxSize, paint);

                string label = $"{entity.Label}: {FormatValueForLabel(entity.Value, entity.Unit)}";
                canvas.DrawText(label, startX + boxSize + padding, y + boxSize - 5, textPaint);
            }
        }
        public static void DrawLegendHorizontal(SKCanvas canvas, ChartDefinition chart, float startX, float startY, SKColor[] colors)
        {
            using var paint = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill
            };

            using var textPaint = new SKPaint
            {
                Color = SKColors.Black,
                TextSize = 18,
                IsAntialias = true,
                TextAlign = SKTextAlign.Left
            };

            float boxSize = 20;
            float padding = 8;
            float spaceBetweenItems = 20;

            float currentX = startX;
            float y = startY;

            for (int i = 0; i < chart.DataPoints.Count; i++)
            {
                var entity = chart.DataPoints[i];
                paint.Color = colors[i % colors.Length];

                canvas.DrawRect(currentX, y, boxSize, boxSize, paint);

                string label = $"{entity.Label}: {FormatValueForLabel(entity.Value, entity.Unit)}";
                canvas.DrawText(label, currentX + boxSize + padding, y + boxSize - 5, textPaint);

                currentX += boxSize + padding + textPaint.MeasureText(label) + spaceBetweenItems;
            }
        }
        public static void DrawVerticalScale(SKCanvas canvas, int offsetX, int offsetY, int width, int height, decimal maxValue)
        {
            using var paint = new SKPaint { Color = SKColors.LightGray, StrokeWidth = 1, IsAntialias = true };
            using var textPaint = new SKPaint { Color = SKColors.Gray, TextSize = 14, IsAntialias = true, TextAlign = SKTextAlign.Right };

            int numberOfLines = 5;
            for (int i = 0; i <= numberOfLines; i++)
            {
                float y = offsetY + height - 20 - (i * (height - 50) / numberOfLines);
                canvas.DrawLine(offsetX + 40, y, offsetX + width - 10, y, paint);

                int value = (int)(maxValue / numberOfLines * i);
                canvas.DrawText(value.ToString(), offsetX + 35, y + 5, textPaint);
            }
        }
        public static void DrawHorizontalScale(SKCanvas canvas, int offsetX, int offsetY, int width, int height, decimal maxValue)
        {
            using var paint = new SKPaint { Color = SKColors.LightGray, StrokeWidth = 1, IsAntialias = true };
            using var textPaint = new SKPaint { Color = SKColors.Gray, TextSize = 14, IsAntialias = true, TextAlign = SKTextAlign.Center };

            int numberOfLines = 5;
            for (int i = 0; i <= numberOfLines; i++)
            {
                float x = offsetX + 150 + (i * (width - 200) / numberOfLines);
                float yStart = offsetY;
                float yEnd = offsetY + height - 20;

                canvas.DrawLine(x, yStart, x, yEnd, paint);

                int value = (int)(maxValue / numberOfLines * i);
                canvas.DrawText(value.ToString(), x, yEnd + 20, textPaint);
            }
        }

        public static void DrawMultiLineText(SKCanvas canvas, string text, float x, float y, SKPaint paint)
        {
            string[] lines = text.Split('\n');
            float lineHeight = paint.TextSize + 2;
            float totalHeight = lines.Length * lineHeight;
            float startY = y - totalHeight / 2 + paint.TextSize;

            foreach (var line in lines)
            {
                canvas.DrawText(line, x, startY, paint);
                startY += lineHeight;
            }
        }

        public static string FormatValueForLabel(decimal quantity, string? symbol)
        {
            if (string.IsNullOrEmpty(symbol))
                return ((int)quantity).ToString();

            if (symbol == "R$")
                return $"{symbol} {quantity:N2}";

            if (symbol == "%")
                return $"{quantity:N2}%";

            return quantity.ToString("N2");
        }

        public static DashboardDefinition GetDashboardDataTest()
        {
            return new DashboardDefinition
            {
                Title = "Dashboard de Performance Comercial",
                Subtitle = "Resultados Consolidado - 1º Semestre 2025",
                Charts =
[
        new() {
            Title = "Vendas por Filial (R$)",
            ChartType = ChartType.Pie,
            DisplayLegend = true,
            DataPoints =
            [
                new() { Label = "São Paulo", Value = 35200.75m, Unit = "R$" },
                new() { Label = "Rio de Janeiro", Value = 27450.30m, Unit = "R$" },
                new() { Label = "Bahia", Value = 19870.00m, Unit = "R$" },
                new() { Label = "Minas Gerais", Value = 15800.90m, Unit = "R$" }
            ]
        },
        new() {
            Title = "Faturamento por Região (R$)",
            ChartType = ChartType.Pie,
            DisplayLegend = true,
            DataPoints =
            [
                new() { Label = "Sudeste", Value = 82650.45m, Unit = "R$" },
                new() { Label = "Nordeste", Value = 39520.20m, Unit = "R$" },
                new() { Label = "Sul", Value = 28790.60m, Unit = "R$" }
            ]
        },
        new() {
            Title = "Vendas por Produto (%)",
            ChartType = ChartType.VerticalBar,
            DisplayLegend = true,
            DataPoints =
            [
                new() { Label = "Produto A", Value = 35.5m, Unit = "%" },
                new() { Label = "Produto B", Value = 45.2m, Unit = "%" },
                new() { Label = "Produto C", Value = 19.3m, Unit = "%" }
            ]
        },
        new() {
            Title = "Atendimentos por Canal",
            ChartType = ChartType.HorizontalBar,
            DisplayLegend = true,
            DataPoints =
            [
                new() { Label = "Telefone", Value = 1200, Unit = "" },
                new() { Label = "WhatsApp", Value = 950, Unit = "" },
                new() { Label = "E-mail", Value = 700, Unit = "" },
                new() { Label = "Presencial", Value = 400, Unit = "" }
            ]
        },
        new() {
            Title = "Taxa de Conversão por Filial (%)",
            ChartType = ChartType.HorizontalBar,
            DisplayLegend = true,
            DataPoints =
            [
                new() { Label = "São Paulo", Value = 68.3m, Unit = "%" },
                new() { Label = "Rio de Janeiro", Value = 54.7m, Unit = "%" },
                new() { Label = "Bahia", Value = 49.2m, Unit = "%" }
            ]
        },
        new() {
            Title = "Crescimento Mensal de Vendas (R$)",
            ChartType = ChartType.VerticalBar,
            DisplayLegend = true,
            DataPoints =
            [
                new() { Label = "Janeiro", Value = 12000m, Unit = "R$" },
                new() { Label = "Fevereiro", Value = 14500m, Unit = "R$" },
                new() { Label = "Março", Value = 13200m, Unit = "R$" },
                new() { Label = "Abril", Value = 16000m, Unit = "R$" },
                new() { Label = "Maio", Value = 17050m, Unit = "R$" },
                new() { Label = "Junho", Value = 18900m, Unit = "R$" }
            ]
        }
    ]
            };
        }
    }
}