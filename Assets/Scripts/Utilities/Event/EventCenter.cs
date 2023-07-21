#region File Info

// Provided by Fendou at 2023-07-14-22:43

#endregion

using System;
using System.Collections.Generic;
using System.Linq;

namespace Plutono.Util
{
    public static class EventCenter
    {
        private static readonly Dictionary<Type, LinkedList<Delegate>> Handlers = new();

        public static void AddListener<T>(in Action<T> action) where T : struct, IEvent
        {
            var type = typeof(T);
            if (!Handlers.ContainsKey(type))
            {
                Handlers[type] = new LinkedList<Delegate>();
            }
            Handlers[type].AddLast(action);
        }

        public static void RemoveListener<T>(in Action<T> action) where T : struct, IEvent
        {
            var type = typeof(T);
            if (Handlers.TryGetValue(type, out var handler))
            {
                handler.Remove(action);
            }
        }

        public static void RemoveAllListener<T>() where T : struct, IEvent
        {
            var type = typeof(T);
            if (!Handlers.ContainsKey(type)) return;
            Handlers[type].Clear();
            Handlers.Remove(type);
        }

        public static void Broadcast<T>(T @event) where T : struct, IEvent
        {
            var type = typeof(T);
            if (!Handlers.ContainsKey(type)) return;
            foreach (var item in Handlers[type].ToList())
            {
                (item as Action<T>)?.Invoke(@event);
            }
        }
    }
}