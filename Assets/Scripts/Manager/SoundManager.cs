using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;



public class SoundManager : MonoBehaviour
{
    #region Singleton
    public static SoundManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    [Header("Audio Clips")]
    public AudioClip gameStartSound;
    public AudioClip buttonSound;
    public AudioClip cardFlipSound;
    public AudioClip cardMatchSound;
    public AudioClip stageClearSound;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlayGameStartSound()
    {
        PlaySound(gameStartSound);
    }

    public void PlayButtonSound()
    {
        PlaySound(buttonSound);
    }

    public void PlayCardFlipSound()
    {
        PlaySound(cardFlipSound);
    }

    public void PlayCardMatchSound()
    {
        PlaySound(cardMatchSound);
    }

    public void PlayStageClearSound()
    {
        PlaySound(stageClearSound);
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
