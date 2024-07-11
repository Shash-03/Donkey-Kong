using UnityEngine;

public class Barrel : MonoBehaviour
{
    private Rigidbody2D barrel;
    public float speed = 1f;

    private void Awake()
    {
        barrel = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer  == LayerMask.NameToLayer("Ground"))
        {
            barrel.AddForce(collision.transform.right * (speed + 5f), ForceMode2D.Impulse);
        }
    }


}