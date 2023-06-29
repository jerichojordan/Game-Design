using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float HealthAmount;
    [SerializeField] AudioClip PickupSound;
    private AudioSource Asource;
    private GameObject playerObj;
    private Player player;
    void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        Asource = this.gameObject.GetComponent<AudioSource>();
        Asource.volume += 3f;
        playerObj = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && player.HitPoint < 100)
        {
            if (player.HitPoint + HealthAmount > 100) { player.HitPoint = 100; }
            else player.HitPoint += HealthAmount;
            AudioSource.PlayClipAtPoint(PickupSound,playerObj.transform.position);
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine(PlayParticle());
            Debug.Log("Health +" + HealthAmount);
        }

    }
    // Update is called once per frame
    private IEnumerator PlayParticle()
    {
        this.gameObject.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(4.0f);
        this.gameObject.SetActive(false);
    }
}
