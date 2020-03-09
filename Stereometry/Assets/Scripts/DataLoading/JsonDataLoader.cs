using System;

namespace Assets.Scripts.DataLoading
{
	public class JsonDataLoader : IDataLoader
	{
		private readonly string _filePath;

		public JsonDataLoader(string filePath)
		{
			_filePath = !string.IsNullOrWhiteSpace(filePath) ? filePath : throw new ArgumentNullException(nameof(filePath));
		}

		public Level LoadLevel(int number)
		{
			throw new System.NotImplementedException();
		}
	}
}