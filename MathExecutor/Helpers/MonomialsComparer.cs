using System;
using System.Collections.Generic;
using System.Linq;
using MathExecutor.Models;

namespace MathExecutor.Helpers
{
    public class MonomialsComparer : IComparer<Monomial>
    {
        public int Compare(Monomial x, Monomial y)
        {
            var xCount = x.Variables?.Count() ?? 0;
            var yCount = y.Variables?.Count() ?? 0;

            if (xCount == 0 && yCount == 0)
            {
                return 0;
            }

            if (xCount != yCount)
            {
                return xCount > yCount ? 1 : -1;
            }

            var xOrdered = x.Variables.OrderBy(v => v.Name).ToList();
            var yOrdered = y.Variables.OrderBy(v => v.Name).ToList();

            for (var i = 0; i < xOrdered.Count; i++)
            {
                if (xOrdered[i].Name == yOrdered[i].Name &&
                    Math.Abs(xOrdered[i].Exponent - yOrdered[i].Exponent) < 0.001)
                {
                    continue;
                }
                if (xOrdered[i].Name != yOrdered[i].Name)
                {
                    return -string.Compare(xOrdered[i].Name, yOrdered[i].Name, StringComparison.InvariantCulture); 
                }
                return xOrdered[i].Exponent > yOrdered[i].Exponent ? 1 : -1;                
            }
            return 0;
        }
    }
}