using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;
using System.Collections;
using Unity.Mathematics;

public class GameMasterScript : MonoBehaviour

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
    private GameObject shootingMessageUI;
    private GameObject eggUi;
    private TextMeshProUGUI shootingText;
    private int collectedEasterEggs = 0;
    private bool added = false;

    private readonly List<string> activeModifiersNames = new();
    List<string> activatedModifiers = new();
    List<Modifier> availableModifiers = new();
    private List<Func<Modifier>> enabledModifiers = new();

    public void GotEasterEgg()
    {
        collectedEasterEggs += 1;
        if(collectedEasterEggs==1){
              availableModifiers.Add(new SunColorModifier());
            StartCoroutine(EasterEggMessage());
        }
        else if(collectedEasterEggs==2){
            availableModifiers.Add(new PlayerFovModifier());
            StartCoroutine(EasterEggMessage());
        }
        eggUi = GameObject.Find("EggText");
        eggUi.GetComponent<TextMeshProUGUI>().SetText(collectedEasterEggs + "/8 easter eggs found ;)");
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
        shootingMessageUI = GameObject.Find("ShootingText");

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
        shootingText =  shootingMessageUI.GetComponent<TextMeshProUGUI>();

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
        availableModifiers.Add(new PlayerShrinkModifier());
        availableModifiers.Add(new IncreaseHealthModifier(20f));
        StartCoroutine(ApplyShootingMessage());
        if(currentRound==0){
             EndRound();
        }
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
        return string.Join('\n', activatedModifiers);
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
        //Debug.Log(MouseLook.sensitivityX);
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
    private IEnumerator ApplyShootingMessage()
    {
        shootingText.text ="You can shoot with left click\nYou can scope with Right click";
        yield return new WaitForSeconds(2);
        shootingText.text = "";
        yield return null;
    }
    private IEnumerator EasterEggMessage()
    {
        if(collectedEasterEggs==1){
            newModifierText.text = string.Format("New Modifier added: Sun Color");
        }
        else if(collectedEasterEggs==2){
            newModifierText.text = string.Format("New Modifier added: FOV");
        }
        
        yield return new WaitForSeconds(2);
        newModifierText.text = "";
        yield return null;
    }
    

    public void ApplyModifier(Modifier modifier)
    {
        StartCoroutine(ApplyNewModifier(modifier.name));
        StartCoroutine(InternalApplyModifier(modifier));
    }

    int GetActiveEnemies()
    {
        return GameObject.FindGameObjectsWithTag("Enemy_Melee").Length + GameObject.FindGameObjectsWithTag("Enemy_Ranged").Length;
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
        if(currentRound<=10 && GetActiveEnemies()==0 && current == enemies){
            EndRound();
            StartCoroutine(WaitAndStartNextRound(secondsBeforeNextRound));
        }
        else if(currentRound==11 && GetActiveEnemies()==0 && current ==20){
            EndRound();
            StartCoroutine(WaitAndStartNextRound(secondsBeforeNextRound));
        }
        else if (currentRound<=20 && GetActiveEnemies()==0){
            EndRound();
            StartCoroutine(WaitAndStartNextRound(secondsBeforeNextRound));
        }
        else if(currentRound==21 && GetActiveDragons()==0 && currDragons==3){
            EndRound();
            StartCoroutine(WaitAndStartNextRound(secondsBeforeNextRound));
        }
        else if (currentRound<30 && GetActiveDragons()==0 && currDragons==3 && GetActiveEnemies()==0 && current==enemies){
            EndRound();
            StartCoroutine(WaitAndStartNextRound(secondsBeforeNextRound));
        }
        else if(currentRound==30 && GetActiveBoss()==0 && spawned){
            EndRound();
            StartCoroutine(WaitAndStartNextRound(secondsBeforeNextRound));
        }
        else{
            if(current==enemies && GetActiveEnemies()==0 && currDragons==5 && GetActiveDragons()==0){
            EndRound();
            StartCoroutine(WaitAndStartNextRound(secondsBeforeNextRound));
            }
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
        if (currentRound == 30)
        {
            player.transform.position = new Vector3(405.9f, 0.7f, 62.8f);
        }
        else if (currentRound == 31)
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
        // ApplyModifier(enabledModifiers[3]());
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
        if (currentRound <= 10)
        {
            while (GetActiveEnemies() < 25 && current < enemies)
            {
                spawnDelay -= 1;
                spawnDelay = Mathf.Max(spawnDelay, 0.5f);
                current++;
                int randomNumber = UnityEngine.Random.Range(0, 4);
                xPos = pairs[randomNumber].Item1;
                zPos = pairs[randomNumber].Item2;
                Instantiate(minotaur, new Vector3(xPos, 0, zPos), Quaternion.identity);
                yield return new WaitForSeconds(spawnDelay);
            }
            if (current == enemies)
            {
                needed = false;
            }
            else
            {
                needed = true;
            }
        }
        else if (currentRound == 11)
        {
            while (GetActiveEnemies() < 10 && current < 20)
            {
                spawnDelay -= 1;
                spawnDelay = Mathf.Max(spawnDelay, 0.5f);
                current++;
                int randomNumber = UnityEngine.Random.Range(0, 4);
                xPos = pairs[randomNumber].Item1;
                zPos = pairs[randomNumber].Item2;
                Instantiate(ranged, new Vector3(xPos, 0, zPos), Quaternion.identity);
                yield return new WaitForSeconds(spawnDelay);
            }
            if (current == 20)
            {
                needed = false;
            }
            else
            {
                needed = true;
            }
        }
        else if (currentRound <= 20)
        {
            while (GetActiveEnemies() < 20 && current < enemies)
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
                    yield return new WaitForSeconds(spawnDelay);
                }
                else
                {
                    Instantiate(ranged, new Vector3(xPos, 0, zPos), Quaternion.identity);
                    yield return new WaitForSeconds(spawnDelay);
                }

            }
            if (current == enemies)
            {
                needed = false;
            }
            else
            {
                needed = true;
            }
        }
        else if (currentRound == 21)
        {
            while (GetActiveDragons() < 1 && currDragons < 3)
            {
                currDragons++;
                Instantiate(dragon, new Vector3(300f, 0, 390f), Quaternion.identity);
                yield return new WaitForSeconds(5f);
            }
            if (currDragons == 3)
            {
                needed = false;
            }
            else
            {
                needed = true;
            }
        }
        else if (currentRound < 30)
        {
            while (GetActiveEnemies() < 25 && current < enemies)
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
                    yield return new WaitForSeconds(spawnDelay);
                }
                else
                {
                    Instantiate(ranged, new Vector3(xPos, 0, zPos), Quaternion.identity);
                    yield return new WaitForSeconds(spawnDelay);
                }
            }
            while (GetActiveDragons() < 1 && currDragons < 3)
            {
                currDragons++;
                Instantiate(dragon, new Vector3(300f, 0, 390f), Quaternion.identity);
                yield return new WaitForSeconds(5f);
            }
            if (current == enemies && currDragons == 3)
            {
                needed = false;
            }
            else 
            {
                needed = true;
            }
        }
        else if (currentRound == 30)
        {
            if(GetActiveBoss()==0 && !spawned){
            needed = false;
            yield return new WaitForSeconds(5f);
            Instantiate(boss, new Vector3(305f, 0, 153f), Quaternion.identity);
            spawned = true;
            }

        }
        else
        {
            while (GetActiveEnemies() < 25 && current < enemies)
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
                    yield return new WaitForSeconds(spawnDelay);
                }
                else
                {
                    Instantiate(ranged, new Vector3(xPos, 0, zPos), Quaternion.identity);
                    yield return new WaitForSeconds(spawnDelay);
                }
            }
            while (GetActiveDragons() < 1 && currDragons < 5)
            {
                currDragons++;
                Instantiate(dragon, new Vector3(300f, 0, 390f), Quaternion.identity);
                yield return new WaitForSeconds(25f);
            }
            if (current == enemies && currDragons == 3)
            {
                needed = false;
            }
            else
            {
                needed = true;
            }
        }


    }
}
