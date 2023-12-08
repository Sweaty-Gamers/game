using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;
using System.Collections;

public class CustomMasterScript : MonoBehaviour

{
    // ------------ Game Configuration -------------
    public float secondsBeforeNextRound = 5;

    // ------------ Game Objects ----------------------
    public GameObject minotaur;
    public GameObject dragon;
    public GameObject ranged;
    public GameObject boss;
    public GameObject player;
    // ------------ Round Spawn Variables -------------
    public int numOfDragons = 3;
    public int currentRound = 1;
    public bool roundStarted = false;
    public int enemies = 1;
    public float minotaurHealth;
    public float minotaurSpeed;
    public float dragonHealth;
    public float rangedHealth;
    public int xPos;
    public int zPos;
    public int current;
    public float spawnDelay;
    private bool needed;
    private int currDragons;
    private bool spawned = false;
    // ------------ Current State ------------------
    private GameObject roundUi;
    private TextMeshProUGUI roundText;
    private GameObject modifiersUi;
    private TextMeshProUGUI modifiersText;
    private GameObject newModifierUI;
    private TextMeshProUGUI newModifierText;
    private int collectedEasterEggs = 0;

    private readonly List<string> activeModifiersNames = new();
    List<string> activatedModifiers = new();
    List<Modifier> availableModifiers = new();
    private List<Func<Modifier>> enabledModifiers = new();

    public void GotEasterEgg()
    {
        collectedEasterEggs += 1;
        ApplyModifier(new SunColorModifier());
        ApplyModifier(new PlayerFovModifier());
    }


    // Start is called before the first frame update
    void Start()
    {
        dragonHealth = 700f;
        minotaurHealth = 25f;
        minotaurSpeed = 2f;
        rangedHealth = 15f;
        roundUi = GameObject.Find("Round");
        modifiersUi = GameObject.Find("Modifiers");
        newModifierUI = GameObject.Find("NewModifierText");

        if (newModifierUI != null)
        {
            Debug.Log("ahhhhhhhhhhhhh");
        }
        else
        {
            Debug.Log("wack af");
        }

        roundText = roundUi.GetComponent<TextMeshProUGUI>();
        modifiersText = modifiersUi.GetComponent<TextMeshProUGUI>();
        newModifierText = newModifierUI.GetComponent<TextMeshProUGUI>();

        /*
        enabledModifiers.Add(() => new EnemyGrowth());
        enabledModifiers.Add(() => new HealingModifier(5, 10, false));
        enabledModifiers.Add(() => new PlayerFovModifier());
        enabledModifiers.Add(() => new PlayerGrowModifier());
        //enabledModifiers.Add(() => new SunColorModifier());
        enabledModifiers.Add(() => new TreeGrowModifier());
        enabledModifiers.Add(() => new IncreaseHealthModifier(20f));
        */
        availableModifiers.Add(new EnemyGrowth());
        availableModifiers.Add(new PlayerGrowModifier());
        availableModifiers.Add(new IncreaseHealthModifier(20f));

        StartNextRound();
        roundText.text = currentRound.ToString();

        // Test modifiers:
        //ApplyModifier(new HealingModifier(5, 10.5f));
        //ApplyModifier(new PlayerFovModifier());
        //ApplyModifier(new TreeGrowModifier());
        // ApplyModifier(new SunColorModifier());
    }

    private string GetModifiersString()
    {
        return string.Join('\n', activeModifiersNames);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(activeModifiersNames);
        //Debug.Log("Active Enemies: " + GetActiveEnemies());
        if (needed)
        {
            StartCoroutine(EnemyDrop());
        }

        modifiersText.text = GetModifiersString();
        CheckRoundEnd();
    }
   private IEnumerator InternalUpdateActiveList(Modifier modifier)
    {
        if (modifier.permanent) yield return null;

        activatedModifiers.Add(modifier.name);
        availableModifiers.Remove(modifier);

        yield return new WaitForSeconds(modifier.sec);

        activatedModifiers.Remove(modifier.name);
        availableModifiers.Add(modifier);

        yield return null;
    }
    private IEnumerator InternalApplyModifier(Modifier modifier)
    {
        StartCoroutine(InternalUpdateActiveList(modifier));
        yield return modifier.apply(this);
    }
       private IEnumerator ApplyNewModifier(string name)
    {
        newModifierText.text = string.Format("New Modifier:\n{0}", name);
        yield return new WaitForSeconds(2);
        newModifierText.text = "";
        yield return null;
    }
    void ApplyModifier(Modifier modifier)
    {
        
        StartCoroutine(ApplyNewModifier(modifier.name));
        StartCoroutine(InternalApplyModifier(modifier));
    }

    int GetActiveEnemies()
    {
        return GameObject.FindGameObjectsWithTag("Enemy_Melee").Length + GameObject.FindGameObjectsWithTag("Enemy_Ranged").Length + GameObject.FindGameObjectsWithTag("Enemy_Flying").Length;
    }
    int GetActiveDragons()
    {
        return GameObject.FindGameObjectsWithTag("Enemy_Flying").Length;
    }
    int GetActiveBoss()
    {
        Debug.Log("get active boss: " + GameObject.FindGameObjectsWithTag("Enemy_Boss").Length);
        return GameObject.FindGameObjectsWithTag("Enemy_Boss").Length;
    }

