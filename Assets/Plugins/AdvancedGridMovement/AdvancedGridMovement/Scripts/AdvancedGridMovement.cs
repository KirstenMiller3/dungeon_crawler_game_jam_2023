﻿/* Copyright 2021-2022 Lutz Großhennig

Use of this source code is governed by an MIT-style
license that can be found in the LICENSE file or at
https://opensource.org/licenses/MIT.
*/


//using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

/*
 * This script adds animated grid based movement similar to Dungeon Master, Eye of the Beholder & Legend of Grimrock.
 * It overs advanced options like control over the movement and headbob. 
 */

public class AdvancedGridMovement : MonoBehaviour
{
    private const float RightHand = 90.0f;
    private const float LeftHand = -RightHand;
    private const float approximationThreshold = 0.025f;
    private bool start = true;

    [SerializeField] private float gridSize = 3.0f;
    [SerializeField] private LayerMask _groundCheckLayers;
    [SerializeField] private LayerMask _layersToCheck;

    [Header("Walk speed settings")]
    [SerializeField] private float walkSpeed = 2.0f;
    [SerializeField] private float turnSpeed = 5.0f;

    [Header("Walking animation curve")]
    [SerializeField] private AnimationCurve walkSpeedCurve;

    [Header("Walking head bob curve")]
    [SerializeField] private AnimationCurve walkHeadBobCurve;

    [Header("Run speed settings")]
    [SerializeField] private float runningSpeed = 1.5f;

    [Header("Running animation curve")]
    [SerializeField] private AnimationCurve runningSpeedCurve;

    [Header("Running head bob curve")]
    [SerializeField] private AnimationCurve runningHeadBobCurve;

    [Header("Maximum step height")]
    [SerializeField] private float maximumStepHeight = 2.0f;

    [Header("Event when the path is blocked")]
    [SerializeField] private UnityEvent blockedEvent;

    [Header("Event when the player takes a step")]
    [SerializeField] private UnityEvent stepEvent;

    [Header("Event when the player is turning")]
    [SerializeField] private UnityEvent turnEvent;

    // Animation target values.
    private Vector3 moveTowardsPosition;
    private Quaternion rotateFromDirection;

    // Animation source values.
    private Vector3 moveFromPosition;
    private Quaternion rotateTowardsDirection;

    // Animation progress
    private float rotationTime = 0.0f;
    private float curveTime = 0.0f;

    private float stepTime = 0.0f;
    private float stepTimeCounter = 0.0f;

    //Current settings
    private AnimationCurve currentAnimationCurve;
    private AnimationCurve currentHeadBobCurve;
    private float currentSpeed;

    private bool _invertMovement;
    private bool _disableMovement;

    void Start()
    {
        moveTowardsPosition = transform.position;
        rotateTowardsDirection = transform.rotation;
        currentAnimationCurve = walkSpeedCurve;
        currentHeadBobCurve = walkHeadBobCurve;
        currentSpeed = walkSpeed;
        stepTime = 1.5f / gridSize;
    }

    void Update() {
        if(_disableMovement) {
            return;
        }

        if (IsMoving())
        {
            AnimateMovement();
        }

        if (IsRotating())
        {
            AnimateRotation();
        }
    }

    public void Teleport(Vector3 moveToPosition, Quaternion dir) {
        moveTowardsPosition = moveToPosition;
        rotateTowardsDirection = dir;

        transform.position = moveToPosition;
        transform.rotation = dir;
    }

    public void SwitchToWalking()
    {
        var currentPosition = currentAnimationCurve.Evaluate(curveTime);
        var newPosition = walkSpeedCurve.Evaluate(curveTime);

        if (newPosition < currentPosition)
        {
            curveTime = FindTimeForValue(currentPosition, walkSpeedCurve);
        }

        currentSpeed = walkSpeed;
        currentAnimationCurve = walkSpeedCurve;
        currentHeadBobCurve = walkHeadBobCurve;
    }

