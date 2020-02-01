using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonAI : MonoBehaviour
{
    [SerializeField] string m_DemonType;
    [SerializeField] GameObject m_game_Manager;
    List<GameObject> map = new List<GameObject>();
    GameObject currentTarget = null;
    public int speed =10;
    // Start is called before the first frame update
    void Start()
    {
        map = m_game_Manager.GetComponentInChildren<ListScript>().Tilemap2DArray;
        print(map);
    }
    private void Awake()
    {
        map = m_game_Manager.GetComponentInChildren<ListScript>().Tilemap2DArray;
    }

    // Update is called once per frame
    void Update()
    {
        if (map.Count > 0)
        {
            for (int i = 0; i < map.Count; i++)
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
            transform.LookAt(currentTarget.transform);
            transform.Translate(new Vector2(speed* Time.deltaTime, 0));

        }

            // Check the array to see if you find a building, saving the first, checking by proximity.
            // If you find a the m_DemonType tag on the building, thats the better target. If you see Vice as the tag, that's the best target
            // If the demon is next to a building, deal damage to it.
    }
}
