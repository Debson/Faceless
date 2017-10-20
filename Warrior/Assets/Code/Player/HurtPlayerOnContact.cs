using UnityEngine;

public class HurtPlayerOnContact : MonoBehaviour
{
    [SerializeField]
    public int damageToGive;

    private void OnTriggerStay2D(Collider2D collision)
      {
         if (collision.tag == "Player")
         {
            HealthManager.HurtPlayer(damageToGive);

            var player = collision.GetComponent<WalkMovement>();
            player.knockbackTimeCount = player.knockBackLength;

            if(collision.transform.position.x < transform.position.x)
            {
                player.knockFromRight = true;
            }
            else
            {
                player.knockFromRight = false;
            }
        }
    }   
}
