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
        StartCoroutine(EnemyDrop());
        StartNextRound();

        // Test modifiers:
        ApplyModifier(new PlayerGrowModifier());
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
    public GameObject enemy;
    public int xPos;
    public int zPos;
    public int enemyCount;

    IEnumerator EnemyDrop()
    {
        var pairs = new (int, int)[4];
        pairs[0] = (256, 277);
        pairs[1] = (268, 250);
        pairs[2] = (249, 230);
        pairs[3] = (220, 230);
        while (enemyCount < 20)
        {
            int randomNumber = Random.Range(0, 5);
            xPos = pairs[randomNumber].Item1;
            zPos = pairs[randomNumber].Item2;
            Instantiate(enemy, new Vector3(xPos, 0, zPos), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
            enemyCount += 1;
        }
    }
}
