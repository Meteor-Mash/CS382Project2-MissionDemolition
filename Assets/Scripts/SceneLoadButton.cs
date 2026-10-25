using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoadButton : MonoBehaviour
{
    [Header("Inscribed")]
    public string thisScene;
    // Start is called before the first frame update
    void Start()
    {
        Button thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(LoadScene);
    }

    void LoadScene()
    {
        SceneManager.LoadScene(thisScene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
