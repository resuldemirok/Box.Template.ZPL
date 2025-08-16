using System;
using System.IO;
using Box.Template.ZPL.Models;

namespace Box.Template.ZPL.Utilities;

public static class ImageValidator
{
    public static void Validate(ZplImage image)
    {
        if (image.Path == null && image.File == null)
            throw new ArgumentException("Image must have either a path or a file.");

        if (image.Path != null && !File.Exists(image.Path))
            throw new FileNotFoundException($"Image file not found: {image.Path}");

        if (image.File != null && image.File.Length == 0)
            throw new ArgumentException("Uploaded file is empty.");
    }
}
