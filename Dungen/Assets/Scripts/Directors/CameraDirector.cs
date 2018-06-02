using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraDirector : MonoBehaviour
{
    public static CameraDirector _this;
    public GameObject mPlayer;

    public float MVelocity = 10f;
    public float MStoppedVelocity = 1f;
    public float MAttackVelocity = 2f;
    public Image MDeathIndicatorPanel;

    private RaycastHit hitInfo;

    private int MCurrentState = 0;
    private enum EStates
    {
        Moving,
        Stopped,
        Dead,
        Attack
    }

	void Start ()
    {
        _this = this;
	}
	
	void Update ()
    {
        MCurrentState = PlayerDirector._this.ReturnCurrentState();
        MoveCamera(MCurrentState);

        switch ((int)GetDistanceToPlayer())
        {
            case 6:
                MDeathIndicatorPanel.color = new Color(1, 1, 1, 0f);
                break;
            case 5:
                MDeathIndicatorPanel.color = new Color(1, 1, 1, .5f);
                break;
            case 4:
                MDeathIndicatorPanel.color = new Color(1, 1, 1, .7f);
                break;
            case 3:
                MDeathIndicatorPanel.color = new Color(1, 1, 1, .8f);
                break;
            case 2:
                MDeathIndicatorPanel.color = new Color(1, 1, 1, .9f);
                break;
            case 1:
                MDeathIndicatorPanel.color = new Color(1, 1, 1, 1f);
                break;
            default:
                MDeathIndicatorPanel.color = new Color(1, 1, 1, 0);
                break;
        }
	}

    private void MoveCamera(int state)
    {
        switch (state)
        {
            case 0:         // Moving
                transform.Translate(0, 0, MVelocity * Time.deltaTime);
                break;
            case 1:         // Stopped
                transform.Translate(0, 0, MStoppedVelocity * Time.deltaTime);
                break;
            case 2:         // Dead
                break;
            case 3:         // Attack
                transform.Translate(0, 0, MAttackVelocity * Time.deltaTime);
                break;
        }
    }

    public void SetCurrentState(int state)
    {
        if(state >= 0 && state <= 3)
        {
            MCurrentState = state;
        }
    }

    public bool IsPlayerDead()
    {
        // Check if the player is dead from something else
        if(PlayerDirector._this.ReturnCurrentState() == (int)EStates.Dead)
        {
            MCurrentState = (int)EStates.Dead;
            return true;
        }

        // Check if the player is too close to the camera
        if(GetDistanceToPlayer() >= 2)
        {
            return false;
        }
        else if(GetDistanceToPlayer() < 2)
        {
            return true;
        }
        return false;
    }

    public float GetDistanceToPlayer()
    {
        if (mPlayer)
        {
            return Mathf.Round(this.transform.position.x - mPlayer.transform.position.x);
        }
        return 0;
    }
}
