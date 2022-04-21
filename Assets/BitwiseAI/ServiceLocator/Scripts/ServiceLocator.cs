using System;
using System.Collections.Generic;
using UnityEngine;

namespace BitwiseAI.ServiceLocator
{
	public class ServiceLocator : MonoBehaviour
	{
		public static ServiceLocator Instance { get; private set; } = null;

		// ----------------------------------------------------------------------------

		private Dictionary<Type, IService> m_Services = new Dictionary<Type, IService>();
		private List<IUpdatedService> m_UpdatedServices = new List<IUpdatedService>();

		// ----------------------------------------------------------------------------

		private void Awake()
		{
			if (null != Instance)
			{
				Debug.LogError("[BitwiseAI.ServiceLocator.ServiceLocator::Awake] Multiple ServiceLocator instances");
				DestroyImmediate(gameObject);
			}
			else
			{
				Instance = this;
				DontDestroyOnLoad(gameObject);
			}
		}

		private void Start()
		{
			var services = GetComponents<IService>();
			for (int i = 0; i < services.Length; ++i)
			{
				AddService(services[i]);
			}

			var updatedServices = GetComponents<IUpdatedService>();
			for (int i = 0; i < updatedServices.Length; ++i)
			{
				AddUpdatedService(updatedServices[i]);
			}
		}

		private void Update()
		{
			var timeData = new TimeData(Time.deltaTime, Time.time, Time.frameCount);
			for (int i = 0; i < m_UpdatedServices.Count; ++i)
			{
				m_UpdatedServices[i].OnUpdate(in timeData);
			}
		}

		// ----------------------------------------------------------------------------

		private void AddService(IService service)
		{
			if (null == service)
			{
				Debug.LogError("[BitwiseAI.ServiceLocator.ServiceLocator::AddService] received null IService");
				return;
			}

			var serviceType = service.GetType();
			if (false == m_Services.ContainsKey(serviceType))
			{
				m_Services.Add(serviceType, service);
				Debug.Log($"[BitwiseAI.ServiceLocator.ServiceLocator::AddService] Added Service: {serviceType}");
			}
			else
			{
				Debug.LogError($"[BitwiseAI.ServiceLocator.ServiceLocator::AddService] received duplicate Service: {serviceType}");
			}
		}

		private void AddUpdatedService(IUpdatedService updatedService)
		{
			if (null == updatedService)
			{
				Debug.LogError("[BitwiseAI.ServiceLocator.ServiceLocator::AddUpdatedService] received null IUpdatedService");
				return;
			}

			m_UpdatedServices.Add(updatedService);
			Debug.Log($"[BitwiseAI.ServiceLocator.ServiceLocator::AddUpdatedService] Added Updated Service: {updatedService.GetType()}");
		}

		// ----------------------------------------------------------------------------

		public void RegisterService(IService service)
		{
			AddService(service);
		}

		public void RegisterUpdatedService(IUpdatedService updatedService)
		{
			AddService(updatedService);
			AddUpdatedService(updatedService);
		}

		public TService GetService<TService>()
		{
			if (m_Services.TryGetValue(typeof(TService), out var service))
			{
				return (TService) service;
			}
			else
			{
				Debug.LogError($"[BitwiseAI.ServiceLocator.ServiceLocator::GetService] Failed to find Service: {typeof(TService)}");
				return default;
			}
		}
	}
}