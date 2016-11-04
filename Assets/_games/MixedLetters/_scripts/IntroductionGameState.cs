﻿using UnityEngine;
using System.Collections.Generic;
namespace EA4S.MixedLetters
{
    public class IntroductionGameState : IGameState
    {
        MixedLettersGame game;

        private float anturaEnterTimer;
        private bool anturaEntered = false;
        private float anturaExitTimer;
        private bool anturaExited = false;

        public IntroductionGameState(MixedLettersGame game)
        {
            this.game = game;
        }

        public void EnterState()
        {
            anturaEnterTimer = Random.Range(1f, 2f);
            anturaEntered = false;
            anturaExitTimer = Random.Range(0.75f, 1.5f);
            anturaExited = false;
            game.GenerateNewWord();
            VictimLLController.instance.Enable();
            Vector3 victimLLPosition = VictimLLController.instance.transform.position;
            victimLLPosition.x = Random.Range(0, 40) % 2 == 0 ? 0.5f : -0.5f;
            VictimLLController.instance.SetPosition(victimLLPosition);

            game.roundNumber++;
        }

        public void ExitState()
        {
        }

        public void Update(float delta)
        {
            anturaEnterTimer -= delta;
            if (anturaEnterTimer < 0 && !anturaEntered)
            {
                AnturaController.instance.Enable();
                AnturaController.instance.EnterScene(OnFightBegan);
                anturaEntered = true;
            }

            if (anturaExited)
            {
                anturaExitTimer -= delta;

                if (anturaExitTimer < 0)
                {
                    game.SetCurrentState(game.PlayState);
                }
            }
        }

        public void UpdatePhysics(float delta)
        {
        }

        public void OnFightBegan()
        {
            AnturaController.instance.SetPosition(VictimLLController.instance.transform.position);
            AnturaController.instance.Disable();
            VictimLLController.instance.Disable();
            ParticleSystemController.instance.Enable();
            ParticleSystemController.instance.SetPosition(VictimLLController.instance.transform.position);
            SeparateLettersSpawnerController.instance.SetPosition(VictimLLController.instance.transform.position);
            SeparateLettersSpawnerController.instance.SpawnLetters(game.lettersInOrder, OnFightEnded);
        }

        public void OnFightEnded()
        {
            AnturaController.instance.Enable();
            AnturaController.instance.SetPositionWithOffset(VictimLLController.instance.transform.position, new Vector3(0, 0, 1f));
            ParticleSystemController.instance.Disable();
            anturaExited = true;
        }
    }
}