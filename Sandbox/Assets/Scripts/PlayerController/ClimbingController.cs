using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

[ExecuteInEditMode]
public class ClimbingController : MonoBehaviour
{
    [Header("Vault properies")]
    public Vector3 vault_SPH; //' speed, height and distance variables of vault


    public float[] origins_forward;
    public float[] origins_down;
    public LedgeDetector ledgeDetector;

    public bool isTouchingWall;
    // climbing variables
    public bool isVaulting;
    public bool isClimbing;

    public bool canVault;
    public bool canClimb;
    public bool canJumpClimb;

    public bool ledgeFound;

    // components
    public Rigidbody _rb;
    public PlayerInputHandler InputHandler { get; protected set; }

    protected bool inputInteract;

    // Start is called before the first frame update
    void Start()
    {
        ledgeDetector = new LedgeDetector(this.transform, origins_forward, origins_down);

        _rb = GetComponent<Rigidbody>();
        InputHandler = GetComponent<PlayerInputHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        inputInteract = InputHandler.InputInteract;

        if (ledgeDetector != null)
        {
            isTouchingWall = ledgeDetector.TouchingWall().Any();
            ledgeFound = ledgeDetector.ledgeFound;
        }
        else
        {
            ledgeFound = false;
            isTouchingWall = false;
        }
       

        if(isTouchingWall)
        {
            if(ledgeDetector.TouchingWall().Length>0)
            {
                bool[] hits = ledgeDetector.TouchingWall();

                canVault = hits[0] && !hits[1] && !hits[2];
                canClimb = hits[1] && !hits[2];
                canJumpClimb = hits[2];

                //Debug.Log(ColourConsoleText(canVault, "VAULT") + "    " + ColourConsoleText(canClimb, "CLIMB") + "    " + ColourConsoleText(canJumpClimb, "JUMP"));

            }
        }

        if(inputInteract)
        {
            if (canVault)
            {
                Vault();
            }

            if (canClimb)
            {
                Climb();
            }

            if (canJumpClimb)
            {
                Climb();
            }
            inputInteract = false;
        }

        if (ledgeDetector != null && ledgeFound)
            ledgeDetector.HeightCheck(ledgeDetector.ledgePosition, 2);

    }


    // vault
    public void Vault()
    {
        isVaulting = true;

        Vector3 jumpPos = transform.position + transform.up * 1f;
        Vector3 landingPos = transform.position + transform.forward * 0.5f;
        Vector3 distance = landingPos - transform.position;

        _rb.DOJump(landingPos, 0.5f, 1, 0.8f).OnComplete(FinishVault);
        GetComponent<Animator>().SetBool("Vault", true);
        //Invoke(nameof(SetVaultAnimFalse), 0.8f);
    }

    void FinishVault()
    {
        isVaulting = false;
        GetComponent<Animator>().SetBool("Vault", false);
    }

    // climb
    public void Climb()
    {
        isClimbing = true;

        Vector3 jumpPos = transform.position + transform.up * 1f;
        Vector3 landingPos = ledgeDetector.ledgePosition;
        Vector3 distance = landingPos - transform.position;

        //GetComponent<CharacterController>().Move(distance);
        _rb.DOJump(landingPos, 0.1f, 1, 0.3f).OnComplete(FinishClimb);
        GetComponent<Animator>().SetBool("ClimbUp", true);
    }

    void FinishClimb()
    {
        isClimbing = false;
        GetComponent<Animator>().SetBool("ClimbUp", false);
    }



    // console text display
    string ColourConsoleText(bool result, string text)
    {
        string newString = "";
        if (result)
        {
            newString = string.Format("<color=#00ff00>{0}</color>", text);
        }
        else
        {
            newString = string.Format("<color=#ff0000>{0}</color>", text);
        }

        return newString;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (ledgeDetector!= null && ledgeDetector.ledgeFound)
        {
            Gizmos.DrawSphere(ledgeDetector.ledgePosition, 0.1f);
        }

    }
}
