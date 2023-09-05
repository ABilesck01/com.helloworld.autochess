using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : BaseController
{
    [SerializeField] private Vector2Int minMaxUnitsCount;

    private void Awake()
    {
        gridBuilder = GetComponentInChildren<GridBuilder>();
        gridBuilder.SetController(this);
    }

    private void Start()
    {
        SpawnRandomCharacters();
    }

    private CharacterData GetRandomCharacter()
    {
        int rand = UnityEngine.Random.Range(0, characterDataDeck.Count);
        return characterDataDeck[rand];
    }

    private void SpawnRandomCharacters()
    {
        int rand = UnityEngine.Random.Range(minMaxUnitsCount.x, minMaxUnitsCount.y);
        for (int i = 0; i < rand; i++)
        {
            gridBuilder.PlaceCharacterInRandomPosition(GetRandomCharacter());
        }
    }

    private void ShowUnits()
    {
        foreach (var placed in placedUnits)
        {
            placed.gameObject.SetActive(true);
        }
    }

    public void Play()
    {
        ShowUnits();
        ReadyUnits();
    }

    public void Respawn()
    {
        Invoke(nameof(SpawnRandomCharacters), 2.5f);
    }
}
