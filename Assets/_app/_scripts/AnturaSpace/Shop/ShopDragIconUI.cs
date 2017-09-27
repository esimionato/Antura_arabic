﻿using Antura.Core;
using Antura.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.AnturaSpace
{
    public class ShopDragIconUI : MonoBehaviour
    {
        public Image iconUI;

        private void Start()
        {
            ShopDecorationsManager.I.OnDragStart += HandleDragStart;
            ShopDecorationsManager.I.OnDragStop += HandleDragStop;
        }

        private bool isDragging = false;

        private void HandleDragStart(ShopDecorationObject decorationObject)
        {
            isDragging = true;
            iconUI.sprite = decorationObject.iconSprite;
        }

        private void HandleDragStop()
        {
            isDragging = false;
        }

        private void Update()
        {
            if (isDragging)
            {
                iconUI.rectTransform.anchoredPosition = Input.mousePosition - new Vector3(Screen.width/2, Screen.height/2);
            }
            else
            {
                iconUI.rectTransform.anchoredPosition = Vector3.right * 10000;
            }
        }

    }
}