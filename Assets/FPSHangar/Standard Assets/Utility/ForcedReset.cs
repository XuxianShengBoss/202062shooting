using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

<<<<<<< HEAD
//[RequireComponent(typeof (//GUITexture))]
=======
//[RequireComponent(typeof (GUITexture))]
>>>>>>> 3f6843efe8a77428223184835104db02f3c8eb23
public class ForcedReset : MonoBehaviour
{
    private void Update()
    {
        // if we have forced a reset ...
        if (CrossPlatformInputManager.GetButtonDown("ResetObject"))
        {
            //... reload the scene
            Application.LoadLevelAsync(Application.loadedLevelName);
        }
    }
}
