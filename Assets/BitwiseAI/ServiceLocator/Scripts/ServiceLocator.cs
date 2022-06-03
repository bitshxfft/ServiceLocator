using System;
using System.Collections.Generic;
using UnityEngine;

namespace BitwiseAI.ServiceLocator
{
	public class ServiceLocator : MonoBehaviour
	{
		private enum InitialisationState
		{
			Uninitialised,
			Initialised,
		}

		// ----------------------------------------------------------------------------

		public static ServiceLocator Instance { get; private set; }

		// ----------------------------------------------------------------------------

		private Dictionary<Type, IService> m_Services = new Dictionary<Type, IService>();
		private List<IUpdatedService> m_UpdatedServices = new List<IUpdatedService>();
		private InitialisationState m_InitialisationState = InitialisationState.Uninitialised;

		// ----------------------------------------------------------------------------

		private void Awake()
		{
			if (null != Instance)
			{
				Debug.LogError("[ServiceLocator::Awake] Multiple ServiceLocator instances");
				DestroyImmediate(gameObject);
			}
			else
			{
				Instance = this;
				DontDestroyOnLoad(gameObject);
				m_InitialisationState = InitialisationState.Initialised;
			}
		}

		private void Start()
		{
			var services = GetComponents<IService>();
			foreach (IService service in services)
			{
				Register(service);
			}
		}

		private void Update()
		{
			var timeData = new TimeData(Time.deltaTime, Time.time, Time.frameCount);
			foreach (IUpdatedService service in m_UpdatedServices)
			{
				service.OnUpdate(in timeData);
			}
		}

		// ----------------------------------------------------------------------------

		public void Register(IService service)
		{
			if (null == service)
			{
				Debug.LogError("[ServiceLocator::Register] received null Service");
				return;
			}
			
			var serviceType = service.GetType();

			if (m_InitialisationState != InitialisationState.Initialised)
			{
				Debug.LogError($"[ServiceLocator::Register] ServiceLocator is not Initialised, did you try to register {serviceType} from Awake?");
				return;
			}

			// add service
			if (false == m_Services.ContainsKey(serviceType))
			{
				m_Services.Add(serviceType, service);
				Debug.Log($"[ServiceLocator::Register] Added Service: {serviceType}");
			}
			else
			{
				Debug.LogError($"[ServiceLocator::Register] Service already registered: {serviceType}");
			}

			// add updated service reference
			if (service is IUpdatedService updatedService)
			{
				m_UpdatedServices.Add(updatedService);
				Debug.Log($"[ServiceLocator::Register] Added Updated Service: {updatedService.GetType()}");
			}
		}

		public void Unregister(IService service)
		{
			if (null == service)
			{
				Debug.LogError("[ServiceLocator::Unregister] received null Service");
				return;
			}

			Type serviceType = service.GetType();
			
			if (m_InitialisationState != InitialisationState.Initialised)
			{
				Debug.LogError($"[ServiceLocator::Unregister] ServiceLocator is not Initialised, did you try to unregister {serviceType} from Awake?");
				return;
			}

			// remove service
			if (m_Services.Remove(serviceType))
			{
				Debug.Log($"[ServiceLocator::Unregister] Removed Service: {serviceType}");
			}
			else
			{
				Debug.LogError($"[ServiceLocator::Unregister] Service not registered: {serviceType}");
			}

			// remove updated service reference
			if (service is IUpdatedService updatedService)
			{
				if (m_UpdatedServices.Remove(updatedService))
				{
					Debug.Log($"[ServiceLocator::Unregister] Removed Updated Service: {serviceType}");
				}
				else
				{
					Debug.LogError($"[ServiceLocator::Unregister] Failed to remove Updated Service: {serviceType}");
				}
			}
		}

		public TService Get<TService>() where TService : IService
		{
			var serviceType = typeof(TService);
			if (m_Services.TryGetValue(serviceType, out IService service))
			{
				return (TService)service;
			}
			else
			{
				Debug.LogError($"[ServiceLocator::Get] Failed to find Service: {serviceType}");
				return default;
			}
		}
	}
}