﻿using EA4S.Core;
using EA4S.Helpers;
using EA4S.Rewards;
using SQLite;

namespace EA4S.Database
{
    /// <summary>
    /// Serialized data relative to a reward, used for unlocking. Updated at runtime.
    /// </summary>
    [System.Serializable]
    public class RewardPackUnlockData : IData
    {
        /// <summary>
        /// Primary key for the database.
        /// </summary>
        [PrimaryKey]
        public string Id { get; set; }

        #region Reward Keys

        /// <summary>
        /// Part of the keys used to define the complete reward.
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// Part of the keys used to define the complete reward.
        /// </summary>
        public string ColorId { get; set; }

        /// <summary>
        /// Part of the keys used to define the complete reward.
        /// </summary>
        public RewardTypes Type { get; set; }

        #endregion

        /// <summary>
        /// The play session at which this reward is assigned.
        /// </summary>
        public string JourneyPositionId { get; set; }

        /// <summary>
        /// The order of playsession rewards in case of multi reward for same playsession.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// True if never used by player.
        /// </summary>
        public bool IsNew { get; set; }

        /// <summary>
        /// True if not unlocked yet
        /// </summary>
        public bool IsLocked { get; set; }

        /// <summary>
        /// Timestamp of creation of the reward.
        /// </summary>
        public int CreationTimestamp { get; set; }


        public RewardPackUnlockData()
        {
        }

        public RewardPackUnlockData(string itemId, string colorId, RewardTypes type, JourneyPosition journeyPosition)
        {
            ItemId = itemId;
            ColorId = colorId;
            Type = type;
            Id = GetIdAccordingToDBRules();
            JourneyPositionId = journeyPosition.ToStringId();
            Order = 0;
            IsNew = true;
            IsLocked = true;
            CreationTimestamp = GenericHelper.GetTimestampForNow();
        }

        public string GetIdAccordingToDBRules()
        { 
            return ItemId + "." + ColorId + "." + Type;
        }

        #region Rewards API

        public MaterialPair GetMaterialPair()
        {
            return RewardSystemManager.GetMaterialPairFromRewardIdAndColorId(ItemId, ColorId);
        }

        public Reward GetReward()
        {
            if (Type != RewardTypes.reward)
                return null;
            return RewardSystemManager.GetConfig().Rewards.Find(r => r.ID == ItemId);
        }

        public string GetRewardCategory()
        {
            if (Type != RewardTypes.reward)
                return string.Empty;
            Reward reward = RewardSystemManager.GetConfig().Rewards.Find(r => r.ID == ItemId);
            if (reward != null)
                return reward.Category;
            return string.Empty;
        }

        public JourneyPosition GetJourneyPosition()
        {
            return new JourneyPosition(JourneyPositionId);
        }

        #endregion

        #region Database API

        public string GetId()
        {
            return Id;
        }

        public override string ToString()
        {
            return string.Format("{0} : {1} [{2}] [{3}]", ItemId, ColorId, Type, JourneyPositionId);
        }

        #endregion
    }
}