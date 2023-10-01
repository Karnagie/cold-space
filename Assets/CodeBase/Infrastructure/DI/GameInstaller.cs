using CodeBase.Core.Models.Services;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Helpers;
using CodeBase.Infrastructure.Services.Binding;
using CodeBase.Infrastructure.Services.Input;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.Physics;
using CodeBase.Infrastructure.Services.System;
using CodeBase.Infrastructure.Services.Ticking;
using CodeBase.Infrastructure.Services.Ui;
using CodeBase.Infrastructure.States;
using CodeBase.UI;
using Zenject;

namespace CodeBase.Infrastructure.DI
{
    public class GameInstaller : MonoInstaller, ICoroutineRunner
    {
        public override void InstallBindings()
        {
            //Infrastructure
            RegisterAssetProvider();
            RegisterSceneLoader();
            RegisterLoadingCurtain();

            //Services
            RegisterTickService();
            RegisterInputService();
            RegisterPersistentProgressService();
            Container.Bind<IPhysicsService>().To<PhysicsService>().AsSingle();
            Container.Bind<SystemService>().AsSingle();
            Container.Bind<BinderService>().AsSingle();
            Container.Bind<WindowService>().AsSingle();
            Container.Bind<UnitService>().AsSingle();
            Container.Bind<DeathService>().AsSingle();
            Container.Bind<UnitFactory>().AsSingle();
            Container.Bind<AstarService>().AsSingle();
            Container.Bind<RoomService>().AsSingle();
            Container.Bind<ICoroutineRunner>().FromInstance(this).AsSingle();
            
            Container.BindInterfacesAndSelfTo<EnemySpawner>().AsSingle();
            
            //Factories
            Container.Bind<GameFactory>().To<GameFactory>().AsSingle();
            RegisterGameStateFactories();
            Container.Bind<ViewFactory>().To<ViewFactory>().AsSingle();
            Container.Bind<ServiceSystemFactory>().To<ServiceSystemFactory>().AsSingle();
            Container.Bind<BinderFactory>().To<BinderFactory>().AsSingle();
            Container.Bind<ItemFactory>().AsSingle();
        }

        private void RegisterInputService() => Container.BindInterfacesTo<StandaloneInputService>().AsSingle();

        private void RegisterTickService() => Container.BindInterfacesAndSelfTo<TickService>().AsSingle();

        private void RegisterPersistentProgressService() => 
            Container.Bind<IPersistentProgressService>().To<PersistentProgressService>().AsSingle();

        private void RegisterGameStateFactories() => 
            Container.BindFactory<GameStateMachine, SceneLoader, LoadingCurtain, LoadLevelState, LoadLevelState.Factory>();

        private void RegisterLoadingCurtain() => Container.Bind<LoadingCurtain>().To<LoadingCurtain>().AsSingle();

        private void RegisterSceneLoader() => Container.Bind<SceneLoader>().To<SceneLoader>().AsSingle();

        private void RegisterAssetProvider() => Container.Bind<IAssetProvider>().To<AssetProvider>().AsSingle();
    }
}