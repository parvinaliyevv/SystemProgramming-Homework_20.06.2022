namespace TaskManager.Services;

public static class ImageService
{
    private static ImageSourceConverter Converter { get; } = new();


    public static ImageSource? GetImageFromFullPath(string path)
    {
        var image = Converter.ConvertFromString(path) as ImageSource;

        return image;
    }

    public static ImageSource? GetImageFromByteArray(byte[] array)
    {
        var image = Converter.ConvertFrom(array) as ImageSource;

        return image;
    }
}
