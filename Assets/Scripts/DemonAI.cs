using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonAI : MonoBehaviour
{
    [SerializeField] string m_DemonType;
    [SerializeField] GameObject m_game_Manager;
    // Start is called before the first frame update
    void Start()
    {
        List<GameObject> map = m_game_Manager.GetComponentInChildren<ListScript>().Tilemap2DArray;
    }

    // Update is called once per frame
    void Update()
    {
            // Check the array to see if you find a building, saving the first, checking by proximity.
            // If you find a the m_DemonType tag on the building, thats the better target. If you see Vice as the tag, that's the best target
            // If the demon is next to a building, deal damage to it.
    }
}
