using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class HockeyController : MonoBehaviour
{
    [SerializeField]
    GameObject Palette;
    GameObject palette;

    [SerializeField]
    GameObject Panel;
    GameObject panel;

    [SerializeField]
    GameObject Floor;
    GameObject floor;

    [NonSerialized]
    public SelectController select;

    Vector3 velocity;

    float time;
    float speed;

    void Start()
    {
        Initialize();
        time = Time.time;
    }

    void Update()
    {
        speed += 0.1f * Time.deltaTime;
        palette.transform.position = new Vector3(palette.transform.position.x , -14.2f , palette.transform.position.z);
        float palX = palette.transform.position.x;
        float palZ = palette.transform.position.z;

        float paneX = panel.transform.position.x;

        float hor = Input.GetAxis("Horizontal");

        float dis = Vector3.Distance(palette.transform.position , panel.transform.position);

        if(palZ >= -5.5f)
		{
            velocity.z = -velocity.z;
            palette.transform.position += Vector3.back * 0.15f;
		}
        if(palZ < -14.5f && dis <= 1.5f)
		{
            velocity.z = -velocity.z;
            palette.transform.position += Vector3.forward * 0.1f;
        }
        if(palX > 8.5f)
		{
            velocity.x = -velocity.x;
            palette.transform.position += Vector3.left * 0.15f;
		}
        if(palX < -8.5f)
        {
            velocity.x = -velocity.x;
            palette.transform.position += Vector3.right * 0.15f;
        }

        if(palZ < -16f) Finish();

        palette.transform.position += velocity * speed * Time.deltaTime;
        if(paneX <= 8f && paneX >= -8f)
        {
            panel.transform.position += new Vector3(hor , 0 , 0) * 12f * Time.deltaTime;
        }
        else if(paneX <= 8f) panel.transform.position += Vector3.right * 0.2f;
        else if(paneX >= 8f) panel.transform.position += Vector3.left * 0.2f;
    }
    void Finish()
	{
        Destroy(palette);
        Destroy(panel);
        Destroy(floor);
        {
            string sub = "";
            float t = Time.time - time;
            int min = (int)( t / 60 );
            int sec = (int)( t % 60 );
            if(sec < 10) sub = "0";
            string result = min.ToString() + ":" + sub + sec.ToString();
            select.EndPuzzle(result);
        }
        Destroy(this.gameObject);
	}
    void Initialize()
    {
        select = GameObject.Find("PuzzleSelect").GetComponent<SelectController>();

        float x = UnityEngine.Random.Range(1 , 2) / 1f;
        float z = UnityEngine.Random.Range(1 , 2) / 1f;

		velocity = new Vector3(x , 0 , z);
        speed = 3f;

        palette = Instantiate(Palette);
        panel = Instantiate(Panel);
        floor = Instantiate(Floor);
	}
}