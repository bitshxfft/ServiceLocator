using UnityEngine;

namespace BitwiseAI.ServiceLocator.Sample
{
	// Exists simply as an example of registering non-MonoBehaviour services
	public class ServiceRegister : MonoBehaviour
	{
		private void Start()
		{
			ServiceLocator.Instance.RegisterService(new SampleService());
			ServiceLocator.Instance.RegisterUpdatedService(new SampleUpdatedService());
		}

		private void Update()
		{
			Debug.Assert(ServiceLocator.Instance.GetService<SampleService>() != null);
			Debug.Assert(ServiceLocator.Instance.GetService<SampleUpdatedService>() != null);
			Debug.Assert(ServiceLocator.Instance.GetService<SampleMonoBehaviourService>() != null);
			Debug.Assert(ServiceLocator.Instance.GetService<SampleMonoBehaviourUpdatedService>() != null);
		}
	}
}