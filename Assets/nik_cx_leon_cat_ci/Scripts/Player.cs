using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector]
    public Vector2 lookDirection;
    [SerializeField] SpriteRenderer myRend;

    private void Start()
    {
        lookDirection = myRend.flipX ? Vector2.left : Vector2.right;
    }

    private void OnMouseDown()
    {
        if (!Manager.IsStarted)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            FlipMeX();
        }
    }

    void FlipMeX()
    {
        myRend.flipX = !myRend.flipX;
        lookDirection = myRend.flipX ? Vector2.left : Vector2.right;
    }
}
