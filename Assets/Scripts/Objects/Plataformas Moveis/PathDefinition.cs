using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class PathDefinition : MonoBehaviour
{
	public enum ePathRepeatType
	{
		Once,
		PingPong,
		Loop
	}

	public ePathRepeatType mMovementType;

	// -------------------------------------------------------------------------
	public abstract IEnumerator<PathNode> GetPathEnumerator();
	
	// -------------------------------------------------------------------------
    public void OnDrawGizmos()
    {
	IEnumerator<PathNode> nodes = GetPathEnumerator();

	if (!nodes.MoveNext())
	    return;

	Gizmos.color = Color.grey;
	PathNode firstNode = nodes.Current;

	PathNode node1 = firstNode;
	PathNode node2 = null;

	while (nodes.MoveNext())
	{
	    node2 = nodes.Current;

	    Gizmos.DrawLine(node1.transform.position, node2.transform.position);

	    if (node2 == firstNode)
		break;

	    node1 = node2;
	}
    }
}