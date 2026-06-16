using System;
using System.Collections.Generic;
using UnityEngine;

namespace SledSurfers.Scripts.Core
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _services = new();

        public static void Register<T>(T service) where T : class
        {
            var type = typeof(T);

            if (_services.ContainsKey(type))
            {
                Debug.LogWarning($"[ServiceLocator] Overwriting existing registration for {type.Name}");
            }

            _services[type] = service;
        }

        public static T Get<T>() where T : class
        {
            var type = typeof(T);

            if (_services.TryGetValue(type, out var service))
                return service as T;

            throw new InvalidOperationException($"[ServiceLocator] Service not registered: {type.Name}");
        }

        public static bool TryGet<T>(out T service) where T : class
        {
            if (_services.TryGetValue(typeof(T), out var raw))
            {
                service = raw as T;
                return service != null;
            }

            service = null;
            return false;
        }

        public static void Unregister<T>() where T : class
        {
            _services.Remove(typeof(T));
        }

        public static void Clear()
        {
            _services.Clear();
        }
    }
}