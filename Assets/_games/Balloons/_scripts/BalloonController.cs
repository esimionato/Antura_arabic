﻿using UnityEngine;
using System.Collections;
using EA4S;

namespace Balloons
{
    public class BalloonController : MonoBehaviour
    {
        public FloatingLetterController parentFloatingLetter;
        public Collider balloonCollider;
        public Renderer balloonRenderer;
        public Animator animator;

        private int taps = 0;

        // Middle balloon adjustment for Triple Balloon Variation
        private bool adjustMiddleBalloon = false;
        private Vector3 adjustedLocalPosition = new Vector3(0f, 3.5f, 0f);
        private float adjustmentDuration = 7.5f;
        private float adjustmentProgress;
        private float adjustmentProgressRatio;

        void Start()
        {
            animator = GetComponent<Animator>();
        }

        void FixedUpdate()
        {
            if (adjustMiddleBalloon)
            {
                if (adjustmentProgress < adjustmentDuration)
                {
                    transform.localPosition = Vector3.Lerp(transform.localPosition, adjustedLocalPosition, adjustmentProgressRatio);
                    adjustmentProgress += Time.deltaTime;
                    adjustmentProgressRatio = adjustmentProgress / adjustmentDuration;
                }
                else
                {
                    adjustMiddleBalloon = false;
                }
            }
        }

        public void OnMouseDown()
        {
            TapAction();
        }

        void TapAction()
        {
            taps++;
            if (taps >= parentFloatingLetter.tapsNeeded)
            {
                Pop();
            }
            else
            {
                animator.SetTrigger("Tap");
            }
        }
            
        public void Pop()
        {
            balloonCollider.enabled = false;
            parentFloatingLetter.Pop();
            AudioManager.I.PlaySfx(Sfx.BaloonPop);
            animator.SetBool("Pop", true);
        }

        public void AdjustMiddleBalloon()
        {
            adjustmentProgress = 0f;
            adjustmentProgressRatio = 0f;
            adjustMiddleBalloon = true;
        }

        public void DisableCollider()
        {
            balloonCollider.enabled = false;
        }

        public void SetColor(Color color)
        {
            balloonRenderer.material.color = color;
        }
    }
}