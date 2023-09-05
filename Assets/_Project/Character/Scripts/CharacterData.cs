using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/New Character")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public int cost;
    public int width;
    public int height;
    public PlacedCharacter placedCharacter;
}
