using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CodeBase.Infrastructure.Services.Ui
{
    public class WindowService
    {
        private DiContainer _container;
        private MainMenuWindow _mainMenuWindow;

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

        public MainMenuWindow CreateLevelMenu()
        {
            CloseMainMenu();
            _mainMenuWindow = _container.InstantiatePrefabResourceForComponent<MainMenuWindow>("UI/MainMenu");
            return _mainMenuWindow;
        }

        public void CloseMainMenu()
        {
            if(_mainMenuWindow != null)
                Object.Destroy(_mainMenuWindow.gameObject.transform.parent.gameObject);
        }
    }
}