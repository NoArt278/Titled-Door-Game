using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PlayerInput pInput;
    private InputAction pause;
    public int hammerCount, pickUpsCount;
    public bool spawnBomb;
    int currScore, roomScore, highScore, highRoom;
    private float roomStartTime;
    public bool isFinished, isFirstRun;
    public int currTrackIdx, nextTrackIdx, currRoom;
    private int companionIdx;
    [SerializeField] Material companionMaterial, pickupMaterial;
    private List<Stoppable> stoppables;
    public GameObject currTrack;
    public List<GameObject> tracksList;
    [SerializeField] Door door;
    [SerializeField] ResetButton resetButton;
    [SerializeField] Bomb bomb;
    [SerializeField] GameObject hammerParent, pickups, spawnPoint, player, startBtn, resetBtn;
    [SerializeField] TMP_Text hallwayText, roomLeftText, roomRightText;
    [SerializeField] GameObject pauseMenuObj;
    private void Awake()
    {
        pInput = new PlayerInput();
        highScore = PlayerPrefs.GetInt("Highscore", -1);
        highRoom = PlayerPrefs.GetInt("RoomHighscore", -1);
        isFirstRun = highScore == -1;
        stoppables = new List<Stoppable>();
        currScore = 0;
        hammerCount = 0;
        pickUpsCount = 0;
        spawnBomb = false;
        currTrackIdx = 0;
        nextTrackIdx = 0;
        roomScore = 10;
        currRoom = 1;
        companionIdx = -1;
        isFinished = false;
        for (int i=0; i<hammerParent.transform.childCount; i++)
        {
            stoppables.Add(hammerParent.transform.GetChild(i).GetComponent<Stoppable>());
        }
        SpawnTrack();
        if (isFirstRun)
        {
            hallwayText.fontSize = 17;
            hallwayText.text = "W,A,S,D to move\n\nEsc to pause";
            roomLeftText.text = "Press E to interact";
            roomRightText.text = "Get the ball to the circle";
        }
        else
        {
            hallwayText.text = string.Concat("Highscore\n", highScore, "\n\nHighest room reached\n", highRoom);
            roomLeftText.text = string.Concat("Room ", currRoom.ToString());
            roomRightText.text = string.Concat("Room worth :", roomScore.ToString(), "\nCurrent score :", currScore.ToString());
        }
    }

    private void OnEnable()
    {
        pause = pInput.Menu.Pause;
        pause.performed += PauseGame;
        pause.Enable();
    }

    private void OnDisable()
    {
        pause.performed -= PauseGame;
        pause.Disable();
    }

    private void PauseGame(InputAction.CallbackContext ctx)
    {
        pauseMenuObj.SetActive(!pauseMenuObj.activeSelf);
        PauseMenu.isPaused = pauseMenuObj.activeSelf;
        
        if (pauseMenuObj.activeSelf)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        } else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1;
        }
    }

    public void BackToStart()
    {
        PauseMenu.isPaused = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void SpawnTrack()
    {
        currTrack = Instantiate(tracksList[currTrackIdx]);
        Transform stopParent = currTrack.transform.GetChild(0);
        stoppables = stoppables.Where(stoppable => stoppable != null).ToList();
        for (int i = 0; i < stopParent.childCount; i++)
        {
            stoppables.Add(stopParent.transform.GetChild(i).GetComponent<Stoppable>());
        }
    }

    public void ResetTrack()
    {
        if (!isFinished)
        {
            Destroy(currTrack);
            for (int i = 0; i < hammerParent.transform.childCount; i++)
            {
                GameObject hammer = hammerParent.transform.GetChild(i).gameObject;
                hammer.GetComponent<HammerTrap>().StopObject();
            }
            SpawnTrack();
        }
    }

    public void RunStoppables()
    {
        foreach (var stoppable in stoppables)
        {
            if (stoppable != null)
            {
                stoppable.ContinueObject();
            }
        }
    }

    public void LoadNextLevel()
    {
        isFinished = false;
        currTrackIdx = nextTrackIdx;
        roomScore = Mathf.Min(90, hammerCount * 10) + (spawnBomb ? 10 : 0) + (currTrackIdx + 1) * (currTrackIdx >= 2 ? 30 : 10);
        hallwayText.text = "";
        currRoom++;
        if (isFirstRun)
        {
            roomLeftText.text = "";
            if (currRoom == 2)
            {
                roomRightText.text = "Your actions will have consequences";
            } else
            {
                roomRightText.text = "";
            }
            if (currRoom == 8)
            {
                Destroy(currTrack);
                startBtn.SetActive(false);
                resetBtn.SetActive(false);
                for (int i = 0; i < hammerParent.transform.childCount; i++)
                {
                    hammerParent.transform.GetChild(i).gameObject.SetActive(false);
                }
                for (int i = 0; i < pickups.transform.childCount; i++)
                {
                    if (i != companionIdx)
                    {
                        pickups.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
                bomb.SetTimer(5);
                bomb.transform.position = new Vector3(0.239999995f, 1.37f, 9.64999962f);
                bomb.gameObject.SetActive(true);
                return;
            }
        } else
        {
            if (currRoom == 51)
            {
                roomLeftText.text = "";
                roomRightText.text = "";
                Destroy(currTrack);
                startBtn.SetActive(false);
                resetBtn.SetActive(false);
                for (int i = 0; i < hammerParent.transform.childCount; i++)
                {
                    hammerParent.transform.GetChild(i).gameObject.SetActive(false);
                }
                for (int i = 0; i < pickups.transform.childCount; i++)
                {
                    if (i != companionIdx)
                    {
                        pickups.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
                bomb.SetTimer(5);
                bomb.transform.position = new Vector3(0.239999995f, 1.37f, 9.64999962f);
                bomb.gameObject.SetActive(true);
                return;
            }
            roomLeftText.text = string.Concat("Room ", currRoom.ToString());
            roomRightText.text = string.Concat("Room worth :", roomScore.ToString(), "\nCurrent score :", currScore.ToString());
        }

        // Instantiate new track
        ResetTrack();
        
        // Disable all traps and pickups
        for (int i=0; i<hammerParent.transform.childCount; i++)
        {
            hammerParent.transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < pickups.transform.childCount; i++)
        {
            if (i != companionIdx)
            {
                pickups.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        bomb.gameObject.SetActive(false);

        int hammerSpeed = Mathf.Min(20, (Mathf.Max(hammerCount, hammerParent.transform.childCount) - hammerParent.transform.childCount) * 2) + 2;
        hammerCount = Mathf.Min(hammerParent.transform.childCount, hammerCount);
        pickUpsCount %= pickups.transform.childCount;

        // Enable traps and pickups based on the counters
        for (int i = 0; i < hammerCount; i++)
        {
            hammerParent.transform.GetChild(i).GetComponent<HammerTrap>().speedOfPendulum = hammerSpeed;
            hammerParent.transform.GetChild(i).gameObject.SetActive(true);
        }
        for (int i = 0; i < pickUpsCount; i++)
        {
            if (i != companionIdx)
            {
                pickups.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        bomb.gameObject.SetActive(spawnBomb);

        spawnBomb = false;
        hammerCount = 0;
    }

    public void LevelFinished()
    {
        currScore += roomScore;
        nextTrackIdx++;
        nextTrackIdx %= tracksList.Count;
        door.OpenDoor();
        bomb.StopCountdown();
        // Save highscore and rooms survived
        if (currScore > highScore)
        {
            PlayerPrefs.SetInt("Highscore", currScore);
            highScore = currScore;
            PlayerPrefs.Save();
        }
        if (currRoom > highRoom)
        {
            PlayerPrefs.SetInt("RoomHighscore", currRoom);
            highRoom = currRoom;
            PlayerPrefs.Save();
        }
        if (Time.time - roomStartTime > 180)
        {
            spawnBomb = true;
        }
        isFinished = true;
        if (!isFirstRun)
        {
            roomRightText.text = string.Concat("Room worth :", roomScore.ToString(), "\nCurrent score :", currScore.ToString());
        }
    }

    public void SetStartTime()
    {
        roomStartTime = Time.time;
    }

    public void RestartGame()
    {
        currScore = 0;
        hammerCount = 0;
        pickUpsCount = 0;
        spawnBomb = false;
        isFinished = false;
        currTrackIdx = 0;
        nextTrackIdx = 0;
        roomScore = 0;
        door.CloseDoor();
        startBtn.SetActive(true);
        resetBtn.SetActive(true);

        if (isFirstRun)
        {
            hallwayText.fontSize = 13;
            isFirstRun = false;
            bomb.SetTimer(180);
            bomb.transform.position = new Vector3(3.68000007f, 1.37f, 2.19000006f);
        }

        Camera.main.GetComponent<PlayerCam>().isInteracting = false;

        if (companionIdx != -1)
        {
            pickups.transform.GetChild(companionIdx).GetComponent<Renderer>().sharedMaterial = pickupMaterial;
            pickups.transform.GetChild(companionIdx).GetComponent<Highlight>().RefreshMaterials();
            companionIdx = -1;
        }

        player.transform.position = spawnPoint.transform.position;

        LoadNextLevel();

        currRoom = 1;
        hallwayText.text = string.Concat("Highscore\n", highScore, "\n\nHighest room reached\n", highRoom);
        roomLeftText.text = string.Concat("Room ", currRoom.ToString());
        roomRightText.text = string.Concat("Room worth :", roomScore.ToString(), "\nCurrent score :", currScore.ToString());
    }

    public void SetCompanion(int idx)
    {
        if (companionIdx == -1)
        {
            companionIdx = idx;
            GameObject companion = pickups.transform.GetChild(companionIdx).gameObject;

            // Flip uv so texture is not upside down on one side
            Vector2[] uvs = companion.GetComponent<MeshFilter>().sharedMesh.uv;

            uvs[6] = new Vector2(0, 0);
            uvs[7] = new Vector2(1, 0);
            uvs[10] = new Vector2(0, 1);
            uvs[11] = new Vector2(1, 1);

            companion.GetComponent<MeshFilter>().sharedMesh.uv = uvs;
            companion.GetComponent<Renderer>().sharedMaterial = companionMaterial;
            companion.GetComponent<Highlight>().RefreshMaterials();
        } else if (idx != companionIdx)
        {
            pickups.transform.GetChild(companionIdx).gameObject.SetActive(false);
        }
    }
}
