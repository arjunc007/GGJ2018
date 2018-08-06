using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField]
    private GameObject[] hidden;
    private GameObject[] b;
    private List<GameObject> built = new List<GameObject>();
    private Player_Controller pc;

    private void Awake()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject go in gos)
        {
            if (go.name != "Graphics")
            {
                pc = go.GetComponent<Player_Controller>();
            }
        }
        foreach (GameObject go in hidden)
        {
            go.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player" && pc.hasComponent)
        {
            RevealComponent();
        }
    }

    private void RevealComponent()
    {
        foreach (GameObject go in hidden)
        {
            if (go.activeSelf)
            {
                continue;
            }
            else
            {
                if (go.name == pc.componentName)
                {
                    go.SetActive(true);
                    pc.hasComponent = false;
                    built.Add(go);
                    break;
                }
            }
        }

        if (hidden.Length == built.Count)
        {
            StartCoroutine(Complete());
        }
    }

    IEnumerator Complete()
    {
        yield return new WaitForSeconds(2f);
        Application.LoadLevel("Menu");
    }
}
