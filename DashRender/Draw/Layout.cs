using DashRender.Class;
using DashRender.Enum;
using SkiaSharp;

namespace DashRender.Draw
{
    public class Layout
    {
        public static void DrawChart(SKCanvas canvas, ChartDefinition chart, SKColor[]? colors, int offsetX, int offsetY, int width, int height)
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
                    DrawPieChart(canvas, chart, colors, offsetX, offsetY + 50, width, height - 50);
                    break;
                case ChartType.VerticalBar:
                    DrawVerticalBars(canvas, chart, colors, offsetX, offsetY + 50, width, height - 50);
                    break;
                case ChartType.HorizontalBar:
                    DrawSideBars(canvas, chart, colors, offsetX, offsetY + 50, width, height - 50);
                    break;
            }
        }

        public static void DrawPieChart(SKCanvas canvas, ChartDefinition chart, SKColor[]? colorsDash, int offsetX, int offsetY, int width, int height)
        {
            decimal total = chart.DataPoints.Sum(e => e.Value);

            float centerX = chart.DisplayLegend ? offsetX + width / 3f : offsetX + width / 2f;
            float centerY = offsetY + height / 2f;
            float radius = Math.Min(width, height) / 3f;

            SKColor[] colors = Colors.GetFullColorPalette(chart.DataPoints.Count, colorsDash);

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
                double radians = Math.PI / 180 * midAngle;
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
                DrawLegendVertical(canvas, chart, offsetX + width * 2 / 3 + 20, offsetY + 20, colors, 12);
        }

        public static void DrawVerticalBars(SKCanvas canvas, ChartDefinition chart, SKColor[]? colorsDash, int offsetX, int offsetY, int width, int height)
        {
            decimal max = chart.DataPoints.Max(e => e.Value);
            DrawVerticalScale(canvas, offsetX, offsetY, width, height, max);

            float barWidth = (width - 100) / (chart.DataPoints.Count * 2f);
            SKColor[] colors = Colors.GetFullColorPalette(chart.DataPoints.Count, colorsDash);

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

        public static void DrawSideBars(SKCanvas canvas, ChartDefinition chart, SKColor[]? colorsDash, int offsetX, int offsetY, int width, int height)
        {
            decimal max = chart.DataPoints.Max(e => e.Value);
            DrawHorizontalScale(canvas, offsetX, offsetY, width, height, max);

            float barHeight = (height - 50) / (chart.DataPoints.Count * 1.5f);
            SKColor[] colors = Colors.GetFullColorPalette(chart.DataPoints.Count, colorsDash);

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

        public static void DrawLegendVertical(SKCanvas canvas, ChartDefinition chart, float startX, float startY, SKColor[]? colors, int maxLine = 0)
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
            int MaxItemsPerLine = maxLine == 0 ? 3 : maxLine;

            float currentX = startX;
            float currentY = startY;

            for (int i = 0; i < chart.DataPoints.Count; i++)
            {
                var entity = chart.DataPoints[i];
                paint.Color = colors[i % colors.Length];

                if (i > 0 && i % MaxItemsPerLine == 0)
                {
                    currentY = startY;
                    currentX += 250;
                }

                canvas.DrawRect(currentX, currentY, boxSize, boxSize, paint);

                string label = $"{entity.Label}: {FormatValueForLabel(entity.Value, entity.Unit)}";
                canvas.DrawText(label, currentX + boxSize + padding, currentY + boxSize - 5, textPaint);

                currentY += lineHeight;
            }
        }

        public static void DrawLegendHorizontal(SKCanvas canvas, ChartDefinition chart, float startX, float startY, SKColor[]? colors)
        {
            const int MaxItemsPerLine = 3;

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
            float lineHeight = boxSize + 10;

            float currentX = startX;
            float currentY = startY;
            int itemsInCurrentLine = 0;

            for (int i = 0; i < chart.DataPoints.Count; i++)
            {
                var entity = chart.DataPoints[i];
                paint.Color = colors[i % colors.Length];

                string label = $"{entity.Label}: {FormatValueForLabel(entity.Value, entity.Unit)}";
                float textWidth = textPaint.MeasureText(label);
                float itemWidth = boxSize + padding + textWidth + spaceBetweenItems;

                if (itemsInCurrentLine >= MaxItemsPerLine)
                {
                    currentX = startX;
                    currentY += lineHeight;
                    itemsInCurrentLine = 0;
                }

                canvas.DrawRect(currentX, currentY, boxSize, boxSize, paint);

                canvas.DrawText(label, currentX + boxSize + padding, currentY + boxSize - 5, textPaint);

                currentX += itemWidth;
                itemsInCurrentLine++;
            }
        }

        public static void DrawVerticalScale(SKCanvas canvas, int offsetX, int offsetY, int width, int height, decimal maxValue)
        {
            using var paint = new SKPaint { Color = SKColors.LightGray, StrokeWidth = 1, IsAntialias = true };
            using var textPaint = new SKPaint { Color = SKColors.Gray, TextSize = 14, IsAntialias = true, TextAlign = SKTextAlign.Right };

            int numberOfLines = 5;
            for (int i = 0; i <= numberOfLines; i++)
            {
                float y = offsetY + height - 20 - i * (height - 50) / numberOfLines;
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
                float x = offsetX + 150 + i * (width - 200) / numberOfLines;
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
    }
}
