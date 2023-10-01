using UnityEngine;
using Zenject;
using Observable = CodeBase.Infrastructure.Helpers.Observable;

namespace CodeBase.Infrastructure.Services.Input
{
    public class StandaloneInputService : IInputService, ITickable
    {
        private const int DefaultSpeedMultiplier = 3;
        private const float DefaultJumpSpeedMultiplier = 10f/3f;

        
        public bool BombPlacing { get; private set; }
        public bool RoomDestroyerPlacing { get; private set; }
        public bool LampPlacing { get; private set; }
        public bool Picking { get; private set; }


        public Vector2 Moving()
        {
            var horizontal = UnityEngine.Input.GetAxis("Horizontal");
            var vertical = UnityEngine.Input.GetAxis("Vertical")*DefaultJumpSpeedMultiplier;

            var direction = new Vector2(horizontal, vertical);
            return direction*Time.deltaTime*DefaultSpeedMultiplier;
        }

        public void Tick()
        {
            Picking = true;
            BombPlacing = false;
            RoomDestroyerPlacing = false;
            LampPlacing = false;
            
            if (UnityEngine.Input.GetAxis("Fire1") == 1)
                RoomDestroyerPlacing = true;
            if (UnityEngine.Input.GetAxis("Fire2") == 1)
                BombPlacing = true;
            if (UnityEngine.Input.GetAxis("Jump") == 1)
                LampPlacing = true;
        }
    }
}