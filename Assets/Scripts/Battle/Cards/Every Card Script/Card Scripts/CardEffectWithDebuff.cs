using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardEffectWithDebuff : CardEffect
{
    [SerializeField] protected int _duration;
    [SerializeField] protected GameObject _buffable;
    [SerializeField] Sprite _buffArtwork;
    public Sprite BuffArtworkCard => _buffArtwork;
}
