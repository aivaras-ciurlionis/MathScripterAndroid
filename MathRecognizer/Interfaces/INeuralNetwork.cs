namespace MathRecognizer.Interfaces
{
    public interface INeuralNetwork
    {
        int GetPrediction(byte[] input);
    }
}