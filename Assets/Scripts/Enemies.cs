using UnityEngine;

public class Enemies : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Transform player;
    public float moveSpeed = 1.5f;
    public bool requiresHighAttack;

    public FloorManager floorManager;
    private bool alreadyCleaned;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null){
            return;
        }
        float nextX = Mathf.MoveTowards(transform.position.x,player.position.x,moveSpeed*Time.deltaTime); //Method moves enemy towards player
        transform.position = new Vector3(nextX, transform.position.y,transform.position.z);
    }
    public void match(bool highAttack)
    {
        if(highAttack == requiresHighAttack)
        {
            alreadyCleaned = true;
            floorManager.AddCleaned();
            Destroy(gameObject);
        }
    }
}
