using UnityEngine;

namespace BitwiseAI.ServiceLocator.Sample
{
	public class SampleStandaloneService : MonoBehaviour, IUpdatedService
	{
		private void Start()
		{
			// Register in Start, not Awake, to ensure ServiceLocator singleton is created
			ServiceLocator.Instance.Register(this);

			DontDestroyOnLoad(this);
		}

		void IUpdatedService.OnUpdate(in TimeData timeData)
		{
			;
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