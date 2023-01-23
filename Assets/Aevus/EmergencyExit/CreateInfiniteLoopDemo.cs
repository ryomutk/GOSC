using UnityEngine;

namespace Aevus
{
	public class CreateInfiniteLoopDemo : MonoBehaviour
	{
		//public bool singleRunInfiniteLoop = false;
		public bool infiniteLoopInUpdate = false;
		public bool infiniteLoopInOnDrawGizmos = false;

		void Update()
		{
			while (infiniteLoopInUpdate) ;
		}

		private void FixedUpdate() 
		{
			transform.Rotate(0.77f, 0.66f, 0.88f);
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawRay(transform.position, Vector3.right);
			while (infiniteLoopInOnDrawGizmos) ;
		}
	}
}