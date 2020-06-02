using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIHomePanel : MonoBehaviour
{
    public Button _StartGame;
    // Start is called before the first frame update
    void Start()
    {
        _StartGame.onClick.AddListener(delegate {

            SceneManager.LoadScene("Scene1");
     	});
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
