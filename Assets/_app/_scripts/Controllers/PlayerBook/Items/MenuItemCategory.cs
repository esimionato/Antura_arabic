﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using EA4S.Db;

namespace EA4S
{
    public class MenuItemCategory : MonoBehaviour, IPointerClickHandler
    {
        CategoryData data;
        public TextRender Title;
        BookPanel manager;

        public void Init(BookPanel _manager, CategoryData _data)
        {
            data = _data;
            manager = _manager;

            Title.text = data.Title;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            manager.SelectSubCategory(data);
        }
    }
}