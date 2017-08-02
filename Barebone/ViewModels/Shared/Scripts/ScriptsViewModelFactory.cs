using System.Collections.Generic;
using System.Linq;
using Barebone.ViewModels.Shared.Script;
using ExtCore.Infrastructure;
using Infrastructure;
using Barebone.ViewModels.Shared.Scripts;

namespace Barebone.ViewModels.Shared.Scripts
{
    public class ScriptsViewModelFactory
    {
        public ScriptsViewModel Create()
        {
            List<Infrastructure.Script> scripts = new List<Infrastructure.Script>();

            foreach (IExtensionMetadata extensionMetadata in ExtensionManager.GetInstances<IExtensionMetadata>())
                scripts.AddRange(extensionMetadata.Scripts);

            return new ScriptsViewModel()
            {
                Scripts = scripts.OrderBy(s => s.Position).Select(s => new ScriptViewModelFactory().Create(s))
            };
        }
    }
}