using UnityEngine;
using SlimeBasher.CrossPlatformInput;

namespace SlimeBasher.Characters.ThirdPerson
{
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        private ThirdPersonCharacter character; // A reference to the ThirdPersonCharacter on the object
        private Transform cameraTransform;      // A reference to the main camera in the scenes transform
        private Vector3 cameraForward;          // The current forward direction of the camera
        private Vector3 movementVector;
        private bool doJump;                    // the world-relative desired move direction, calculated from the camForward and user input.
        private bool doAttack;


        private void Start()
        {
            // get the transform of the main camera
            if (Camera.main != null)
            {
                cameraTransform = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning("Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

            // get the third person character ( this should never be null due to require component )
            character = GetComponent<ThirdPersonCharacter>();
        }


        private void Update()
        {
            if (!doJump)
            {
                doJump = CrossPlatformInputManager.GetButtonDown("Jump");
            }

            doAttack = CrossPlatformInputManager.GetButtonDown("Fire1");
        }


        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            // read inputs
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
            bool crouch = false; //Input.GetButtonDown("Crouch");

            // calculate move direction to pass to character
            if (cameraTransform != null)
            {
                // calculate camera relative direction to move:
                cameraForward = Vector3.Scale(cameraTransform.forward, new Vector3(1, 0, 1)).normalized;
                movementVector = v * cameraForward + h * cameraTransform.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                movementVector = v * Vector3.forward + h * Vector3.right;
            }

#if !MOBILE_INPUT
            // walk speed multiplier
            if (Input.GetButton("Sprint"))
                movementVector *= 0.5f;
#endif

            // pass all parameters to the character control script
            character.Move(movementVector, crouch, doJump, doAttack);
            doJump = false;
        }
    }
}
