using SkiaSharp;

namespace DashRender.Class
{
    public class DashboardDefinition
    {
        public string? Title { get; set; }
        public string? Subtitle { get; set; }
        public SKColor[]? Colors { get; set; }
        public List<ChartDefinition>? Charts { get; set; }
    }
}