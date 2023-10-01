using CodeBase.Infrastructure.Helpers;
using CodeBase.Infrastructure.Services.Binding;
using CodeBase.Infrastructure.Services.Ui;
using CodeBase.Infrastructure.States;
using CodeBase.UI;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        public LoadingCurtain Curtain;
    
        private Game _game;
    
        private LoadLevelState.Factory _loadLevelFactory;
        private IInitializable _initializable;
        private BinderService _binderService;
        private WindowService _windowService;

        [Inject]
        void Construct(LoadLevelState.Factory loadLevelFactory, IInitializable initializable, BinderService binderService,
            WindowService windowService)
        {
            _windowService = windowService;
            _binderService = binderService;
            _initializable = initializable;
            _loadLevelFactory = loadLevelFactory;
        }

        private void Awake()
        {
            _game = new Game(this, Curtain, _loadLevelFactory, _initializable, _binderService, _windowService);
            _game.StateMachine.Enter<BootstrapState>();
        
            DontDestroyOnLoad(this);
        }
    }
}