using System;
using MathRecognizer.Interfaces;

namespace MathRecognizer.SegmentsRecognition
{
    public class IndexMapper : IIndexMapper
    {
        public string GetIndexName(int index)
        {
            switch (index)
            {
                case 0: return "?";
                case 1: return "1";
                case 2: return "2";
                case 3: return "3";
                case 4: return "4";
                case 5: return "5";
                case 6: return "6";
                case 7: return "7";
                case 8: return "8";
                case 9: return "9";

                case 10: return "0";
                case 11: return "+";
                case 12: return "-";
                case 13: return "(";
                case 14: return ")";
                case 15: return "q";
                case 16: return "x";
                case 17: return "y";
                case 18: return "a";
                case 19: return "b";
                case 20: return ".";
                case 21: return ",";
                case 22: return "s";
                case 23: return "pi";
                case 24: return "n";
                case 25: return "c";
                case 26: return "t";
                case 27: return "l";
                case 28: return "g";
                case 29: return "f";
                case 30: return ">";
                case 31: return "<";
                default: return "?";
            }
        }
    }
}