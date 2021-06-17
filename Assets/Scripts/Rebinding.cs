using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Rebinding : MonoBehaviour
{
    //public InputActionReference forwardAction = null;
    //public InputAction   backAction = null;
    //public InputActionReference leftAction = null;
    //public InputActionReference rightAction = null;
    public InputActionReference shootAction = null; 
    public InputActionReference interactAction = null;
    public InputActionReference jumpAction = null;
    public PlayerInput playerInput = null;

    public TMPro.TMP_Text bindingDisplayNameTextJump = null;
    public GameObject startRebindObjectJump = null;
    public GameObject waitingForInputObjectJump = null;

    public TMPro.TMP_Text bindingDisplayNameTextShoot = null;
    public GameObject startRebindObjectShoot = null;
    public GameObject waitingForInputObjectShoot = null;

    public TMPro.TMP_Text bindingDisplayNameTextInteract = null;
    public GameObject startRebindObjectInteract = null;
    public GameObject waitingForInputObjectInteract = null;

    public GameObject EInteract;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    //Jump

    public void Start()
    {
        EInteract.GetComponent<TMPro.TMP_Text>().text = "'" + InputControlPath.ToHumanReadableString(interactAction.action.bindings[0].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice) + "'" + " Interact";
        EInteract.SetActive(false);
        //backAction = playerInput.actions.FindAction("Forward");
    }

    public void StartRebindingJump()
    {
        startRebindObjectJump.SetActive(false);
        waitingForInputObjectJump.SetActive(true);

        playerInput.SwitchCurrentActionMap("Menu");
        rebindingOperation = jumpAction.action.PerformInteractiveRebinding().WithControlsExcluding("Mouse" + "Escape").OnMatchWaitForAnother(0.1f).OnComplete(operation => RebindComplete(jumpAction, bindingDisplayNameTextJump, startRebindObjectJump, waitingForInputObjectShoot)).Start();
    }

    public void StartRebindingInteract()
    {
        startRebindObjectInteract.SetActive(false);
        waitingForInputObjectInteract.SetActive(true);

        playerInput.SwitchCurrentActionMap("Menu");
        rebindingOperation = interactAction.action.PerformInteractiveRebinding().WithControlsExcluding("Mouse" + "Escape").OnMatchWaitForAnother(0.1f).OnComplete(operation => RebindComplete(interactAction, bindingDisplayNameTextInteract, startRebindObjectInteract, waitingForInputObjectInteract)).Start();
    }

    public void StartRebindingShoot()
    {
        startRebindObjectShoot.SetActive(false);
        waitingForInputObjectShoot.SetActive(true);

        playerInput.SwitchCurrentActionMap("Menu");
        rebindingOperation = shootAction.action.PerformInteractiveRebinding().WithControlsExcluding("Escape").OnMatchWaitForAnother(0.1f).OnComplete(operation => RebindComplete(shootAction, bindingDisplayNameTextShoot, startRebindObjectShoot, waitingForInputObjectShoot)).Start();
    }
    
    private void RebindComplete(InputActionReference temp, TMPro.TMP_Text displayName, GameObject startRebingObject, GameObject waitingForInput)
    {
        int bindingIndex = temp.action.GetBindingIndexForControl(temp.action.controls[0]);

        displayName.text = InputControlPath.ToHumanReadableString(temp.action.bindings[bindingIndex].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        EInteract.GetComponent<TMPro.TMP_Text>().text = "'" + InputControlPath.ToHumanReadableString(rebindingOperation.action.bindings[0].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice) + "'" + " Interact";
        rebindingOperation.Dispose();
        startRebingObject.SetActive(true);
        waitingForInput.SetActive(false);

        playerInput.SwitchCurrentActionMap("PlayerInput");
    }

    //private void RebindComplete1D1(InputActionReference temp)
    //{
    //    int bindingIndex = temp.action.bindings.IndexOf(temp => temp.isPartOfComposite && temp.name == "negative");

    //    bindingDisplayNameText.text = InputControlPath.ToHumanReadableString(temp.action.bindings[bindingIndex].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);

    //    rebindingOperation.Dispose();
    //    startRebindObject.SetActive(true);
    //    waitingForInputObject.SetActive(false);

    //    playerInput.SwitchCurrentActionMap("PlayerInput");
    //}

    //public void StartRebindingBack()
    //{
    //    startRebindObject.SetActive(false);
    //    waitingForInputObject.SetActive(true);

    //    playerInput.SwitchCurrentActionMap("Menu");

    //    //rebindingOperation = backAction.action.PerformInteractiveRebinding().WithControlsExcluding("Mouse").OnMatchWaitForAnother(0.1f).OnComplete(operation => RebindComplete1D1(backAction)).Start(); 

    //    rebindingOperation = backAction.PerformInteractiveRebinding().WithTargetBinding(1).OnComplete(operation => Test(operation)).Start();
    //}

    //private void Test(InputActionRebindingExtensions.RebindingOperation operation)
    //{
    //    Debug.Log(operation.action.bindings[1].path);
    //    //Debug.Log(backAction.action.bindings[1].path);

    //    rebindingOperation.Dispose();
    //    startRebindObject.SetActive(true);
    //    waitingForInputObject.SetActive(false);

    //    playerInput.SwitchCurrentActionMap("PlayerInput");
    //}

}
