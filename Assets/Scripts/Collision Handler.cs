using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] AudioClip obstacleHitAudio;
    [SerializeField] AudioClip finishLevelAudio;
    [SerializeField] ParticleSystem obstacleHitParticles;
    [SerializeField] ParticleSystem finishLevelParticles;

    AudioSource audioSource;

    bool isTransitioning = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision other)
    {
        if(isTransitioning) return;
        switch(other.gameObject.tag)
        {
            case "Friendly":
                print("This thing is friendly");
                break;
            case "Fuel":
                print("You got fuel");
                break;
            case "Finish":
                SuccessHandler();
                break;
            default:
                CrashHandler();
                break;
        }
    }

    void SuccessHandler()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(finishLevelAudio);
        finishLevelParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("LoadLevel", 1f);
    }

    private void LoadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene((currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings);
    }

    void CrashHandler()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(obstacleHitAudio);
        obstacleHitParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("ResetScene", 1f);
    }

    private void ResetScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
