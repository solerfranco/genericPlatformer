using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    public static GameMaster instance;
    public CinemachineVirtualCamera cinemachineVirtualCamera;
    public Vector2 currentCheckpoint;
    public Material white;

    private void Awake()
    {
        //Singleton pattern to ensure we have only one instance of this running at the same time.
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else Destroy(gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
    }

    public IEnumerator Flash(SpriteRenderer sprite, Material original)
    {
        sprite.material = white;
        yield return new WaitForSeconds(0.1f);
        sprite.material = original;
        yield return new WaitForSeconds(0.1f);
        sprite.material = white;
        yield return new WaitForSeconds(0.1f);
        sprite.material = original;
    }

    public IEnumerator ScreenShake(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        yield return new WaitForSeconds(time);
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
    }
}
