using System;
using System.Collections.Generic;

using Blockgame.Events;

namespace Blockgame.Layers
{
    public class LayerStack
    {
        List<Layer> _layers;

        public LayerStack()
        {
            _layers = new List<Layer>();
        }

        public void AddLayer(Layer layer)
        {
            layer.Load();
            _layers.Add(layer);
        }

        public void RemoveLayer(Layer layer)
        {
            layer.Unload();
            _layers.Remove(layer);
        }

        public void RemoveAll()
        {
            for (var i = 0; i < _layers.Count; ++i)
            {
                _layers[i].Unload();
            }
            _layers.Clear();
        }

        public void Update(float deltaTime)
        {
            foreach (var layer in _layers)
            {
                layer.Update(deltaTime);
            }
        }
        public void Render()
        {
            foreach (var layer in _layers)
            {
                layer.Render();
            }
        }
        public void OnEvent(Event @event)
        {
           
            // Iterate in reverse order, top-most layer (likely UI)
            // should receive the event first.
            for (var i = _layers.Count-1; i > -1; --i)
            {
                _layers[i].OnEvent(@event);
            }
        }
    }
}
