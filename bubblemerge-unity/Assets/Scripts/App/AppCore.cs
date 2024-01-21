using Tekly.DataModels.Models;
using Tekly.Logging;
using UnityEngine;

namespace App
{
	public class AppCore : MonoBehaviour
	{
		private void Update()
		{
			ModelManager.Instance.Tick();
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void Initialize()
		{
			TkLogger.Initialize();
		}
	}
}