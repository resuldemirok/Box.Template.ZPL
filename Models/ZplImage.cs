namespace Box.Template.ZPL.Models;
using Microsoft.AspNetCore.Http;

public class ZplImage
{
    public string? Path { get; set; }
    public IFormFile? File { get; set; }
    public int WidthDots { get; set; } = 200;
    public int HeightDots { get; set; } = 200;
}
