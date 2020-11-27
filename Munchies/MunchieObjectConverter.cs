using System.Collections.Generic;
using FistVR;
using UnityEngine;
using UnityEngine.Events;

namespace Munchies
{
	public class MunchieObjectConverter : MonoBehaviour
	{
		[Tooltip("These are game objects that get enabled and disabled to represent each stage of eating")]
		public GameObject[] States;

		[Tooltip("How quickly the player is allowed to take another bite")]
		public float EatDelay;

		public bool DestroyOnEat;

		public UnityEvent EatEvent;

		public AudioClip EatSound;

		private void Awake()
		{
			var real = MunchieObject.CopyFrom(GetComponent<FVRPhysicalObject>(), new AudioEvent
			{
				Clips = new List<AudioClip>(new []{EatSound}),
				ClipLengthRange = Vector2.one,
				PitchRange = new Vector2(0.95f, 1.05f),
				VolumeRange = new Vector2(0.95f, 1.05f),
			});
			real.EatStages = States;
			real.EatEvent = EatEvent;
			real.EatDelay = EatDelay;
			real.DestroyOnEat = DestroyOnEat;
			Destroy(this);
		}
	}
}
