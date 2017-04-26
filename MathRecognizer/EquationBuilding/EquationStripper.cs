using System.Linq;
using System.Text.RegularExpressions;
using MathRecognizer.Interfaces;

namespace MathRecognizer.EquationBuilding
{
    public class EquationStripper : IEquationStripper
    {
        public string StripEquation(string equation)
        {
            const string innnerPattern = @"(\.|,){2,}";
            const string endPattern = @"(\.|,|\*)+$";
            var parenthesisSplit = Regex.Split(equation, @"(?<=[\)])");
            if (parenthesisSplit.Length > 1 && !parenthesisSplit[0].Contains("("))
            {
                equation = string.Join("", parenthesisSplit.Skip(1));
            }
            var innerResult = Regex.Replace(equation, innnerPattern, "");
            var endResult = Regex.Replace(innerResult, endPattern, "");
            return endResult;

        }
    }
}