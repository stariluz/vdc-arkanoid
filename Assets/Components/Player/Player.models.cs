using System;
using UnityEngine;
using AYellowpaper.SerializedCollections;

[Serializable]
public class PlayerReferences
{
    [SerializeField] public Player player;
}

[Serializable]
public class PlayerUI
{
    [SerializeField] public TMPro.TMP_Text score;
    [SerializeField] public TMPro.TMP_Text lives;
}