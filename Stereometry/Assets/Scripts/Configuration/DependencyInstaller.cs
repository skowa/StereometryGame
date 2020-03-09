using Assets.Scripts.Authorization;
using Zenject;

namespace Assets.Scripts.Configuration
{
	public class DependencyInstaller : MonoInstaller<DependencyInstaller>
	{
		public override void InstallBindings()
		{
			//Container.Bind<IAuthorization>().To<GoogleAuthorization>().AsSingle();
		}
	}
}