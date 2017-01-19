﻿using UnityEngine;

namespace EA4S.Db
{
    /// <summary>
    /// Custom asset container for LetterData. 
    /// </summary>
    public class LetterDatabase : ScriptableObject
    {
        [SerializeField]
        public LetterTable table;
    }

}