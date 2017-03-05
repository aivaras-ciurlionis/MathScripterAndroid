namespace MathRecognizer.Interfaces
{
    public interface IImageMover
    {
        byte[] MovePixels(byte[] pixels, int width, int height, int offsetX, int offsetY);
    }
}