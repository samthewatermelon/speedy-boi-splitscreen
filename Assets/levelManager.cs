using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelManager : MonoBehaviour
{
    public string levelObjective1;
    public int levelTimerObjective1;

    public string levelObjective2;
    public GameObject levelObjective2Object;

    public string currentObjective;
    public bool levelStarted = false;
    private bool levelStartedRunOnce = false;

    private void FixedUpdate()
    {
        if (levelStartedRunOnce)
            return;

        if (levelStarted && !levelStartedRunOnce)
        {
            levelStartedRunOnce = true;
            StartCoroutine(ObjectiveMessage(levelObjective1, levelTimerObjective1, levelObjective2));
        }
    }

    private IEnumerator ObjectiveMessage(string objective1, int wait, string objective2)
    {
        currentObjective = objective1;
        pushObjectiveToPlayers(objective1);

        yield return new WaitForSeconds(wait); // wait before next objective begins

        if (objective2 == "Race!")
        {
            currentObjective = objective2;
            pushObjectiveToPlayers(objective2);
            levelObjective2Object.SetActive(true);
        }

    }

    private void pushObjectiveToPlayers(string objective)
    {
        playerFunctions[] pfuncs = FindObjectsOfType<playerFunctions>();

        foreach (playerFunctions p in pfuncs)
            p.uiMessage.text = objective;
    }
}
