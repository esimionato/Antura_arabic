﻿using UnityEngine;
using System.Collections;
using System;
using EA4S.UI;
using ModularFramework.Core;
using ModularFramework.Modules;

namespace EA4S.Core
{
    /// <summary>
    /// Module that handles scene load and transitions.
    /// </summary>
    // refactor: can we remove the ModularFramerwork?
    public class SceneModule : SceneModuleDefault
    {

        public override void LoadSceneWithTransition(string _sceneToLoad, SceneTransition _transitionSettings)
        {
            SceneTransitioner.Show(true, delegate {
                sceneTransitionDone(_sceneToLoad);
            });
        }

        void sceneTransitionDone(string _sceneToLoad)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(_sceneToLoad);
        }

        public override void SceneLoadedBehaviour()
        {
            if (SceneTransitioner.IsShown) {
                AppManager.I.StartCoroutine(CloseSceneTransitioner(0.15f));
            }
        }

        IEnumerator CloseSceneTransitioner(float _waitTime)
        {
            yield return new WaitForSeconds(_waitTime);
            SceneTransitioner.Show(false);
            yield return null;
        }

    }

    /// <summary>
    /// Settings for this implementation of DataModule.
    /// </summary>
    [Serializable]
    public class SceneModuleSettings : IModuleSettings
    {

    }
}