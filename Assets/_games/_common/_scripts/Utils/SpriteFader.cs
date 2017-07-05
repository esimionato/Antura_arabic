﻿using UnityEngine;

namespace Antura.MinigamesCommon
{
    /// <summary>
    /// Fades a sprite from-to a specific transparency value in time.
    /// </summary>
    public class SpriteFader : MonoBehaviour
    {
        public bool show = true;
        public float fadeSpeed = 2.0f;

        private SpriteRenderer sprite;
        private float startAlpha;
        private float currentAlpha;

        private bool overwrittenAlpha = false;

        void Awake()
        {
            sprite = GetComponent<SpriteRenderer>();

            startAlpha = sprite.color.a;
            if (overwrittenAlpha) {
                var old = sprite.color;
                old.a = currentAlpha;
                sprite.color = old;
            } else {
                currentAlpha = startAlpha;
            }
        }

        void Update()
        {
            var old = sprite.color;

            var target = startAlpha * (show ? 1 : 0);

            var distance = target - currentAlpha;

            float delta = fadeSpeed * Time.deltaTime;

            if (target <= 0.001f) {
                currentAlpha -= delta;

                if (currentAlpha < 0)
                    currentAlpha = 0;
            } else {
                if (Mathf.Abs(distance) > 0.05f) {
                    currentAlpha += Mathf.Sign(distance) * delta;
                }
            }

            old.a = currentAlpha;
            sprite.color = old;

            var enabled = currentAlpha > 0.001f;
            if (sprite.enabled != enabled) {
                sprite.enabled = enabled;
            }
        }

        public void SetAlphaImmediate(float alpha)
        {
            currentAlpha = alpha;
            overwrittenAlpha = true;
        }
    }
}