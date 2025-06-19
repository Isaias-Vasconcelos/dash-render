using DashRender.Enum;

namespace DashRender.Class
{
    public class ChartDefinition
    {
        public string? Title { get; set; }
        public ChartType ChartType { get; set; }
        public List<ChartDataPoint>? DataPoints { get; set; }
        public bool DisplayLegend { get; set; } = false;
    }
}
