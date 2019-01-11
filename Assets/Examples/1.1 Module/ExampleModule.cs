using UnityEngine;
using KModkit;
using System;
using System.Collections.Generic;
using System.Linq;

public class ExampleModule : MonoBehaviour
{
    public KMSelectable[] buttons;
    public KMBombInfo BombInfo;

    int correctIndex;
    bool isActivated = false;

    void Start()
    {
        Init();

        GetComponent<KMBombModule>().OnActivate += ActivateModule;
    }

    void Init()
    {
        correctIndex = UnityEngine.Random.Range(0, 4);

        for(int i = 0; i < buttons.Length; i++)
        {
            string label = i == correctIndex ? "O" : "X";

            TextMesh buttonText = buttons[i].GetComponentInChildren<TextMesh>();
            buttonText.text = label;
            int j = i;
            buttons[i].OnInteract += delegate () { OnPress(j == correctIndex); return false; };
        }
    }

    void ActivateModule()
    {
        isActivated = true;

        string mostPresentPort = BombInfo.GetPorts().GroupBy(p => p).OrderByDescending(g => g.Count()).FirstOrDefault().Key ?? null;

        Debug.Log("Most Present" + mostPresentPort);
    }

    void OnPress(bool correctButton)
    {
        GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
        GetComponent<KMSelectable>().AddInteractionPunch();

        if (!isActivated)
        {
            Debug.Log("Pressed button before module has been activated!");
            GetComponent<KMBombModule>().HandleStrike();
        }
        else
        {
            Debug.Log("Pressed " + correctButton + " button");
            if (correctButton)
            {
                GetComponent<KMBombModule>().HandlePass();
            }
            else
            {
                GetComponent<KMBombModule>().HandleStrike();
            }
        }
    }
}
