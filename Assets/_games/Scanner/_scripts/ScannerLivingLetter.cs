﻿using UnityEngine;
using System.Collections;

namespace EA4S.Scanner
{

	public class ScannerLivingLetter : MonoBehaviour {

		private enum LLStatus { Sliding, StandingOnBelt, RunningFromAntura, Angry, Happy };

		public GameObject livingLetter;

		public float slideSpeed = 2f;

		private LLStatus status;
		// Use this for initialization
		void Start () {
			livingLetter.GetComponent<LetterObjectView>().Falling = true;
            status = LLStatus.Sliding;
		}

		// Update is called once per frame
		void Update () {
			if (status == LLStatus.Sliding)
			{
				transform.Translate(slideSpeed * Time.deltaTime, -slideSpeed * Time.deltaTime / 2,0);
			}
		}

		IEnumerator RotateGO(GameObject go, Vector3 toAngle, float inTime) {
			var fromAngle = go.transform.rotation;
			var destAngle = Quaternion.Euler(toAngle);
			for(var t = 0f; t < 1; t += Time.deltaTime/inTime) {
				go.transform.rotation = Quaternion.Lerp(fromAngle, destAngle, t);
				yield return null;
			}
		}

		IEnumerator AnimateLL()
		{

			yield return new WaitForSeconds(1f);

			int index = -1;
			LLAnimationStates[] animations = 
			{
				LLAnimationStates.LL_idle,
				LLAnimationStates.LL_dancing, 
				//LLAnimationStates.LL_walking
			};

			do
			{
				int oldIndex = index;
				do
				{
					index = UnityEngine.Random.Range(0, animations.Length);
				} while (index == oldIndex);
				livingLetter.GetComponent<LetterObjectView>().SetState(animations[index]);
				yield return new WaitForSeconds(UnityEngine.Random.Range(2f, 4f));
			} while (status == LLStatus.StandingOnBelt);
		}

		void OnTriggerEnter(Collider other) 
		{
			Debug.Log("Slide Trigger entered");
			if (status == LLStatus.Sliding)
			{
				if (other.tag == ScannerGame.TAG_BELT)
				{
					transform.parent = other.transform;
					status = LLStatus.StandingOnBelt;
					livingLetter.GetComponent<LetterObjectView>().Falling = false;
					StartCoroutine(RotateGO(livingLetter, new Vector3(0,180,0),1f));
					StartCoroutine(AnimateLL());
				}
			}
		}
        
	}
}
