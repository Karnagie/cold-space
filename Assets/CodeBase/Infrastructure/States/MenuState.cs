using CodeBase.Infrastructure.Helpers;
using CodeBase.Infrastructure.Services.Ui;
using CodeBase.UI;

namespace CodeBase.Infrastructure.States
{
    public class MenuState : IPayLoadState<string>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly WindowService _windowService;

        public MenuState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain,
            WindowService windowService)
        {
            _windowService = windowService;
            _loadingCurtain = loadingCurtain;
            _sceneLoader = sceneLoader;
            _gameStateMachine = gameStateMachine;
        }

        public void Enter(string sceneName)
        {
            _loadingCurtain.Show();
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit()
        {
            _windowService.CloseMainMenu();
        }

        private void OnLoaded()
        {
            var window = _windowService.CreateMainMenu();
            window.Start.onClick.AddListener((() => _gameStateMachine.Enter<LoadLevelState, string>("Level")));
            
            _loadingCurtain.Hide();
        }
    }
}