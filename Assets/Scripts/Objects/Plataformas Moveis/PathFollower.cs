using UnityEngine;
using System.Collections.Generic;

public class PathFollower : MonoBehaviour
{
	// path definition
	public PathDefinition mPathDefinition;
	// speed in units per second
	public float mSpeed = 1.0f;
	// stopped or in action?
	public bool mStopped = false;
	// is this a platform? need to make player
	public bool isPlatform = true;

	private enum ePathFollowerState
	{
		Waiting,
		Moving
	}

	// enumerator
	private IEnumerator<PathNode> mPathNodes;

	// inner state of follower
	private ePathFollowerState mState = ePathFollowerState.Waiting;
	// inner waiting time
	private Timer mTimer = new Timer();

	// curren path node, object is in (or going from)
	private PathNode mCurrentPathNode;
	// target path node (or node object is going to)
	private PathNode mTargetPathNode;

	// finished
	private bool mFinished;

	// -------------------------------------------------------------------------
	public void Start()
	{
		if (mSpeed == 0.0f)
			Debug.LogError("Speed cannot be zero!");

		Reset();
	}

	// -------------------------------------------------------------------------
	public void Reset()
	{
		if (mPathDefinition == null)
		{
			Debug.LogError(gameObject + ": path definition cannot be null");
			return;
		}

		mPathNodes = mPathDefinition.GetPathEnumerator();
		mPathNodes.MoveNext();

		mCurrentPathNode = mPathNodes.Current;

		mPathNodes.MoveNext();
		mTargetPathNode = mPathNodes.Current;

		mTimer.Set(mCurrentPathNode.Delay);
		mState = ePathFollowerState.Waiting;

		mFinished = false;
	}

	// -------------------------------------------------------------------------
	public void Update()
	{
		if (mStopped || mFinished)
			return;


		// adjust timer (regardless it is for waiting or moving)
		mTimer.Update(Time.smoothDeltaTime);

		// if not waiting (moving) then adjust position of object
		if (mState == ePathFollowerState.Moving)
		{
			AdjustPosition(mTimer.GetActualRatio());
		}

		// if timer finished its running - do next steps
		if (!mTimer.IsRunning())
		{
			if (mState == ePathFollowerState.Waiting)
			{
				// any event on start of move from current position?
				OnLeaveEvent(mCurrentPathNode.LeaveEvent);
				MoveToTargetNode();

			}
			else if (mState == ePathFollowerState.Moving)
			{
				// any event in the end of move?
				OnArriveEvent(mCurrentPathNode.ArriveEvent);

				// if no new nodes we are finished
				if (!PrepareNextTargetNode())
				{
					mFinished = true;
					return;
				}

				// set waiting state (if any)
				float waitingTime = mCurrentPathNode.Delay;
				if (waitingTime > 0.0f)
				{
					mTimer.Set(waitingTime);
					mState = ePathFollowerState.Waiting;
				}
				else
				{
					OnLeaveEvent(mCurrentPathNode.LeaveEvent);
					MoveToTargetNode();
				}
			}
		}
	}

	// -------------------------------------------------------------------------
	private void MoveToTargetNode()
	{
		// calculate time to move based on nodes positions and speed per unit
		mTimer.Set(CalculateMoveTime());
		// set object as moving
		mState = ePathFollowerState.Moving;
	}

	// -------------------------------------------------------------------------
	private bool PrepareNextTargetNode()
	{
		// set target node as current node
		mCurrentPathNode = mTargetPathNode;

		// read new target node - if no nodes we are finished (only resetting can start it again
		if (!mPathNodes.MoveNext())
		{
			mCurrentPathNode = null;
			mTargetPathNode = null;
			return false;
		}
		
		mTargetPathNode = mPathNodes.Current;
		return true;
	}
	
	// -------------------------------------------------------------------------
	private void AdjustPosition(float aRatio)
	{
		PathNode.eMovementType movementType = mCurrentPathNode.MovementType;

		// adjust ratio based on movement type
		if (movementType == PathNode.eMovementType.Cos)
		{
			// return cos from 0 to 1 in PI range (1...-1 => 0...1)
			aRatio = (-Mathf.Cos(Mathf.PI * aRatio) + 1) / 2.0f;
		}
		else if (movementType == PathNode.eMovementType.Qudratic)
		{
			aRatio *= aRatio;
		}

		transform.position = Vector3.Lerp(mCurrentPathNode.transform.position,
										  mTargetPathNode.transform.position,
										  aRatio);
	}

	// -------------------------------------------------------------------------
	private float CalculateMoveTime()
	{
		Vector3 distance = mTargetPathNode.transform.position -
			mCurrentPathNode.transform.position;

		return (distance.magnitude / mSpeed) * mCurrentPathNode.SpeedModifier;
	}

	// -------------------------------------------------------------------------
	public virtual void OnLeaveEvent(PathNode.ePathNodeEventType aEvent)
	{
		if (aEvent != PathNode.ePathNodeEventType.Nothing)
			Debug.Log("Leave event: " + aEvent);
	}

	// -------------------------------------------------------------------------
	public virtual void OnArriveEvent(PathNode.ePathNodeEventType aEvent)
	{
		if (aEvent != PathNode.ePathNodeEventType.Nothing)
			Debug.Log("Arrive event: " + aEvent);
	}

	//--------------------------------------------------------------------------
	void OnCollisionStay2D(Collision2D colisor){
		//Se esse objeto for definido como plataforma o player poderá ficar em cima do objeto
		if(isPlatform){
			//se o objeto que encostar na plataforma tiver o nome "Player" ele herdará as mesmas funções
			//do transform do objeto
			if (colisor.gameObject.name == "Player") {
				colisor.gameObject.transform.parent = transform;
			}
		}
	}

	void OnCollisionExit2D(Collision2D colisor){
		//Se o objeto for definido como plataforma o player poderá ficar em cima do objeto
		if(isPlatform){
			//se o objeto com nome player sair da plataforma então deixará de herdar as caracteristicas do objeto plataforma
			if (colisor.gameObject.name == "Player") {
				colisor.gameObject.transform.parent = null;
			}
		}
	}
}