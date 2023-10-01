using CodeBase.Infrastructure.Helpers;
using CodeBase.Infrastructure.Services.Binding;
using CodeBase.Infrastructure.Services.Ui;
using CodeBase.Infrastructure.States;
using CodeBase.UI;
using Zenject;

namespace CodeBase.Infrastructure
{
    public class Game
    {
        private GameStateMachine _stateMachine;

        public GameStateMachine StateMachine => _stateMachine;

        public Game(ICoroutineRunner coroutineRunner, LoadingCurtain loadingCurtain, LoadLevelState.Factory loadLevelStateFactor,
            IInitializable initializable, BinderService binderService, WindowService windowService)
        {
            var sceneLoader = new SceneLoader(coroutineRunner);
            var gameStateMachine = new GameStateMachine(sceneLoader, loadingCurtain, loadLevelStateFactor, initializable, 
                binderService, windowService);
            _stateMachine = gameStateMachine;
        }
    }
}