using Infrastructure;

namespace Barebone.ViewModels.Shared.Script
{
    public class ScriptViewModelFactory
    {
        public ScriptViewModel Create(Infrastructure.Script script_)
        {
            return new ScriptViewModel()
            {
                Url = script_.Url
            };
        }
    }
}