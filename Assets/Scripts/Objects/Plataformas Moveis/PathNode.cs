using UnityEngine;

public class PathNode : MonoBehaviour
{
	public enum ePathNodeEventType
	{
		Nothing,
		Hit
	}

	public enum eMovementType
	{
		Linear,
		Cos,
		Qudratic
	}

	public eMovementType mMovementType = eMovementType.Linear;

	// delay in this point in seconds
	public float mDelay = 0.0f;

	public ePathNodeEventType mOnArriveEvent;
	public ePathNodeEventType mOnLeaveEvent;

	public float mSpeedModifier = 1.0f;

	// -------------------------------------------------------------------------
	public float Delay
	{
		get
		{
			return mDelay;
		}
	}

	// -------------------------------------------------------------------------
	public eMovementType MovementType
	{
		get
		{
			return mMovementType;
		}
	}

	// -------------------------------------------------------------------------
	public ePathNodeEventType ArriveEvent
	{
		get
		{
			return mOnArriveEvent;
		}
	}

	// -------------------------------------------------------------------------
	public ePathNodeEventType LeaveEvent
	{
		get
		{
			return mOnLeaveEvent;
		}
	}

	// -------------------------------------------------------------------------
	public float SpeedModifier
	{
		get
		{
			return mSpeedModifier;
		}
	}
}