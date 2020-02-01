using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonAI : MonoBehaviour
{
    [SerializeField] string m_DemonType;
    [SerializeField] GameObject m_game_Manager;
    
    GameObject currentTarget = null;
    public int speed =10;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] map = GameObject.FindGameObjectsWithTag("Mind");
        
        for (int i = 0; i < map.Length; i++)
        {
            if (currentTarget == null)
            {
                currentTarget = map[i];
            }
            else if (currentTarget.tag != m_DemonType && map[i].tag == m_DemonType)
            {
                currentTarget = map[i];
            }
        }
        Vector3 direction = (currentTarget.transform.position - transform.position);
        direction.Normalize();
        transform.Translate(direction*speed * Time.deltaTime);

        

            // Check the array to see if you find a building, saving the first, checking by proximity.
            // If you find a the m_DemonType tag on the building, thats the better target. If you see Vice as the tag, that's the best target
            // If the demon is next to a building, deal damage to it.
    }
}
