using System;
using System.Collections.Generic;
using XCore.Generator;

namespace Confuser.Core;

public class ServiceRegistry : IServiceProvider
{
	private readonly HashSet<string> serviceIds = new HashSet<string>();

	private readonly Dictionary<Type, object> services = new Dictionary<Type, object>();

	object IServiceProvider.GetService(Type serviceType)
	{
		return services.GetValueOrDefault(serviceType);
	}

	public T GetService<T>()
	{
		return (T)services.GetValueOrDefault(typeof(T));
	}

	public void RegisterService(string serviceId, Type serviceType, object service)
	{
		if (!serviceIds.Add(serviceId))
		{
			throw new ArgumentException("Service with ID '" + serviceIds?.ToString() + "' has already registered.", "serviceId");
		}
		if (services.ContainsKey(serviceType))
		{
			throw new ArgumentException("Service with type '" + service.GetType().Name + "' has already registered.", "service");
		}
		services.Add(serviceType, service);
	}

	public bool Contains(string serviceId)
	{
		return serviceIds.Contains(serviceId);
	}
}
