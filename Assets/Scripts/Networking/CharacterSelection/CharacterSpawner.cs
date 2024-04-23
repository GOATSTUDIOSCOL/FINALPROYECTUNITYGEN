using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class CharacterSpawner : NetworkBehaviour
{

    [SerializeField] private CharacterDataBase characterDatabase;
    public const string KEY_PLAYER_CHARACTER = "Character";
    public override void OnNetworkSpawn()
    {

    }
}
