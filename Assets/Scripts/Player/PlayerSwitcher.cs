using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSwitcher : MonoBehaviour
{
    int index = 0;
    [SerializeField] List<GameObject> figthers = new List<GameObject>();
    PlayerInputManager manager;



    private void Start()
    {
        manager = GetComponent<PlayerInputManager>();
        index = Random.Range(0, figthers.Count);
        manager.playerPrefab = figthers[index];

    }

    public void SwitchCharacter()
    {
        index = Random.Range(0, figthers.Count);
        manager.playerPrefab = figthers[index];
    }


}
