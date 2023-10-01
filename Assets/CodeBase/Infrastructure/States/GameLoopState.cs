using CodeBase.Infrastructure.Services.Binding;
using CodeBase.Infrastructure.Services.Ui;

namespace CodeBase.Infrastructure.States
{
    public class GameLoopState : IState
    {
        private BinderService _binderService;
        private WindowService _windowService;

        public GameLoopState(BinderService binderService, WindowService windowService)
        {
            _windowService = windowService;
            _binderService = binderService;
        }

        public void Enter()
        {
        
        }

        public void Exit()
        {
            _binderService.Dispose();
            _windowService.CloseMainMenu();
        }
    }
}