using UnityEngine;
using UnityEngine.SceneManagement;

public class FloorExit : MonoBehaviour
{
    public string nextScene;
    private bool loading;
    public FloorManager floorManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (loading) return;
        if(!floorManager.AllCleaned()) return;

        if (other.GetComponentInParent<PlayerMovement>() != null)
        {
            loading = true;
            SceneManager.LoadSceneAsync(nextScene);
        }
    }
}
