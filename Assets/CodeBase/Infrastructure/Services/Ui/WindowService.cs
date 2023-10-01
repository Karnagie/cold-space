using CodeBase.Infrastructure.States;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using NotImplementedException = System.NotImplementedException;

namespace CodeBase.Infrastructure.Services.Ui
{
    public class WindowService
    {
        private DiContainer _container;
        private MainMenuWindow _mainMenuWindow;
        public GameStateMachine GameStateMachine;

        public WindowService(DiContainer container)
        {
            _container = container;
        }
        
        public MainMenuWindow CreateMainMenu()
        {
            CloseMainMenu();
            _mainMenuWindow = _container.InstantiatePrefabResourceForComponent<MainMenuWindow>("UI/MainMenu");
            return _mainMenuWindow;
        }

        public MainMenuWindow CreateLevelMenu(string buttonText)
        {
            CloseMainMenu();
            _mainMenuWindow = _container.InstantiatePrefabResourceForComponent<MainMenuWindow>("UI/MainMenu");
            _mainMenuWindow.StartText.text = buttonText;
            return _mainMenuWindow;
        }

        public void CloseMainMenu()
        {
            if(_mainMenuWindow != null)
                Object.Destroy(_mainMenuWindow.gameObject.transform.parent.gameObject);
        }

        public void CreateWinMenu()
        {
            var window = _container.InstantiatePrefabResourceForComponent<WinWindow>("UI/Win");
            window.Menu.onClick.AddListener((() => GameStateMachine.Enter<MenuState, string>("Menu")));
        }
    }
}