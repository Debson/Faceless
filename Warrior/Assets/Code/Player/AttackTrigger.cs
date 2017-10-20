using UnityEngine;


public class AttackTrigger : MonoBehaviour
{
    [SerializeField]
    public int damageToGive;
  
    protected void Awake()
    {
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            collision.GetComponent<EnemyHealthManager>().giveDamage(damageToGive);
        }
    }

}
