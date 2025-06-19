
# 📊 DashRender

**DashRender** é uma biblioteca .NET que permite gerar **dashboards gráficos como imagens (PNG)** utilizando o **SkiaSharp**. Ideal para cenários onde você precisa embutir gráficos em e-mails, relatórios PDF, APIs ou aplicações web.

> Criado para gerar dashboards simples e rápidos, sem necessidade de ferramentas externas de BI.

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

Você precisa montar um **objeto de entrada** do tipo `DashboardDefinition`, que representa o seu dashboard.

### Estrutura de `DashboardDefinition`

| Propriedade | Tipo | Descrição |
|------------|----|----|
| `Title` | string | Título principal do dashboard |
| `Subtitle` | string | Subtítulo (ex: período ou região) |
| `Charts` | List<ChartDefinition> | Lista de gráficos |
| `Colors` | SKColor[] (Opcional) | Paleta de cores personalizada para os gráficos |

---

### Detalhes sobre as cores (`Colors`)

- **Se você não informar o array `Colors`**, a biblioteca automaticamente irá gerar uma sequência de **cores randômicas** a partir de uma paleta completa de cores baseadas no padrão `SkiaSharp.SKColors`.

### 📋 Exemplos de Cores Disponíveis

- Red
- Blue
- Green
- Orange
- Yellow
- Purple
- Cyan
- Magenta
- DarkGreen
- Gold
- RoyalBlue
- ... *(veja todas as demais cores disponíveis no namespace `SkiaSharp.SKColors`)*

---

## 📂 Exportando para Imagem

Se quiser gerar a imagem, informe o caminho de saída.

### Exemplo de Uso:

```csharp
DashboardImageRenderer.ExportDashboardImageToFile(dashboard, @"C:\Relatorios\dashboard-vendas.png");
```

### Exemplo de Resultado:

![dashboard_grid](https://github.com/user-attachments/assets/8f7c0b2e-7dc9-4053-aa15-ad2226ca895b)

---

## 🖼️ Exportando como Base64

Se quiser gerar como Base64 (útil para envio via API ou embutir em HTML):

```csharp
string base64Image = DashboardImageRenderer.ExportDashboardImageAsBase64(dashboard);
```

---

## 🛠️ Exemplo Completo de Uso

```csharp
using DashRender;
using SkiaSharp;

var dashboard = new DashboardDefinition
{
    Title = "Dashboard de Performance Comercial",
    Subtitle = "Resultados Consolidados - 1º Semestre 2025",
    Colors = new SKColor[]
    {
        SKColors.Red,
        SKColors.Green,
        SKColors.Blue,
        SKColors.Orange
    },
    Charts =
    [
        new()
        {
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
        new()
        {
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

// Exporta como arquivo
DashboardImageRenderer.ExportDashboardImageToFile(dashboard, @"C:\Relatorios\dashboard-performance.png");

// Ou exporta como Base64
string base64 = DashboardImageRenderer.ExportDashboardImageAsBase64(dashboard);
```

---

## 🎨 Tipos de Gráfico Suportados (`ChartType`)

- **Pie** → Gráfico de Pizza
- **VerticalBar** → Barras Verticais
- **HorizontalBar** → Barras Horizontais

---

## 📌 Requisitos

- .NET 6.0 ou superior
- SkiaSharp (adicionado automaticamente via NuGet)

---

## 🤝 Contribuições

Pull Requests, melhorias de design ou novos tipos de gráfico são super bem-vindos!
