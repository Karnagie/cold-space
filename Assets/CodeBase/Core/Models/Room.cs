using System;
using System.Collections;
using AStar;
using CodeBase.Core.Models.Components;
using CodeBase.Core.Models.Systems;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Helpers;
using UniRx;
using UnityEngine;

namespace CodeBase.Core.Models
{
    public class Room : ISingleSystem
    {
        private ICoroutineRunner _coroutineRunner;
        public RoomComponents Components { get; }
        public Position Position { get; }
        public RoomType Type { get; }
        public bool ReadyToExplodeWindow { get; private set; } = true;
        public bool ReadyToDestroyRoom { get; private set; } = true;

        public BooleanNotifier WindowDestroyed = new ();
        public BooleanNotifier RoomDestroyed = new ();
        public BooleanNotifier IsLampOn = new ();

        public Room(RoomComponents components, Position position, ICoroutineRunner coroutineRunner, RoomType type)
        {
            Type = type;
            _coroutineRunner = coroutineRunner;
            Components = components;
            Position = position;
        }

        public void ExplodeWindow()
        {
            _coroutineRunner.StartCoroutine(WindowExploding());
            ReadyToExplodeWindow = false;
        }
        
        public void RoomDestroy()
        {
            _coroutineRunner.StartCoroutine(RoomDestroying());
            ReadyToExplodeWindow = false;
            ReadyToDestroyRoom = false;
        }
        
        private IEnumerator RoomDestroying()
        {
            yield return new WaitForSeconds(5);
            RoomDestroyed.TurnOn();
        }

        private IEnumerator WindowExploding()
        {
            yield return new WaitForSeconds(5);
            WindowDestroyed.TurnOn();
        }

        public void PlaceLamp()
        {
            Components.Lamp.gameObject.SetActive(true);
            IsLampOn.TurnOff();
        }
    }
}