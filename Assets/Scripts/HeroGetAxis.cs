using UnityEngine;

public class HeroGetAxis : HeroController
{
    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Move(x, y);
    }
}
