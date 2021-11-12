using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    public GameObject PauseInterfaceReference;
    public GameObject DeathInterfaceReference;

    GameObject _pause;

    bool isPause;
    public void Play()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void Update()
    {
        if (PauseInterfaceReference != null)
        {
            if (Input.GetKeyDown(KeyCode.Escape)) TogglePause();
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void TogglePause()
    {
        isPause = !isPause;
        if (isPause)
        {
            Time.timeScale = 0;
            _pause = Instantiate(PauseInterfaceReference, gameObject.transform);
        } else
        {
            Time.timeScale = 1;
            Destroy(_pause);
        }
    }

    public void OnDeath()
    {
        Instantiate(DeathInterfaceReference, gameObject.transform);
    }
}
