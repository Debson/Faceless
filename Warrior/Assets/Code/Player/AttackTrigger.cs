using UnityEngine;


public class AttackTrigger : MonoBehaviour
{
    [SerializeField]
    public int damageToGive;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {

        }
    }
}
