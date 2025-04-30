using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField]private Camera camera;
    [SerializeField]private WeaponBasicFunctions weapon;
    private Vector3 mousePos; // XYZ coordinates of the mouse location on screen
    private bool stopRotation = false;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
         mousePos = camera.ScreenToWorldPoint(Input.mousePosition);

         Vector3 rotation = mousePos - transform.position;

        float angle = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        if (stopRotation == false)
        {
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        
        
        if (stopRotation == true) 
        {
            if (timer >0)
            {
                timer -= Time.deltaTime;
            }
            else if(timer <= 0)
            {
                timer = 0;
                stopRotation = false;
            }
        }

        if (stopRotation == false)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                weapon.MakeAttack(1);
                if (weapon.isInCooldown())
                {
                    timer = weapon.getCooldownTime();
                    stopRotation = true;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                weapon.MakeAttack(2);
                if (weapon.isInCooldown())
                {
                    timer = weapon.getCooldownTime();
                    stopRotation = true;
                }
            }
        }



    }
}
