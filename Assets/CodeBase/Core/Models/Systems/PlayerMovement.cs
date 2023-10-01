using CodeBase.Infrastructure.Services.Input;
using Zenject;

namespace CodeBase.Core.Models.Systems
{
    public class PlayerMovement : IFixedTickable, ISystem
    {
        private IInputService _inputService;
        private IUnit _model;

        public PlayerMovement(IInputService inputService, IUnit model)
        {   
            _model = model;
            _inputService = inputService;
        }

        public void FixedTick()
        {
            var translation = _inputService.Moving();
            _model.Components.Transform.Translate(translation);
            
            _model.Picking = _inputService.Picking;
            
            _model.BombPlacing = _inputService.BombPlacing;
            _model.RoomDestroyerPlacing = _inputService.RoomDestroyerPlacing;
            _model.LampPlacing = _inputService.LampPlacing;
        }
    }
}