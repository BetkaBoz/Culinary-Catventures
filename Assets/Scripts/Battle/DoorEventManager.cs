using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class DoorEventManager : MonoBehaviour
{
    [SerializeField] private List<string> doorSequentialQuotes = new List<string>();
    private int currentQuoteFromSequence = 0;

    [SerializeField] private List<string> doorRandomQuotes = new List<string>();
    private LinkedList<string> _doorRandomQuotes;

    [SerializeField] private float quoteHideAfterSeconds = 5;
    [SerializeField] private float quoteFadingDuration = 5;

    string lastChosenMessage = "";

    private void Start()
    {
        _doorRandomQuotes = new LinkedList<string>(doorRandomQuotes);
    }

    // private void OnMouseEnter()
    // {
    //     showAnotherQuote();
    // }

    private void OnMouseDown()
    {
        showAnotherQuote();
    }

    // private void OnMouseExit()
    // {
    //     TooltipManager.Hide();
    // }

    void showAnotherQuote()
    {
        string message = "";

        if (currentQuoteFromSequence < doorSequentialQuotes.Count)
        {
            if (doorSequentialQuotes.Count > 0)
            {
                message = doorSequentialQuotes[currentQuoteFromSequence];
                currentQuoteFromSequence++;
            }
        }
        else
        {
            int randomQuoteIndex = new Random().Next(_doorRandomQuotes.Count);
            message = _doorRandomQuotes.ElementAt(randomQuoteIndex);

            // zabezpecenie, aby sa po sebe nevybrali nikdy dve rovnake random message:
            _doorRandomQuotes.Remove(message);
            if (lastChosenMessage != "")
            {
                _doorRandomQuotes.AddLast(lastChosenMessage);
            }

            lastChosenMessage = message;
        }

        TooltipManager.Show(
            message, 
            "The Door:", 
            0.7f, 
            0.2f, 
            () => TooltipManager.Hide(quoteFadingDuration),
            quoteHideAfterSeconds
            );
    }
}