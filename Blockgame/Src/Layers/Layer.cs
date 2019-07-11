using System;
using Blockgame.Events;

namespace Blockgame.Layers
{
    public abstract class Layer
    {
        public bool Disabled { get; set; } = false;

        public abstract void Load();

        public abstract void Unload();

        public abstract void Update(float deltaTime);

        public abstract void Render();

        public abstract void OnEvent(Event @event);


    }
}
