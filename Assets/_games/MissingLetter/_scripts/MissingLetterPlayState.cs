﻿using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace EA4S.MissingLetter
{
    public class MissingLetterPlayState : IGameState
    {
        

        public MissingLetterPlayState(MissingLetterGame game)
        {
            this.game = game;

            gameTime.onTimesUp += OnTimesUp;
            
        }

        public void EnterState()
        {
            game.m_RoundManager.onAnswered += OnRoundResult;
            game.mIsTimesUp = false;
            game.ResetScore();

            // Reset game timer
            gameTime.Reset();
            gameTime.Start();

            game.timerText.gameObject.SetActive(true);
            game.timerText.text = "";

            hurryUpSfx = false;

            //game.Context.GetAudioManager().PlayMusic(Music.MainTheme);

            game.m_RoundManager.NewRound();
        }

        public void ExitState()
        {
            if (timesUpAudioSource != null)
                timesUpAudioSource.Stop();

            game.timerText.gameObject.SetActive(false);
            gameTime.Stop();
        }

        public void Update(float delta)
        {

            if(game.miAnturaTriggersIndex < game.mafAnturaEnterTriggers.Length && gameTime.Time <= game.mafAnturaEnterTriggers[game.miAnturaTriggersIndex]) {
                ++game.miAnturaTriggersIndex;
                game.mAnturaRef.GetComponent<AnturaBehaviour>().EnterScene(game.mfAnturaAnimDuration);
                game.StartCoroutine(Utils.LaunchDelay(game.mfAnturaAnimDuration / 6, game.m_RoundManager.ShuffleLetters, game.mfAnturaAnimDuration / 2));
            }

            game.timerText.text = String.Format("{0:0}", gameTime.Time);

            if (!hurryUpSfx)
            {
                if (gameTime.Time < 4f)
                {
                    hurryUpSfx = true;

                    timesUpAudioSource = game.Context.GetAudioManager().PlaySound(Sfx.DangerClockLong);
                }
            }

            gameTime.Update(delta);
        }

        public void UpdatePhysics(float delta)
        {
        }



        void OnTimesUp()
        {
            // Time's up!
            game.mIsTimesUp = true;
            game.SetCurrentState(game.ResultState);
        }


        void OnRoundResult(bool _result) {
            game.OnResult(_result);
            game.m_RoundManager.NewRound();
        }

        #region VARS

        CountdownTimer gameTime = new CountdownTimer(60.0f);
        MissingLetterGame game;
        IAudioSource timesUpAudioSource;
        bool hurryUpSfx;

        #endregion
    }
}
