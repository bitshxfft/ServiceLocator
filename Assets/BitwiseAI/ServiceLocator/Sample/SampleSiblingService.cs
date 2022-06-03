using System;
using UnityEngine;

namespace BitwiseAI.ServiceLocator.Sample
{
	public class SampleSiblingService : MonoBehaviour, IService
	{
		private void Start()
		{
			// no need to register with ServiceLocator, just drop on the Services GameObject and it will be automatically registered

			DontDestroyOnLoad(this);
		}

		private void OnDestroy()
		{
			if (null != ServiceLocator.Instance)
			{
				ServiceLocator.Instance.Unregister(this);
			}
		}
	}
}