namespace BitwiseAI.ServiceLocator
{
	public readonly struct TimeData
	{
		public float DeltaTime { get; }
		public float Time { get; }
		public int Frame { get; }

		// ----------------------------------------------------------------------------

		public TimeData(float deltaTime, float time, int frame)
		{
			DeltaTime = deltaTime;
			Time = time;
			Frame = frame;
		}
	}
}