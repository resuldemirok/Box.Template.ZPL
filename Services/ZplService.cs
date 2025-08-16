using System.Collections.Generic;
using Box.Template.ZPL.Models;
using Box.Template.ZPL.Exceptions;
using Box.Template.ZPL.Utilities;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Text;

namespace Box.Template.ZPL.Services;

public class ZplService
{
    private readonly Dictionary<string, ZplTemplate> _templates = new();

    public void AddTemplate(ZplTemplate template)
    {
        _templates[template.Code] = template;
    }

    public string ProcessTemplate(string templateCode, Dictionary<string, string> parameters, Dictionary<string, ZplImage>? images = null)
    {
        if (!_templates.ContainsKey(templateCode))
            throw new ZplException("Template code not found.");

        var template = _templates[templateCode];
        var result = template.Content;

        foreach (var param in parameters)
            result = result.Replace($"{{{{{param.Key}}}}}", param.Value);

        if (images != null)
        {
            foreach (var kv in images)
            {
                ImageValidator.Validate(kv.Value);
                var zplImage = ConvertImageToZpl(kv.Value);
                result = result.Replace($"{{{{image.{kv.Key}}}}}", zplImage);
            }
        }

        return result;
    }

    private string ConvertImageToZpl(ZplImage image)
    {
        using var img = image.Path != null 
            ? Image.Load<Rgba32>(image.Path) 
            : Image.Load<Rgba32>(image.File!.OpenReadStream());

        img.Mutate(x => x.Resize(image.WidthDots, image.HeightDots));
        img.Mutate(x => x.Grayscale(GrayscaleMode.Bt709));

        int bytesPerRow = (img.Width + 7) / 8;
        int totalBytes = bytesPerRow * img.Height;
        byte[] imageData = new byte[totalBytes];

        img.ProcessPixelRows(accessor =>
        {
            for (int y = 0; y < accessor.Height; y++)
            {
                var rowSpan = accessor.GetRowSpan(y);
                for (int x = 0; x < rowSpan.Length; x++)
                {
                    var pixel = rowSpan[x];
                    bool isWhite = pixel.R != 0;

                    if (isWhite)
                    {
                        int byteIndex = (y * bytesPerRow) + (x / 8);
                        int bitPosition = x % 8;
                        imageData[byteIndex] |= (byte)(0x80 >> bitPosition);
                    }
                }
            }
        });

        var hexBuilder = new StringBuilder();
        
        foreach (var b in imageData)
            hexBuilder.AppendFormat("{0:X2}", b);

        return $"^GFA,{bytesPerRow},{totalBytes},{bytesPerRow},{hexBuilder}";
    }
}
    