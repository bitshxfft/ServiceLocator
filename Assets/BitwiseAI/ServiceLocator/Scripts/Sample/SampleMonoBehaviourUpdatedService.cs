using UnityEngine;

namespace BitwiseAI.ServiceLocator.Sample
{
	public class SampleMonoBehaviourUpdatedService : MonoBehaviour, IUpdatedService
	{
		public void OnUpdate(in TimeData timeData)
		{
			//Debug.Log($"[BitwiseAI.ServiceLocator.SampleSampleUpdatedMonoBehaviourService::OnUpdate] Time: {timeData.Time}");
		}
	}
}