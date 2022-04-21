namespace BitwiseAI.ServiceLocator
{
	public interface IService
	{
	}

	public interface IUpdatedService : IService
	{
		void OnUpdate(in TimeData timeData);
	}
}