    // Check if the current round shound end.
    void CheckRoundEnd()
    {
        // Round must be started to end.
        if (!roundStarted) return;

        // When no enemies left, end the round.
            if (currentRound == 0)
            {
                EndRound();
                StartCoroutine(WaitAndStartNextRound(secondsBeforeNextRound));
            }
            else if (currentRound == 1  && GetActiveEnemies()==0 && current == 10) 
            {
                EndRound();
                StartCoroutine(WaitAndStartNextRound(secondsBeforeNextRound));
            }
            else if (currentRound==2 && GetActiveEnemies()==0 && current == 10)
        {
            EndRound();
            StartCoroutine(WaitAndStartNextRound(secondsBeforeNextRound));

        }
            else if (currentRound==3 && GetActiveEnemies()==0 && current == 3)
        {
            EndRound();
            StartCoroutine(WaitAndStartNextRound(secondsBeforeNextRound));
        }
            else if(currentRound==4 && GetActiveBoss() == 0 && spawned)
        {
            EndRound();
            StartCoroutine(WaitAndStartNextRound(secondsBeforeNextRound));
        }
            else if(GetActiveEnemies()==0 && current==20 && currDragons == 1)
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


    void AddRandomModifier()
    {
      int modIdx = UnityEngine.Random.Range(0, availableModifiers.Count);
        Modifier mod = availableModifiers[modIdx];

        ApplyModifier(mod);
    }

    // Start the next round and spawn enemies.
    void StartNextRound()
    {
        if (currentRound == 4)
        {
            player.transform.position = new Vector3(405.9f, 0.7f, 62.8f);
        }
        else if (currentRound == 5)
        {
            player.transform.position = new Vector3(243.3f, 0.7f, 202.8f);
        }
        currDragons = 0;
        spawnDelay = 2f;
        current = 0;
        AddRandomModifier();
        roundStarted = true;
    StartCoroutine(EnemyDrop());
        
        needed = false;
        // Spawn new enemies.
    }

    // Called when the current round ends, wrap things up.
    void EndRound()
    {
        minotaurHealth += 10f;
        minotaurSpeed += .125f;
        if (currentRound > 10)
            rangedHealth += 15f;
        if (currentRound > 20)
            dragonHealth += 75f;
        enemies += 2;
        roundStarted = false;
        currentRound += 1;
        roundText.text = currentRound.ToString();
    }
    IEnumerator EnemyDrop()
    {
        Minotaur.newHealth = minotaurHealth;
        Minotaur.newSpeed = minotaurSpeed;
        PBRScript.newHealth = rangedHealth;
        DragonScript.newHealth = dragonHealth;
        // Debug.Log(Minotaur.newSpeed);
        var pairs = new (int, int)[4];
        pairs[0] = (256, 277);
        pairs[1] = (268, 250);
        pairs[2] = (249, 230);
        pairs[3] = (220, 230);
        if (currentRound ==1)
        {
            while (current<10)
            {
                current++;
                spawnDelay -= 1;
                spawnDelay = Mathf.Max(spawnDelay, 0.5f);
                int randomNumber = UnityEngine.Random.Range(0, 4);
                xPos = pairs[randomNumber].Item1;
                zPos = pairs[randomNumber].Item2;
                Instantiate(minotaur, new Vector3(xPos, 0, zPos), Quaternion.identity);
                yield return new WaitForSeconds(0.5f);
            }
        }
        else if (currentRound == 2)
        {
            while (current<10)
            {
                spawnDelay -= 1;
                spawnDelay = Mathf.Max(spawnDelay, 0.5f);
                current++;
                int randomNumber = UnityEngine.Random.Range(0, 4);
                xPos = pairs[randomNumber].Item1;
                zPos = pairs[randomNumber].Item2;
                Instantiate(ranged, new Vector3(xPos, 0, zPos), Quaternion.identity);
                yield return new WaitForSeconds(0.5f);
            }
        }
        else if (currentRound == 3)
        {
            while (current < 3)
            {
                current++;
                Instantiate(dragon, new Vector3(300f, 0, 390f), Quaternion.identity);
                yield return new WaitForSeconds(10f);
            }
        }
        else if (currentRound == 4)
        {
            yield return new WaitForSeconds(5f);
            Instantiate(boss, new Vector3(305f, 0, 153f), Quaternion.identity);
            spawned = true;
        }
        else
        {
            while (current < 20)
            {
                current++;
                int seed = UnityEngine.Random.Range(0, 2);
                spawnDelay -= 1;
                spawnDelay = Mathf.Max(spawnDelay, 1f);
                int randomNumber = UnityEngine.Random.Range(0, 4);
                xPos = pairs[randomNumber].Item1;
                zPos = pairs[randomNumber].Item2;
                if (seed == 0)
                {
                    Instantiate(minotaur, new Vector3(xPos, 0, zPos), Quaternion.identity);
                    yield return new WaitForSeconds(0.7f);
                }
                else
                {
                    Instantiate(ranged, new Vector3(xPos, 0, zPos), Quaternion.identity);
                    yield return new WaitForSeconds(0.7f);
                }
            }
            while (currDragons<1)
            {
                currDragons++;
                Instantiate(dragon, new Vector3(300f, 0, 390f), Quaternion.identity);
                yield return new WaitForSeconds(50f);
            }
        }
        
        }


}
