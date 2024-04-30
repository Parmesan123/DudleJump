using UnityEngine;

namespace GamePlay
{
	public abstract class BonusItem : MonoBehaviour
	{
		public abstract void Interact();
		
		public void SetPlatform(Platform platform)
		{
			platform.DisablePlatformEvent += DestroyItem;
		}
		
		private void DestroyItem(Platform platform)
		{
			platform.DisablePlatformEvent -= DestroyItem;
			
			Destroy(gameObject);
		}
	}
}