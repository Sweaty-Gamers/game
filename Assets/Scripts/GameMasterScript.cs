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


    // Start is called before the first frame update
    void Start()
    {
        roundUi = GameObject.Find("Round");
        roundText = roundUi.GetComponent<TextMeshProUGUI>();

        StartNextRound();

        // Test modifiers:
        ApplyModifier(new EnemyGrowth(5, false));
    }

    // Update is called once per frame
    void Update()
    {
        roundText.text = currentRound.ToString();

        CheckRoundEnd();
    }

    void ApplyModifier(Modifier modifier) {
        StartCoroutine(modifier.apply(this));
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
