using DashRender.Class;
using DashRender.Enum;

namespace DashRender.Example
{
    public class DashboardDataTest
    {
        public static DashboardDefinition GetData()
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
