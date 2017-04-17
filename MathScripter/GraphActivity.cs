using Android.App;
using Android.OS;

namespace MathScripter
{
    [Activity(Label = "Function Graph")]
    public class GraphActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Graph);
        }
    }
}