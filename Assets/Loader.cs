using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{
    public bool LoadOnStart = true;
    public const int ScenesCount = 6;
    private int _loaded = 0;
    public Image Fill;

    private AsyncOperation _async;
    private float _startTime;
    private LoadSceneParameters _parameters = new LoadSceneParameters(LoadSceneMode.Additive);
    public bool Launched { get; set; }

    void Start()
    {
        _startTime = Time.time;
        if (LoadOnStart)
        {
            Launched = true;
            Load(_loaded + 1);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Time.time < _startTime + 2)
            return;

        if (Input.anyKeyDown && !Launched)
        {
            Launched = true;
            Load(_loaded + 1);
        }

        if (_async != null)
            Fill.fillAmount = _async.progress * (_loaded + 1) / ScenesCount;
    }


    private void Load(int load)
    {
        SceneManager.LoadSceneAsync(1, _parameters).completed += AsyncOncompleted;
    }

    private void AsyncOncompleted(AsyncOperation async)
    {
        if (async != null)
        {
            _loaded++;
            async.completed -= AsyncOncompleted;
        }

        if (_loaded < ScenesCount)
        {
            _async = SceneManager.LoadSceneAsync(_loaded + 1, _parameters);
            _async.completed += AsyncOncompleted;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
