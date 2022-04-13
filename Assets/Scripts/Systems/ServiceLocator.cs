using System.Collections.Generic;

public static class ServiceLocator
{
    private static readonly Dictionary<System.Type, object> _systemRegistry = new Dictionary<System.Type, object>();

    public static void Register<T>(object system)
    {
        if (!_systemRegistry.ContainsKey(typeof(T)))
        {
            _systemRegistry.Add(typeof(T), system);
            return;
        }
    }

    public static void Register(System.Type systemType, object system)
    {
        if (!_systemRegistry.ContainsKey(systemType))
        {
            _systemRegistry.Add(systemType, system);
            return;
        }
    }

    public static T Get<T>()
    {
        T result = default(T);

        return result;
    }

    public static bool Contains<T>()
    {
        return _systemRegistry.ContainsKey(typeof(T));
    }
}