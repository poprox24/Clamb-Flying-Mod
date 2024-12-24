using UnityEngine;
using MelonLoader;
using UnityEngine.InputSystem;
using System;

namespace flyingMod
{
    public class FlyingMod : MelonMod
    {

        GameObject player = null;
        GameObject rightHand = null;
        Rigidbody controller = null;

        bool canFly = true;
        bool flyActionCreated = false;

        InputAction flyAction;

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            Console.WriteLine(sceneName);
            if (sceneName == "Home")
            {
                //SceneManager.LoadScene(1);
                if (flyActionCreated) flyAction.Disable();
            }
            else if (sceneName == "Hike_V2" || sceneName == "Hike_V2_Demo")
            {
                player = GameObject.Find("Character Controller_Rigidbody");
                controller = player.GetComponent<Rigidbody>();
                rightHand = GameObject.Find("Hand_R_Controller");

                if (player == null) LoggerInstance.Error("Player not found.");
                if (rightHand == null) LoggerInstance.Error("Right Controller not found.");


                if (!flyActionCreated)
                {
                    flyAction = new InputAction(
                        "Fly",
                        InputActionType.Button,
                        "<XRController>{rightHand}/secondaryButton"
                    );
                    flyActionCreated = true;
                }

                flyAction.Enable();
            }

        }
        public override void OnUpdate()
        {
            if (flyAction != null && flyAction.IsPressed() && player != null)
            {
                float maxSpeed = 4f;
                Vector3 velocity = controller.velocity;

                float speed = new Vector3(velocity.x, 0, velocity.z).magnitude;
                if (speed > maxSpeed)
                {
                    canFly = false;
                } else canFly = true;

                Vector3 directionHandForward = rightHand.transform.forward;
                Vector3 movement = directionHandForward * 2f + new Vector3(0, 0.2f, 0);
                controller.AddForce(movement, ForceMode.VelocityChange);
            } 
            else if (flyAction != null && flyAction.WasReleasedThisFrame())
            {
                controller.velocity = Vector3.zero;
            }
        }
    }
}
