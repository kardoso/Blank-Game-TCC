using UnityEngine;
using System.Collections.Generic;

public class SimplePathDefinition : PathDefinition
{
	public PathNode mPathNode1;
	public PathNode mPathNode2;

	// -------------------------------------------------------------------------
	public override IEnumerator<PathNode> GetPathEnumerator()
	{
		// check if both path nodes entered
		if (mPathNode1 == null || mPathNode2 == null)
			yield break;


		// else set pointer to current node
		PathNode currentNode = mPathNode1;

		// return points based on movement type
		// in case of simple path loop and ping pong are the same
		while (true)
		{
			yield return currentNode;

			// swap to next node
			if (currentNode == mPathNode1)
				currentNode = mPathNode2;
			else
				currentNode = mPathNode1;

			// if move only once mode and returned to node1 then exit
			if (mMovementType == ePathRepeatType.Once && currentNode == mPathNode1)
				yield break;
		}
	}
}