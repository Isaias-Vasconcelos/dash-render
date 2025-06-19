
# 📊 DashRender

**DashRender** é uma biblioteca .NET que permite gerar **dashboards gráficos como imagens (PNG)** utilizando o **SkiaSharp**. Ideal para cenários onde você precisa embutir gráficos em e-mails, relatórios PDF, APIs ou aplicações web.

> Este pacote foi criado para facilitar a geração de dashboards simples, sem necessidade de ferramentas externas de BI.

---

## 📦 Instalação via NuGet

```bash
dotnet add package DashRender
```

Ou via Visual Studio:

```
Gerenciador de Pacotes NuGet > Procurar > DashRender
```

---

## ✅ Como Funciona

Você precisa montar um **objeto de entrada** do tipo `DashboardDefinition`, que representa o seu dashboard. Nele você define:

| Propriedade | Tipo | Descrição |
|------------|----|----|
| `Title` | string | Título principal do dashboard |
| `Subtitle` | string | Subtítulo (ex: período ou região) |
| `Charts` | List<ChartDefinition> | Lista de gráficos que compõem o dashboard |

Cada **ChartDefinition** define um gráfico individual:

| Propriedade | Tipo | Descrição |
|------------|----|----|
| `Title` | string | Título do gráfico |
| `ChartType` | ChartType | Tipo de gráfico (Pie, VerticalBar, HorizontalBar) |
| `DisplayLegend` | bool | Exibir ou não a legenda |
| `DataPoints` | List<DataPoint> | Os dados que vão alimentar o gráfico |

Cada **DataPoint** é uma entrada no gráfico:

| Propriedade | Tipo | Exemplo | Descrição |
|------------|----|----|----|
| `Label` | string | `"Bahia"` | Nome da categoria |
| `Value` | decimal | `2500.75m` | Valor numérico |
| `Unit` | string | `"R$"`, `"%"` | Unidade opcional |

---

## 🛠️ Exemplo Completo de Uso

```csharp
using DashRender;

var dashboard = new DashboardDefinition
{
    Title = "Dashboard de Vendas",
    Subtitle = "Relatório Semanal",
    Charts = new List<ChartDefinition>
    {
        new()
        {
            Title = "Vendas por Região",
            ChartType = ChartType.Pie,
            DisplayLegend = true,
            DataPoints = new List<DataPoint>
            {
                new() { Label = "Nordeste", Value = 2500.50m, Unit = "R$" },
                new() { Label = "Sudeste", Value = 1800.75m, Unit = "R$" },
                new() { Label = "Sul", Value = 1200.25m, Unit = "R$" }
            }
        },
        new()
        {
            Title = "Performance por Produto",
            ChartType = ChartType.VerticalBar,
            DisplayLegend = true,
            DataPoints = new List<DataPoint>
            {
                new() { Label = "Produto A", Value = 1500.10m, Unit = "%" },
                new() { Label = "Produto B", Value = 2500.20m, Unit = "%" },
                new() { Label = "Produto C", Value = 1000.30m, Unit = "%" }
            }
        }
    }
};

// Gerar imagem como arquivo PNG
DashboardRenderer.ExportToFile(dashboard, "caminho/output/dashboard.png");

// Ou gerar como Base64 string
string base64Image = DashboardRenderer.ExportToBase64(dashboard);
```

---

## ✨ Principais Métodos da Biblioteca

| Método | Descrição |
|----|----|
| `ExportToFile(DashboardDefinition dashboard, string outputFilePath)` | Gera a imagem e salva em disco como `.png`. |
| `ExportToBase64(DashboardDefinition dashboard)` | Retorna a imagem gerada como string Base64. Ideal para envio via API ou email. |

---

## 🎨 Tipos de Gráfico Suportados (ChartType)

- **Pie**: Gráfico de pizza
- **VerticalBar**: Barras verticais
- **HorizontalBar**: Barras horizontais

---

## 📌 Requisitos

- .NET 6.0 ou superior
- SkiaSharp (já incluído como dependência via NuGet)

---

## 🤝 Contribuições

Pull requests e sugestões são bem-vindos.
