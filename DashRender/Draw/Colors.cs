using SkiaSharp;

namespace DashRender.Draw
{
    public class Colors
    {
        public static SKColor[] GetFullColorPalette(int requiredCount, SKColor[]? colorsUser)
        {
            List<SKColor> baseColors;

            if (colorsUser != null && colorsUser.Length > 0)
            {
                baseColors = colorsUser.ToList();
            }
            else
            {
                baseColors = new List<SKColor> {

                            SKColors.Azure,
                            SKColors.Beige,
                            SKColors.Bisque,
                            SKColors.Black,
                            SKColors.BlanchedAlmond,
                            SKColors.Blue,
                            SKColors.BlueViolet,
                            SKColors.Brown,
                            SKColors.BurlyWood,
                            SKColors.CadetBlue,
                            SKColors.Chartreuse,
                            SKColors.Chocolate,
                            SKColors.Coral,
                            SKColors.CornflowerBlue,
                            SKColors.Cornsilk,
                            SKColors.Crimson,
                            SKColors.Cyan,
                            SKColors.DarkBlue,
                            SKColors.DarkCyan,
                            SKColors.DarkGoldenrod,
                            SKColors.DarkGray,
                            SKColors.DarkGreen,
                            SKColors.DarkKhaki,
                            SKColors.DarkMagenta,
                            SKColors.DarkOliveGreen,
                            SKColors.DarkOrange,
                            SKColors.DarkOrchid,
                            SKColors.DarkRed,
                            SKColors.DarkSalmon,
                            SKColors.DarkSeaGreen,
                            SKColors.DarkSlateBlue,
                            SKColors.DarkSlateGray,
                            SKColors.DarkTurquoise,
                            SKColors.DarkViolet,
                            SKColors.DeepPink,
                            SKColors.DeepSkyBlue,
                            SKColors.DimGray,
                            SKColors.DodgerBlue,
                            SKColors.Firebrick,
                            SKColors.FloralWhite,
                            SKColors.ForestGreen,
                            SKColors.Fuchsia,
                            SKColors.Gainsboro,
                            SKColors.GhostWhite,
                            SKColors.Gold,
                            SKColors.Goldenrod,
                            SKColors.Gray,
                            SKColors.Green,
                            SKColors.GreenYellow,
                            SKColors.Honeydew,
                            SKColors.HotPink,
                            SKColors.IndianRed,
                            SKColors.Indigo,
                            SKColors.Ivory,
                            SKColors.Khaki,
                            SKColors.Lavender,
                            SKColors.LavenderBlush,
                            SKColors.LawnGreen,
                            SKColors.LemonChiffon,
                            SKColors.LightBlue,
                            SKColors.LightCoral,
                            SKColors.LightCyan,
                            SKColors.LightGoldenrodYellow,
                            SKColors.LightGray,
                            SKColors.LightGreen,
                            SKColors.LightPink,
                            SKColors.LightSalmon,
                            SKColors.LightSeaGreen,
                            SKColors.LightSkyBlue,
                            SKColors.LightSlateGray,
                            SKColors.LightSteelBlue,
                            SKColors.LightYellow,
                            SKColors.Lime,
                            SKColors.LimeGreen,
                            SKColors.Linen,
                            SKColors.Magenta,
                            SKColors.Maroon,
                            SKColors.MediumAquamarine,
                            SKColors.MediumBlue,
                            SKColors.MediumOrchid,
                            SKColors.MediumPurple,
                            SKColors.MediumSeaGreen,
                            SKColors.MediumSlateBlue,
                            SKColors.MediumSpringGreen,
                            SKColors.MediumTurquoise,
                            SKColors.MediumVioletRed,
                            SKColors.MidnightBlue,
                            SKColors.MintCream,
                            SKColors.MistyRose,
                            SKColors.Moccasin,
                            SKColors.NavajoWhite,
                            SKColors.Navy,
                            SKColors.OldLace,
                            SKColors.Olive,
                            SKColors.OliveDrab,
                            SKColors.Orange,
                            SKColors.OrangeRed,
                            SKColors.Orchid,
                            SKColors.PaleGoldenrod,
                            SKColors.PaleGreen,
                            SKColors.PaleTurquoise,
                            SKColors.PaleVioletRed,
                            SKColors.PapayaWhip,
                            SKColors.PeachPuff,
                            SKColors.Peru,
                            SKColors.Pink,
                            SKColors.Plum,
                            SKColors.PowderBlue,
                            SKColors.Purple,
                            SKColors.Red,
                            SKColors.RosyBrown,
                            SKColors.RoyalBlue,
                            SKColors.SaddleBrown,
                            SKColors.Salmon,
                            SKColors.SandyBrown,
                            SKColors.SeaGreen,
                            SKColors.SeaShell,
                            SKColors.Sienna,
                            SKColors.Silver,
                            SKColors.SkyBlue,
                            SKColors.SlateBlue,
                            SKColors.SlateGray,
                            SKColors.Snow,
                            SKColors.SpringGreen,
                            SKColors.SteelBlue,
                            SKColors.Tan,
                            SKColors.Teal,
                            SKColors.Thistle,
                            SKColors.Tomato,
                            SKColors.Turquoise,
                            SKColors.Violet,
                            SKColors.Wheat,
                            SKColors.Yellow,
                            SKColors.YellowGreen
        };
            }

            var random = new Random();
            baseColors = baseColors.OrderBy(c => random.Next()).ToList();

            var finalColors = new List<SKColor>();
            for (int i = 0; i < requiredCount; i++)
            {
                finalColors.Add(baseColors[i % baseColors.Count]);
            }

            return finalColors.ToArray();
        }
    }
}
