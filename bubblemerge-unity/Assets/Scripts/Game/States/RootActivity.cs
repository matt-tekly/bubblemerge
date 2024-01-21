using Tekly.DataModels.Models;
using Tekly.Injectors;
using Tekly.TreeState.StandardActivities;

namespace Game.States
{
	public class RootActivity : InjectableActivity, IInjectionProvider
	{
		public void Provide(InjectorContainer container)
		{
			container.Register(RootModel.Instance);
		}
	}
}