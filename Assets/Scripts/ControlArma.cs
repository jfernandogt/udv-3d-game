using UnityEngine;
using UnityEngine.InputSystem;

using System;
using System.Collections;
using System.Collections.Generic;

public class ControlArma : MonoBehaviour
{
    [SerializeField] private Arma arma;

    public void AlDisparar(InputAction.CallbackContext value)
    {
        arma.ProcesarEntrada(value.action.triggered);
    }
}
