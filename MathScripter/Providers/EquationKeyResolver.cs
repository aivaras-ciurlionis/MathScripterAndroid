using MathScripter.Interfaces;
using Org.Json;

namespace MathScripter.Providers
{
    public class EquationKeyResolver : IEquationKeyResolver
    {
        public string ResolveKey(int keyCode, string keyFile)
        {
            var resolver = new JSONObject(keyFile);
            return resolver.GetString(keyCode.ToString());
        }
    }
}