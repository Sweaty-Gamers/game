using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class GameMasterScript : MonoBehaviour
  
{
    // ------------ Game Configuration -------------
    public float secondsBeforeNextRound = 5;

    // ------------ Round Spawn Variables -------------
    public int numOfDragons = 0;
    public int currentRound = 1;
    public bool roundStarted = false;
    public int enemies = 10;
    public GameObject minotaur;
    public GameObject dragon;
    public GameObject ranged;
    public int xPos;
    public int zPos;
    // ------------ Current State ------------------
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
        //ApplyModifier(new PlayerFovModifier());
        //ApplyModifier(new TreeGrowModifier());
        // ApplyModifier(new SunColorModifier());
    }

    private string GetModifiersString() {
        return string.Join('\n', activeModifiersNames);
    }

    // Update is called once per frame
    void Update()
    {
        roundText.text = currentRound.ToString();
        modifiersText.text = GetModifiersString();
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

    int GetActiveEnemies()
    {   
        return GameObject.FindGameObjectsWithTag("Enemy_Melee").Length + GameObject.FindGameObjectsWithTag("Enemy_Ranged").Length;
    }

    // Check if the current round shound end.
    void CheckRoundEnd()
    {
        // Round must be started to end.
        if (!roundStarted) return;

        // Get number of active enemies.
        int activeEnemies = GetActiveEnemies();
 
        // When no enemies left, end the round.
        if (activeEnemies== 0)
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
        StartCoroutine(EnemyDrop());
        Debug.Log(GetActiveEnemies());
        // Spawn new enemies.
    }

    // Called when the current round ends, wrap things up.
    void EndRound()
    {
        roundStarted = false;
        currentRound += 1;
    }
      IEnumerator EnemyDrop()
    {
        var pairs = new (int, int)[4];
        pairs[0] = (256, 277);
        pairs[1] = (268, 250);
        pairs[2] = (249, 230);
        pairs[3] = (220, 230);
        if(currentRound<=10){
             for(int i =0; i<enemies; i++){
            int randomNumber = Random.Range(0, 4);
            xPos = pairs[randomNumber].Item1;
            zPos = pairs[randomNumber].Item2;
            Instantiate(minotaur, new Vector3(xPos, 0, zPos), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
        }

        }
        else if(currentRound==11){
            for(int i =0; i<20; i++){
            int randomNumber = Random.Range(0, 4);
            xPos = pairs[randomNumber].Item1;
            zPos = pairs[randomNumber].Item2;
            Instantiate(ranged, new Vector3(xPos, 0, zPos), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
        }
        }
        else if(currentRound<=20){
            for(int i =0; i<enemies/2; i++){
            int randomNumber = Random.Range(0, 4);
            xPos = pairs[randomNumber].Item1;
            zPos = pairs[randomNumber].Item2;
            Instantiate(minotaur, new Vector3(xPos, 0, zPos), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
        }
            for(int i =0; i<enemies/2; i++){
            int randomNumber = Random.Range(0, 4);
            xPos = pairs[randomNumber].Item1;
            zPos = pairs[randomNumber].Item2;
            Instantiate(ranged, new Vector3(xPos, 0, zPos), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
        }
        }
        else if (currentRound==21){
            for(int i =0; i<5; i++){
            int randomNumber = Random.Range(0, 4);
            xPos = pairs[randomNumber].Item1;
            zPos = pairs[randomNumber].Item2;
            Instantiate(dragon, new Vector3(xPos, 0, zPos), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
            }
            numOfDragons++;
        }
        else if (currentRound<50){
            for(int i =0; i<numOfDragons; i++){
            int randomNumber = Random.Range(0, 4);
            xPos = pairs[randomNumber].Item1;
            zPos = pairs[randomNumber].Item2;
            Instantiate(dragon, new Vector3(xPos, 0, zPos), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
            }
            for(int i =0; i<enemies/2; i++){
            int randomNumber = Random.Range(0, 4);
            xPos = pairs[randomNumber].Item1;
            zPos = pairs[randomNumber].Item2;
            Instantiate(ranged, new Vector3(xPos, 0, zPos), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
        }
            for(int i =0; i<enemies/2; i++){
            int randomNumber = Random.Range(0, 4);
            xPos = pairs[randomNumber].Item1;
            zPos = pairs[randomNumber].Item2;
            Instantiate(minotaur, new Vector3(xPos, 0, zPos), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
            }
            numOfDragons++;
        }
        else if (currentRound==50){
            //Spawn Boss
        }
        else{
             for(int i =0; i<numOfDragons; i++){
            int randomNumber = Random.Range(0, 4);
            xPos = pairs[randomNumber].Item1;
            zPos = pairs[randomNumber].Item2;
            Instantiate(dragon, new Vector3(xPos, 0, zPos), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
            }
            for(int i =0; i<enemies/2; i++){
            int randomNumber = Random.Range(0, 4);
            xPos = pairs[randomNumber].Item1;
            zPos = pairs[randomNumber].Item2;
            Instantiate(ranged, new Vector3(xPos, 0, zPos), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
        }
            for(int i =0; i<enemies/2; i++){
            int randomNumber = Random.Range(0, 4);
            xPos = pairs[randomNumber].Item1;
            zPos = pairs[randomNumber].Item2;
            Instantiate(minotaur, new Vector3(xPos, 0, zPos), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
            }
            numOfDragons++;
        }

        enemies += 2;
    }
}
