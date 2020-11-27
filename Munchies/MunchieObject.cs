using FistVR;
using UnityEngine;
using UnityEngine.Events;

namespace Munchies
{
	[AddComponentMenu("")]
	public class MunchieObject : FVRPhysicalObject
	{
		public AudioEvent EatSound;
		public GameObject[] EatStages;
		public UnityEvent EatEvent;
		public float EatDelay;
		public bool DestroyOnEat;

		private float _lastEatTime = float.MinValue;
		private int _currentStage;

		public static MunchieObject CopyFrom(FVRPhysicalObject original, AudioEvent audioEvent)
		{
			original.gameObject.SetActive(false);
			var real = original.gameObject.AddComponent<MunchieObject>();
			real.ControlType = original.ControlType;
			real.IsSimpleInteract = original.IsSimpleInteract;
			real.HandlingGrabSound = original.HandlingGrabSound;
			real.HandlingReleaseSound = original.HandlingReleaseSound;
			real.PoseOverride = original.PoseOverride;
			real.QBPoseOverride = original.QBPoseOverride;
			real.PoseOverride_Touch = original.PoseOverride_Touch;
			real.UseGrabPointChild = original.UseGrabPointChild;
			real.UseGripRotInterp = original.UseGripRotInterp;
			real.PositionInterpSpeed = original.PositionInterpSpeed;
			real.RotationInterpSpeed = original.RotationInterpSpeed;
			real.EndInteractionIfDistant = original.EndInteractionIfDistant;
			real.EndInteractionDistance = original.EndInteractionDistance;
			real.UXGeo_Held = original.UXGeo_Held;
			real.UXGeo_Hover = original.UXGeo_Hover;
			real.UseFilteredHandTransform = original.UseFilteredHandTransform;
			real.UseFilteredHandRotation = original.UseFilteredHandRotation;
			real.UseFilteredHandPosition = original.UseFilteredHandPosition;
			real.UseSecondStepRotationFiltering = original.UseSecondStepRotationFiltering;
			real.ObjectWrapper = original.ObjectWrapper;
			real.SpawnLockable = original.SpawnLockable;
			real.Harnessable = original.Harnessable;
			real.HandlingReleaseIntoSlotSound = original.HandlingReleaseIntoSlotSound;
			real.Size = original.Size;
			real.QBSlotType = original.QBSlotType;
			real.ThrowVelMultiplier = original.ThrowVelMultiplier;
			real.ThrowAngMultiplier = original.ThrowAngMultiplier;
			real.UsesGravity = original.UsesGravity;
			real.DependantRBs = original.DependantRBs;
			real.DistantGrabbable = original.DistantGrabbable;
			real.IsDebug = original.IsDebug;
			real.IsAltHeld = original.IsAltHeld;
			real.IsKinematicLocked = original.IsKinematicLocked;
			real.DoesQuickbeltSlotFollowHead = original.DoesQuickbeltSlotFollowHead;
			real.IsInWater = original.IsInWater;
			real.AttachmentMounts = original.AttachmentMounts;
			real.IsAltToAltTransfer = original.IsAltToAltTransfer;
			real.CollisionSound = original.CollisionSound;
			real.IsPickUpLocked = original.IsPickUpLocked;
			real.OverridesObjectToHand = original.OverridesObjectToHand;
			real.MP = original.MP;
			real.EatSound = audioEvent;
			Destroy(original);
			original.gameObject.SetActive(true);
			return real;
		}

		public override void UpdateInteraction(FVRViveHand hand)
		{
			base.UpdateInteraction(hand);

			// If we're already completely eaten return
			if (_currentStage == EatStages.Length) return;

			// Check if we're close enough to be eaten
			if (Vector3.Distance(transform.position, GM.CurrentPlayerBody.Head.transform.position + GM.CurrentPlayerBody.Head.transform.up * -0.2f) >= 0.150000005960464)
				return;

			// Check if it's been long enough since the last time we were eaten
			if (_lastEatTime + EatDelay > Time.time) return;

			// Play the eat sound
			_lastEatTime = Time.time;
			SM.PlayGenericSound(EatSound, transform.position);
			EatEvent.Invoke();
			if (_currentStage != EatStages.Length -  1 || DestroyOnEat) EatStages[_currentStage].SetActive(false);
			_currentStage++;

			// If we've eaten all the stages
			if (_currentStage == EatStages.Length && DestroyOnEat)
			{
				EndInteraction(hand);
				hand.ForceSetInteractable(null);
				Destroy(gameObject);
			} else EatStages[_currentStage].SetActive(true);
		}
	}
}
