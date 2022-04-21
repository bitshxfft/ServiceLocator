using UnityEngine;

namespace BitwiseAI.ServiceLocator.Sample
{
	public class SampleUpdatedService : IUpdatedService
	{
		public void OnUpdate(in TimeData timeData)
		{
			//Debug.Log($"[BitwiseAI.ServiceLocator.SampleUpdatedService::OnUpdate] Time: {timeData.Time}");
		}
	}
}