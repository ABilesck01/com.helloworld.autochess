using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseController
{
    private PlayerView playerView;

    public static event EventHandler OnPlayerReady;

    private void Awake()
    {
        gridBuilder = GetComponentInChildren<GridBuilder>();
        gridBuilder.SetController(this);
        playerView = GetComponentInChildren<PlayerView>();
        playerView.SetPlayerController(this);
    }

    private void Start()
    {
        playerView.UpdateCharatersList(characterDataDeck);
    }

    public void SetCharacterToBuild(CharacterData d)
    {
        gridBuilder.SetPlacedCharacter(d);
    }

    public void PlayGame()
    {
        OnPlayerReady?.Invoke(this, EventArgs.Empty);
        ReadyUnits();
    }
}
