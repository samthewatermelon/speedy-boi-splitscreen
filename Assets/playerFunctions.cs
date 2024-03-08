using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerFunctions : MonoBehaviour
{
    public TMPro.TMP_Text uiMessage;

    public Vector3 worldScale;

    private levelManager lm;

    private void Start()
    {
        transform.position = GameObject.Find("offlineSpawn").transform.position;
        transform.rotation = GameObject.Find("offlineSpawn").transform.rotation;

        worldScale = transform.lossyScale;

        lm = GameObject.Find("LevelManager").GetComponent<levelManager>();
        lm.transform.GetChild(0).gameObject.SetActive(false); /// turns the starting camera and ui off
        lm.levelStarted = true;
        uiMessage.text = lm.currentObjective;

        //StartCoroutine(
        //    ObjectiveMessage(lm.levelObjective1, lm.levelTimerObjective1, lm.levelObjective2));
    }

    //private IEnumerator ObjectiveMessage(string objective1, int wait, string objective2)
    //{
    //    uiMessage.text = objective1;
    //    yield return new WaitForSeconds(3f);
    //    //uiMessage.text = "";
    //
    //    yield return new WaitForSeconds(wait);
    //
    //    uiMessage.text = objective2;
    //    yield return new WaitForSeconds(3f);
    //    //uiMessage.text = "";
    //
    //    if (objective2 == "Race!")
    //        lm.levelObjective2Object.SetActive(true);
    //}


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8) //death layer
        {
            StartCoroutine(respawnPlayer("You Died."));
        }
        if (collision.gameObject.layer == 11) //reset layer
        {
            StartCoroutine(loadScene(SceneManager.GetActiveScene().name, "You Died."));
        }
        if (collision.gameObject.layer == 7 && uiMessage.text != "You Died.") //finish layer
        {
            int.TryParse(SceneManager.GetActiveScene().name.Replace("level",""), out int lvl);
            StartCoroutine(loadScene("level" + (lvl + 1), "You Win!"));
        }
        if (collision.gameObject.layer == 3) //moving platform fix
        {
            transform.SetParent(collision.transform.root, true);
            transform.localScale = Vector3.one;
            transform.localScale = new Vector3(worldScale.x / transform.lossyScale.x, worldScale.y / transform.lossyScale.y, worldScale.z / transform.lossyScale.z);
            transform.rotation = Quaternion.Euler(0,0,0);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 3) //moving platform fix
        {
            transform.SetParent(null);
            transform.localScale = Vector3.one;
            transform.localScale = worldScale;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void FixedUpdate()
    {
        if (transform.position.y < -50f)
        {
            StartCoroutine(respawnPlayer("You Died."));
        }
    }

    public IEnumerator respawnPlayer(string message)
    {
        transform.Find("camera").Find("Main Camera").GetComponent<Camera>().cullingMask = LayerMask.GetMask("UI");
        transform.Find("camera").Find("Main Camera").GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
        freezePlayer(RigidbodyConstraints.FreezeAll);
        uiMessage.text = message;
        yield return new WaitForSeconds(3f);

        transform.position = GameObject.Find("offlineSpawn").transform.position;
        transform.rotation = GameObject.Find("offlineSpawn").transform.rotation;
        uiMessage.text = lm.currentObjective;
        freezePlayer(RigidbodyConstraints.FreezeRotation);
        transform.Find("camera").Find("Main Camera").GetComponent<Camera>().cullingMask = -1;
        transform.Find("camera").Find("Main Camera").GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
    }

    public IEnumerator loadScene(string sceneName, string message)
    {        
        uiMessage.text = message;
        //GetComponent<timer>().complete = true;
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(sceneName);
        //if (isServer)
        //    NetworkManager.singleton.ServerChangeScene(sceneName);
    }

    public void freezePlayer(RigidbodyConstraints status)
    {
        GetComponent<Rigidbody>().constraints = status;
    }
}
