using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBGAnimation : MonoBehaviour
{
    Animator anim;
    private float run = 5f;
    private int count = 0;

    private void Start()
    {
        anim = this.gameObject.GetComponentInChildren<Animator>();
    }

    //Walk left run right
    private void Update()
    {

        if (this.transform.position.x < 35f)
        {
            transform.position += gameObject.transform.right * (run * Time.deltaTime);
        }
        if (this.transform.position.x >= 35f)
        {
            transform.position = new Vector3(-35f, transform.position.y, transform.position.z);
            if (count == 1)
            {
                count--;
                anim.SetTrigger("Run");
            }
            else if (count == 0)
            {
                count++;
                anim.SetTrigger("Walk");
            }
        }
        /*else if (this.transform.position.x >= -35 && !right)
        {
            transform.position += gameObject.transform.right * (run * Time.deltaTime);
            transform.rotation = new Quaternion(0, 180, 0, 0);
            anim.SetTrigger("Right");
            right = true;
        }*/
    }

    public void Tutorial()
    {
        Application.LoadLevel("Tutorial");
    }

    public void Play()
    {
        Application.LoadLevel("Gameplay");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
