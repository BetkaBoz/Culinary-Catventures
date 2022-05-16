using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this is a set of interfaces used to make card effects
//I'm not happy with this code will change it after presentation
public interface IFeed
{
    void Feed(Customer target, int amount);
}
public interface ISpendEnergy
{
    void SpendEnergy(GameManager gm, int amount);
}
public interface IDrawCards
{
    void DrawCards(GameManager gm, int amount);
}
public interface IDiscardAll
{
    void DiscardAll(GameManager gm);
}
public interface IGainCard
{
    void GainCard(string name);
}
public interface IStun
{
    void StunEnemy(Customer target, int amount);
}
//Add other interfaces after you figure out how buffing works
public interface IChangeFoodPower
{
    void ChangeFoodPower(GameManager gm, float amount, string type);
}

