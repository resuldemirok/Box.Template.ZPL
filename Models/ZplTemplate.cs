using System.Collections.Generic;

namespace Box.Template.ZPL.Models;

public class ZplTemplate
{
    public string Code { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public Dictionary<string, ZplImage> Images { get; set; } = new();
}