    public void SwitchToRunning()
    {
        var currentPosition = currentAnimationCurve.Evaluate(curveTime);
        var newPosition = runningSpeedCurve.Evaluate(curveTime);

        if (newPosition < currentPosition)
        {
            curveTime = FindTimeForValue(currentPosition, runningSpeedCurve);
        }

        currentSpeed = runningSpeed;
        currentAnimationCurve = runningSpeedCurve;
        currentHeadBobCurve = runningHeadBobCurve;
    }

    private float FindTimeForValue(float position, AnimationCurve curve)
    {
        float result = 1.0f;

        while ((position < curve.Evaluate(result)) && (result > 0.0f))
        {
            result -= approximationThreshold;
        }

        return result;
    }

    private void AnimateRotation()
    {
        rotationTime += Time.deltaTime;
        transform.rotation = Quaternion.Slerp(rotateFromDirection, rotateTowardsDirection, rotationTime * turnSpeed);
        CompensateRoundingErrors();
    }

    private void AnimateMovement()
    {
        curveTime += Time.deltaTime * currentSpeed;

        stepTimeCounter += Time.deltaTime * currentSpeed;

        if (stepTimeCounter > stepTime)
        {
            stepTimeCounter = 0.0f;
            stepEvent?.Invoke();
        }

        var currentPositionValue = currentAnimationCurve.Evaluate(curveTime);
        var currentHeadBobValue = currentHeadBobCurve.Evaluate(curveTime * gridSize);
       
        var targetHeading = Vector3.Normalize(HeightInvariantVector(moveTowardsPosition) - HeightInvariantVector(moveFromPosition));
        var newPosition = moveFromPosition + (targetHeading * (currentPositionValue * gridSize));
        //newPosition.y = maximumStepHeight;
        
        //RaycastHit hit;
        //Ray downRay = new Ray(newPosition, -Vector3.up);

        // Cast a ray straight downwards.
        //if (Physics.Raycast(downRay, out hit, _groundCheckLayers))
        //{
        //    newPosition.y = (maximumStepHeight - hit.distance) + currentHeadBobValue;
        //}
        //else
        //{
        //    newPosition.y = currentHeadBobValue;
        //}

        newPosition.y = currentHeadBobValue;

        transform.position = newPosition;
        CompensateRoundingErrors();
    }

    private void CompensateRoundingErrors()
    {
        // Bear in mind that floating point numbers are inaccurate by design. 
        // The == operator performs a fuzy compare which means that we are only approximatly near the target value.
        // We may not entirely reached the value yet or we may have slightly overshot it already (both within the margin of error).
        if (transform.rotation == rotateTowardsDirection)
        {
            // To compensate rounding errors we explictly set the transform to our desired rotation.
            transform.rotation = rotateTowardsDirection;
        }

        //mask out the head bobbing
        var currentPosition = HeightInvariantVector(transform.position);
        var target = HeightInvariantVector(moveTowardsPosition);

        if (currentPosition == target)
        {
            // To compensate rounding errors we explictly set the transform to our desired rotation.
            currentPosition = HeightInvariantVector(moveTowardsPosition);
            currentPosition.y = transform.position.y;

            transform.position = currentPosition;
            curveTime = 0.0f;
            stepTimeCounter = 0.0f;
        }

    }

    private float _baseSpeed;
    private float _baseRunSpeed;
    public void SetSpeedMultiplier(float multiplier) {
        _baseSpeed = walkSpeed;
        walkSpeed *= multiplier;
        _baseRunSpeed = runningSpeed;
        runningSpeed *= multiplier;
    }

    public void ResetSpeedMultiplier() {
        walkSpeed = _baseSpeed;
        runningSpeed = _baseRunSpeed;
    }

    public void SetInvertedMovement(bool invert) {
        _invertMovement = invert;
    }

    public void DisableMovement(bool disable) {
        _disableMovement = disable;
    }

    public void MoveForward() {
        if(_disableMovement) {
            return;
        }
        if(_invertMovement) {
            CollisonCheckedMovement(-CalculateForwardPosition());
            return;
        }
        CollisonCheckedMovement(CalculateForwardPosition());
    }

