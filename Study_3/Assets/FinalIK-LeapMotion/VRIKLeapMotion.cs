using UnityEngine;
using System.Collections;
using RootMotion.FinalIK;
using System;
using System.Collections.Generic;

namespace RootMotion.Demos {
	
	// Connects a 3D character to the LeapMotin hand models using the VRIK component.
	public class VRIKLeapMotion : MonoBehaviour {
		
		#region Editor
		
		[ContextMenu("Assign Fingers Automatically")]
		void AutoAssignFingers() {
			if (ik == null) {
				Debug.LogError("'Ik' unassigned in VRIKLeapMotion, can not find the fingers.", transform);
				return;
			}
			leftHand.fingers = FindFingers(ik.references.leftHand);
			rightHand.fingers = FindFingers(ik.references.rightHand);
		}
		
		private List<Finger> FindFingers(Transform hand) {
			List<Finger> fingers = new List<Finger>(0);
			
			for (int i = 0; i < hand.childCount; i++) {
				Finger finger = new Finger();
				AddFingerBonesRecursive(hand.GetChild(i), ref finger);
				fingers.Add(finger);
			}
			
			return fingers;
		}
		
		private void AddFingerBonesRecursive(Transform transform, ref Finger finger) {
			finger.bones.Add(transform);
			if (transform.childCount != 1) return;
			
			AddFingerBonesRecursive(transform.GetChild(0), ref finger);
		}
		
		#endregion Editor

		[System.Serializable]
		public class Hand {
			
			public Transform target;
			public List<Finger> fingers;

			public Vector3 handPositionOffset;

			[Space(5)]

			[Tooltip("Local axis of the character's hand bone that is pointing from the wrist towards the palm.")] public Vector3 handWristToPalmAxis;
			[Tooltip("Local axis of the character's hand bone that is pointing from the palm towards the thumb.")] public Vector3 handPalmToThumbAxis;
			[Tooltip("Local axis of the 'Target' transform that is pointing from the wrist towards the palm.")] public Vector3 handTargetWristToPalmAxis;
			[Tooltip("Local axis of the 'Target' transform that is pointing from the palm towards the thumb.")] public Vector3 handTargetPalmToThumbAxis;

			[Space(5)]

			[Tooltip("Local axis of the character's finger bones that is pointing towards the next finger bone")] public Vector3 fingerAxis;
			[Tooltip("Local axis of the character's finger bones that would be pointing up if the hands were resting on a table.")] public Vector3 fingerUpAxis;
			[Tooltip("Local axis of the target finger bones that is pointing towards the next finger bone")] public Vector3 fingerTargetAxis = Vector3.right;
			[Tooltip("Local axis of the target finger bones that would be pointing up if the hands were resting on a table.")] public Vector3 fingerTargetUpAxis = Vector3.up;

			public void Initiate() {
				foreach (Finger finger in fingers) finger.targetRotations = new Quaternion[finger.bones.Count];
			}

			public Quaternion ConvertHandRotation(Quaternion q) {
				return MatchRotation(q, handTargetWristToPalmAxis, handTargetPalmToThumbAxis, handWristToPalmAxis, handPalmToThumbAxis);
			}

			public Quaternion ConvertFingerRotation(Quaternion q) {
				return MatchRotation(q, fingerTargetAxis, fingerTargetUpAxis, fingerAxis, fingerUpAxis);
			}

			public Quaternion MatchRotation(Quaternion targetRotation, Vector3 targetforwardAxis, Vector3 targetUpAxis, Vector3 forwardAxis, Vector3 upAxis) {
				Quaternion f = Quaternion.LookRotation(forwardAxis, upAxis);
				Quaternion fTarget = Quaternion.LookRotation(targetforwardAxis, targetUpAxis);

				Quaternion d = targetRotation * fTarget;
				return d * Quaternion.Inverse(f);
			}
		}

		// Defines the character's fingers
		[System.Serializable]
		public class Finger {
			public List<Transform> bones = new List<Transform>(0);
			public List<Transform> targetBones = new List<Transform>();

			[HideInInspector] public Quaternion[] targetRotations = new Quaternion[0];
		}
		
		public VRIK ik;
			
		[Header("Hands")]

		[Tooltip("How fast will IK weights be blended in/out when the hand is found/lost")] public float weightSpeed = 3f;
		public Hand leftHand;
		public Hand rightHand;

		[Header("Elbows")]
		[Range(0f, 1f)] public float bendGoalWeight = 0.1f;
		
		void Start() {
			leftHand.Initiate();
			rightHand.Initiate();
		}
		
		void LateUpdate() {
			UpdateHandTarget(ik.solver.leftArm, leftHand, ik.references.leftHand);
			UpdateHandTarget(ik.solver.rightArm, rightHand, ik.references.rightHand);
		}

		private void UpdateHandTarget(IKSolverVR.Arm arm, Hand hand, Transform handBone) {
			float weightTarget = hand.target.gameObject.activeInHierarchy? 1f: 0f;
			arm.positionWeight = Mathf.MoveTowards (arm.positionWeight, weightTarget, Time.deltaTime * weightSpeed);
			arm.rotationWeight = arm.positionWeight;

			arm.IKRotation = hand.ConvertHandRotation(hand.target.rotation);
			arm.IKPosition = hand.target.position + hand.target.rotation * hand.handPositionOffset;

			// Get the finger target rotations
			for (int i = 0; i < hand.fingers.Count; i++) {
				for (int b = 0; b < hand.fingers[i].bones.Count; b++) {
					hand.fingers[i].targetRotations[b] = hand.fingers[i].targetBones[b].rotation;
				}
			}
				
			// Cannot use elbow position because Leap does not know the shoulder position
			arm.bendDirection = arm.IKRotation * Vector3.forward;

			// Rotate the hand bone.
			handBone.localRotation = Quaternion.Slerp(handBone.localRotation, Quaternion.Inverse(handBone.parent.rotation) * arm.IKRotation, arm.positionWeight);

			foreach (Finger finger in hand.fingers) {
				for (int b = 0; b < finger.bones.Count; b++) {
					finger.bones[b].rotation = Quaternion.Slerp(finger.bones[b].rotation, hand.ConvertFingerRotation(finger.targetRotations[b]), arm.positionWeight);
				}
			}
			
			// Apply bend goal weight
			arm.bendGoalWeight = arm.positionWeight * bendGoalWeight;
		}
	}
}
