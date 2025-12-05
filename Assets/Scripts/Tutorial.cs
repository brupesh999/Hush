using System;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    private GameObject tutTextObject;

    private TextMeshProUGUI tutText;

    private static readonly string[] actions =
    {
        "attack",
        "deflect",
        "increase strength",
        "heal",
        "increase radius",
        "shield",
    };

    static Dictionary<string, string> actionMap = new Dictionary<string, string>()
    {
        { "attack", "Press A to melee attack when near an enemy" },
        { "deflect", "Press Q to deflect a moving attack!" },
        { "heal", "Press W to heal" },
        { "shield", "Press E to shield" },
        { "increase radius", "Press D in combination with attack to do long range attacks!" },
        { "increase strength", "Press S while attacking to increase your strength!" },
    };

    static Dictionary<string, int> levelReqs = new Dictionary<string, int>()
    {
        { "attack", 1 },
        { "deflect", 1 },
        { "heal", 3 },
        { "shield", 7 },
        { "increase radius", 5 },
        { "increase strength", 2 },
    };

    private static bool[] activated = new bool[actions.Length];

    private int actionsIndex = 0;

    // public static Action<InputAction.CallbackContext> GenerateOnScript(string title)
    // {
    //     return (InputAction.CallbackContext context) =>
    //     {
    //         if (context.started && actions[actionsIndex] == title)
    //         {
    //             Debug.Log("If these walls could talk 2");
    //             actionsIndex++;
    //         }
    //         else
    //         {
    //             Debug.Log("If these walls could talk part 2");
    //         }
    //     };
    // }

    public void OnAction(InputAction.CallbackContext context)
    {
        if (context.started && actions[actionsIndex] == "attack")
        {
            actionsIndex++;
            Debug.Log("If these walls could talk");
            Debug.Log(actionsIndex);
        }
    }

    public void OnDeflect(InputAction.CallbackContext context)
    {
        if (context.started && actions[actionsIndex] == "deflect")
        {
            actionsIndex++;
        }
    }

    public void OnHeal(InputAction.CallbackContext context)
    {
        if (context.started && actions[actionsIndex] == "heal")
        {
            actionsIndex++;
        }
    }

    public void OnShield(InputAction.CallbackContext context)
    {
        if (context.started && actions[actionsIndex] == "shield")
        {
            actionsIndex++;
        }
    }

    public void OnIncRad(InputAction.CallbackContext context)
    {
        if (context.started && actions[actionsIndex] == "increase radius")
        {
            actionsIndex++;
        }
    }

    public void OnStr(InputAction.CallbackContext context)
    {
        if (context.started && actions[actionsIndex] == "increase strength")
        {
            actionsIndex++;
        }
    }

    // public Action<InputAction.CallbackContext> OnDeflect = GenerateOnScript("deflect");

    // public Action<InputAction.CallbackContext> OnHeal = GenerateOnScript("heal");

    // public Action<InputAction.CallbackContext> OnShield = GenerateOnScript("heal");
    // public Action<InputAction.CallbackContext> OnIncRad = GenerateOnScript("increase radius");

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // playerInput = GetComponent<PlayerInput>();
        tutText = tutTextObject.GetComponent<TextMeshProUGUI>();
        // playerInput.actions["Deflect"].performed += OnDeflect;

        // // You can also subscribe a lambda inline
        // playerInput.actions["Heal"].performed += OnHeal;
        // playerInput.actions["Shield"].performed += OnShield;
        // playerInput.actions["LongRange"].performed += OnIncRad;
    }

    // Update is called once per frame
    void Update()
    {
        if (actionsIndex < actions.Length)
        {
            tutText.text = actionMap[actions[actionsIndex]];
            tutText.gameObject.SetActive(LevelManager.level >= levelReqs[actions[actionsIndex]]);
        }
    }
}
