using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class GameMasterScript : MonoBehaviour
{
    // ------------ Game Configuration -------------
    public float secondsBeforeNextRound = 5;


    // ------------ Current State ------------------
    public int currentRound = 1;
    public bool roundStarted = false;


    private GameObject roundUi;
    private TextMeshProUGUI roundText;
    private GameObject modifiersUi;
    private TextMeshProUGUI modifiersText;

    private readonly List<string> activeModifiersNames = new();


    // Start is called before the first frame update
    void Start()
    {
        roundUi = GameObject.Find("Round");
        modifiersUi = GameObject.Find("Modifiers");
        roundText = roundUi.GetComponent<TextMeshProUGUI>();
        modifiersText = modifiersUi.GetComponent<TextMeshProUGUI>();

        StartNextRound();

        // Test modifiers:
        //ApplyModifier(new HealingModifier(5, 10.5f));
        ApplyModifier(new PlayerFovModifier());
    }

    private string GetModifiersString() {
        return string.Join('\n', activeModifiersNames);
    }

    // Update is called once per frame
    void Update()
    {
        roundText.text = currentRound.ToString();
        modifiersText.text = GetModifiersString();
        print(GetModifiersString());

        CheckRoundEnd();
    }

    private IEnumerator InternalApplyModifier(Modifier modifier) {
        activeModifiersNames.Add(modifier.name);
        yield return modifier.apply(this);
        activeModifiersNames.Remove(modifier.name);
    }

    void ApplyModifier(Modifier modifier) {
        StartCoroutine(InternalApplyModifier(modifier));
    }

    GameObject[] GetActiveEnemies()
    {
        return GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Check if the current round shound end.
    void CheckRoundEnd()
    {
        // Round must be started to end.
        if (!roundStarted) return;

        // Get number of active enemies.
        GameObject[] activeEnemies = GetActiveEnemies();

        // When no enemies left, end the round.
        if (activeEnemies.Length == 0)
        {
            EndRound();
            StartCoroutine(WaitAndStartNextRound(secondsBeforeNextRound));
        }
    }

    IEnumerator WaitAndStartNextRound(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        StartNextRound();
    }

    // Start the next round and spawn enemies.
    void StartNextRound()
    {
        roundStarted = true;

        // Spawn new enemies.
    }

    // Called when the current round ends, wrap things up.
    void EndRound()
    {
        roundStarted = false;
        currentRound += 1;
    }
}
