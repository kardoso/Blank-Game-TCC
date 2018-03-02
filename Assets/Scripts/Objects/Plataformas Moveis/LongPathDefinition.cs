using UnityEngine;
using System.Collections.Generic;

public class LongPathDefinition : PathDefinition
{
    public bool mReverseDirection;
    public PathNode[] mPathNodes;
    
    // -------------------------------------------------------------------------
	public override IEnumerator<PathNode> GetPathEnumerator()
	{
		// check if nodes array is valid entered and longer than 2 nodes (1 node = no path)
		if (mPathNodes == null || mPathNodes.Length < 2)
			yield break;
		
		// set index to start
		int currentNodeIndex = 0;
		int directionAdjustment = mReverseDirection ? -1 : 1;
		// if move type is only once and direction is reversed, start from last node
		if (mReverseDirection && mMovementType == ePathRepeatType.Once)
			currentNodeIndex = mPathNodes.Length - 1;
		
		// return points based on movement type
		// in case of simple path loop and ping pong are the same
		while (true)
		{
			yield return mPathNodes[currentNodeIndex];

			// adjust in move direction
			currentNodeIndex += directionAdjustment;

			if (currentNodeIndex < 0 || currentNodeIndex >= mPathNodes.Length)
			{
				if (mMovementType == ePathRepeatType.Once)
				{
					yield break;
				}
				else if (mMovementType == ePathRepeatType.PingPong)
				{
					directionAdjustment *= -1;
					// adjust twice - first return to current node, second move in new direction
					currentNodeIndex += directionAdjustment * 2;
				}
				else if (mMovementType == ePathRepeatType.Loop)
				{
					if (currentNodeIndex < 0)
						currentNodeIndex = mPathNodes.Length - 1;
					else
						currentNodeIndex = 0;
				}
			}
		}
	}
}