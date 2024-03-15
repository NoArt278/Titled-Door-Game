using UnityEngine;
using UnityEngine.SceneManagement;

public class Begin : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = true;
    }

    private void Update()
    {
        Camera.main.transform.position = Camera.main.transform.position + Camera.main.transform.forward * Time.deltaTime;
    }

    public void BeginGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
