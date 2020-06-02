using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameContorlManager : MonoSingleton<GameContorlManager>
{
    public UiContorlManager _uiContorlManager;
    public InputManager _inputManager;
    public PlayerCollider _playerCollider;
    public WaveManger _waveManger;
    public ZombiesMananger _zombiesMananger;
}
