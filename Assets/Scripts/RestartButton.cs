using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Button thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(RestartGame);
    }

    void RestartGame()
    {
        SceneManager.LoadScene("_Scene_0");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
