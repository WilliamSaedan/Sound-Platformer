using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public int subdivMult = 8;
    public TokenInstance[] tokens;

    private static bool paused = false;

    private PlayerController player;
    private Health playerHealth;
    private CameraController cam;
    private OSCSendReceive osc;

    private int numTokens = 0;
    private int numCollected = 0;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerHealth = player.gameObject.GetComponent<Health>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        osc = GameObject.FindGameObjectWithTag("OSC").GetComponent<OSCSendReceive>();
        tokens = UnityEngine.Object.FindObjectsOfType<TokenInstance>();
        numTokens = tokens.Length;
    }

    private void Start()
    {
        InitiateStartSequence();
    }

    private void Update()
    {
        if (playerHealth.currentHealth <= 0)
        {
            ResetGame();
        }
    }

    public void TogglePauseGame()
    {
        paused = !paused;
        Time.timeScale = (paused) ? 0f : 1f;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    public void EndGame()
    {
        PauseGame();
        cam.followTarget = false;
        SceneManager.LoadScene(1); // Load End Screen
    }

    public void ResetGame()
    {
        numCollected = 0;
        foreach (TokenInstance tok in tokens)
        {
            tok.gameObject.SetActive(true);
            tok.collected = false;
        }

        osc.GetComponent<OSCSendReceive>().PlaySoundOSC("/Game_Started " + 0 + " " + (player.bpm) + " " + (4 + (((float)numCollected / (float)numTokens) * subdivMult)));
        player.Respawn();
        InitiateStartSequence();
    }

    public void InitiateStartSequence()
    {
        StartCoroutine(StartSequence(3));
    }

    public void CollectToken(TokenInstance token)
    {
        if (player.transform.position.y > 0f)
        {
            osc.GetComponent<OSCSendReceive>().PlaySoundOSC("/Collected_Token " + player.transform.position.y);
        }
        else
        {
            osc.GetComponent<OSCSendReceive>().PlaySoundOSC("/Collected_Token " + (1 / Mathf.Abs(player.transform.position.y)));
        }
        token.gameObject.SetActive(false);
        numCollected++;
        osc.GetComponent<OSCSendReceive>().PlaySoundOSC("/Game_Started " + 1 + " " + (player.bpm) + " " + (4 + (((float)numCollected / (float)numTokens) * subdivMult)));

    }

    IEnumerator StartSequence(int timeDelay)
    {
        PauseGame();
        player.gameObject.GetComponentInChildren<Animator>().SetBool("IsWalking", false);
        for (int i = 0; i < timeDelay; i++)
        {
            Debug.Log(i + 1); // Update Visuals
            yield return new WaitForSecondsRealtime(1);
        }
        osc.GetComponent<OSCSendReceive>().PlaySoundOSC("/Game_Started " + 1 + " " + (player.bpm) + " " + (4 + (((float)numCollected / (float)numTokens) * subdivMult)));
        ResumeGame();
    }
}
