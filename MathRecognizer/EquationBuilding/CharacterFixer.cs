using MathRecognizer.Interfaces;
using MathRecognizer.Models;

namespace MathRecognizer.EquationBuilding
{
    public class CharacterFixer : ICharacterFixer
    {
        private static string CheckDotRole(NamedSegment lastSegment, NamedSegment currentSegment, NamedSegment nextSegment)
        {
            if (lastSegment == null || nextSegment == null)
            {
                return ".";
            }

            var height = lastSegment.MaxY - lastSegment.MinY;
            if (currentSegment.MinY > lastSegment.MinY + height / 5 &&
                currentSegment.MaxY < lastSegment.MaxY - height / 5 &&
                currentSegment.MinY > nextSegment.MinY + height / 5 &&
                currentSegment.MaxY < nextSegment.MaxX - height / 5)
            {
                return "*";
            }
            return ".";
        }


        public string AddAdjusted(string equation, NamedSegment lastSegment, NamedSegment currentSegment, NamedSegment nextSegment)
        {
            if (currentSegment.SegmentName == "1" && equation.EndsWith("s"))
            {
                return equation.Substring(0, equation.Length - 1) + "si";
            }

            if (currentSegment.SegmentName == "0" && equation.EndsWith("c"))
            {
                return equation.Substring(0, equation.Length - 1) + "co";
            }

            if (currentSegment.SegmentName == "g" && equation.EndsWith("10"))
            {
                return equation.Substring(0, equation.Length - 2) + "log";
            }

            if (currentSegment.SegmentName == "g" && equation.EndsWith("1"))
            {
                return equation.Substring(0, equation.Length - 1) + "lg";
            }

            if (currentSegment.SegmentName == ".")
            {
                return equation + CheckDotRole(lastSegment, currentSegment, nextSegment);
            }

            return equation + currentSegment.SegmentName;
        }
    }
}