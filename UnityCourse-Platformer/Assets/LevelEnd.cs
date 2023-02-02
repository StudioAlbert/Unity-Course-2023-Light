using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{

    [SerializeField] private SpriteRenderer _up;
    [SerializeField] private SpriteRenderer _down;
    [SerializeField] private GameObject _endPanel;
    [SerializeField] private string _nextSceneName;

    [SerializeField] private UnityEvent _endLevel;

    // Start is called before the first frame update
    void Start()
    {
        _down.enabled = true;
        _up.enabled = false;
        _endPanel.SetActive(false);
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            StartCoroutine("CO_EndLevel");
        }
    }

    IEnumerator CO_EndLevel()
    {
        _down.enabled = false;
        _up.enabled = true;

        _endLevel.Invoke();
        
        yield return new WaitForSeconds(1);
        
        _endPanel.SetActive(true);

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(_nextSceneName, LoadSceneMode.Single);

    }

}
