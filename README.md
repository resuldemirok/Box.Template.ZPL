# Box.Template.ZPL

Box.Template.ZPL is a .NET library for creating ZPL (Zebra Programming Language) labels using template-based approach. It supports dynamic template processing with parameters and images, including IFormFile support and image resizing/validation.

With Box.Template.ZPL, you can:
- Define and manage dynamic ZPL templates
- Process templates with parameters to generate ZPL commands
- Include images from file paths or uploaded IFormFile streams
- Resize and validate images before converting to ZPL
- Use a strongly-typed, easy-to-use Package for label generation

---

## Installation

You can install the package via NuGet:

```powershell
dotnet add package Box.Template.ZPL
```

---

## Example Usage

Below is an example of how to use Box.Template.ZPL:

```csharp
using Box.Template.ZPL.Models;
using Box.Template.ZPL.Services;
using System.Collections.Generic;

var service = new ZplService();

service.AddTemplate(new ZplTemplate
{
    Code = "100",
    Content = "^XA\n^FO50,50^FD{{price}}^FS\n^FO100,200{{image.test}}\n^XZ"
});

var parameters = new Dictionary<string, string>
{
    { "price", "29.99" }
};

var images = new Dictionary<string, ZplImage>
{
    { "test", new ZplImage { Path = "images/test.png", WidthDots = 300, HeightDots = 300 } }
};

var zpl = service.ProcessTemplate("100", parameters, images);
Console.WriteLine(zpl);
```