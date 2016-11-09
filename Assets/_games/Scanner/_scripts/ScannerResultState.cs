﻿using System;
using UnityEngine;

namespace EA4S.Scanner
{
	public class ScannerResultState : IGameState
	{
		ScannerGame game;

		float timer = 4;

		public ScannerResultState(ScannerGame game)
		{
			this.game = game;
		}

		public void EnterState()
		{
			timer = 4;
			game.Context.GetAudioManager().PlayMusic(Music.Relax);

		}

		public void ExitState()
		{
		}

		public void Update(float delta)
		{
			timer -= delta;

			if (timer < 0)
			{
				Debug.Log("Stars: " + game.CurrentStars);

				game.EndGame(game.CurrentStars, game.CurrentScoreRecord);
			}
		}

		public void UpdatePhysics(float delta)
		{
		}
	}
}
