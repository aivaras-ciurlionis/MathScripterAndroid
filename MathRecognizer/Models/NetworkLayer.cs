using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using Newtonsoft.Json;

namespace MathRecognizer.Models
{
    public class NetworkLayer
    {

        public NetworkLayer(int input, int output, string weightsFileContent, string biasesFileContent)
        {
            InputSize = input;
            OutputSize = output;
            Weights = DenseMatrix.OfArray(LoadWeightsFromFile(weightsFileContent));
            Biases = DenseVector.OfArray(LoadBiasesFromFile(biasesFileContent));
        }

        public int InputSize { get; set; }
        public int OutputSize { get; set; }
        public Matrix<double> Weights { get; set; }
        public Vector Biases { get; set; }

        private static double[,] LoadWeightsFromFile(string fileContent)
        {
            var result =  JsonConvert.DeserializeObject<double[,]>(fileContent);
            return result;
        }

        private static double[] LoadBiasesFromFile(string fileContent)
        {
            var result = JsonConvert.DeserializeObject<double[]>(fileContent);
            return result;
        }

    }
}