    public void MoveBackward() {
        if(_disableMovement) {
            return;
        }
        if(_invertMovement) {
            CollisonCheckedMovement(CalculateForwardPosition());
            return;
        }
        CollisonCheckedMovement(-CalculateForwardPosition());
    }

    public void StrafeRight() {
        if(_disableMovement) {
            return;
        }
        if(_invertMovement) {
            CollisonCheckedMovement(-CalculateStrafePosition());
            return;
        }
        CollisonCheckedMovement(CalculateStrafePosition());
    }

    public void StrafeLeft() {
        if(_disableMovement) {
            return;
        }
        if(_invertMovement) {
            CollisonCheckedMovement(CalculateStrafePosition());
            return;
        }
        CollisonCheckedMovement(-CalculateStrafePosition());
    }

    private void CollisonCheckedMovement(Vector3 movementDirection)
    {
        if (IsStationary())
        {
            Vector3 targetPosition = moveTowardsPosition + movementDirection;
            if (start || FreeSpace(targetPosition))
            {
                moveFromPosition = transform.position;
                moveTowardsPosition = targetPosition;
                start = false;
            }
            else
            {
                Debug.Log("blocked");
                blockedEvent?.Invoke();
            }
        }
    }

    // should be refactored into an new class
    private bool FreeSpace(Vector3 targetPosition)
    {
        // this is pretty lousy way to perform collision checks, its just here for demonstration purposes.
        // Hint: layers are much faster then tags ;-)
          Vector3 delta = targetPosition - moveTowardsPosition;
          delta *= .6f;
        //  Collider[] intersectingColliders = Physics.OverlapBox(moveTowardsPosition + delta, new Vector3((gridSize / 2.0f) - .1f, 1.0f, (gridSize / 2.0f) - .1f), gameObject.transform.rotation);
        ////  Collider[] filteredColliders = System.Array.FindAll(intersectingColliders, collider => collider.CompareTag("Enemy") || collider.CompareTag("Level"));
        //  Collider[] filteredColliders = System.Array.FindAll(intersectingColliders, collider => collider.CompareTag("Level"));

        //Vector3 fwd = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        if(Physics.Raycast(transform.position + new Vector3(0, 1f, 0), delta, out hit, 4f, _layersToCheck)) {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("door")) {
               // AudioManager.instance.Play("pickup");
            }
            Debug.DrawRay(transform.position, delta * 3f, Color.red);
            return false;
        }

        Debug.DrawRay(transform.position, delta * 3f, Color.white);
        return true;
       // return filteredColliders.Length == 0;
    }

    public void TurnRight() {
        if(_disableMovement) {
            return;
        }
        turnEvent?.Invoke();

        if(_invertMovement) {
            TurnEulerDegrees(LeftHand);
            return;
        }

        TurnEulerDegrees(RightHand);
    }

    public void TurnLeft() {
        if(_disableMovement) {
            return;
        }
        turnEvent?.Invoke();

        if(_invertMovement) {
            TurnEulerDegrees(RightHand);
            return;
        }

        TurnEulerDegrees(LeftHand);
    }

    private void TurnEulerDegrees(in float eulerDirectionDelta)
    {
        if (!IsRotating())
        {
            rotateFromDirection = transform.rotation;
            rotateTowardsDirection *= Quaternion.Euler(0.0f, eulerDirectionDelta, 0.0f);
            rotationTime = 0.0f;
        }
    }

    public bool IsStationary()
    {
        return !(IsMoving() || IsRotating());
    }

    private bool IsMoving()
    {
        var current = HeightInvariantVector(transform.position);
        var target = HeightInvariantVector(moveTowardsPosition);
        return current != target;
    }

    private bool IsRotating()
    {
        return transform.rotation != rotateTowardsDirection;
    }

    private Vector3 HeightInvariantVector(Vector3 inVector)
    {
        return new Vector3(inVector.x, 0.0f, inVector.z);
    }

    private Vector3 CalculateForwardPosition()
    {
        return transform.forward * gridSize;
    }

    private Vector3 CalculateStrafePosition()
    {
        return transform.right * gridSize;
    }
}
