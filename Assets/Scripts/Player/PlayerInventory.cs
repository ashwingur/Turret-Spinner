using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private Transform basicTurret;
    [SerializeField] private GameObject turretHudIcon;
    private TurretType currentTurret;

    private void Start()
    {
        currentTurret = TurretType.None;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && currentTurret != TurretType.None)
        {
            switch (currentTurret)
            {
                case TurretType.Basic:
                    Instantiate(basicTurret, transform.position, Quaternion.Euler(new Vector2(0, 0)));
                    currentTurret = TurretType.None;
                    break;
            }
            turretHudIcon.SetActive(false);
        }
    }

    public void SetCurrentTurret(TurretType turret)
    {
        currentTurret = turret;
        turretHudIcon.SetActive(true);
    }
}

public enum TurretType
{
    None,
    Basic
}
