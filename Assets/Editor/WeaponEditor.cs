using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Weapon))]
public class WeaponEditor : Editor
{

    Weapon weapon;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        weapon = (Weapon)target;

        EditorGUILayout.LabelField("Weapon Helpers");

        if(GUILayout.Button("Save gun equip location"))
        {
            Transform weaponT = weapon.transform;
            Vector3 weaponPos = weaponT.localPosition;
            Vector3 weaponRot = weaponT.localEulerAngles;
            weapon.weaponSettings.equipPosition = weaponPos;
            weapon.weaponSettings.equipRotation = weaponRot;
        }
        if (GUILayout.Button("Save gun UNequip location"))
        {
            Transform weaponT = weapon.transform;
            Vector3 weaponPos = weaponT.localPosition;
            Vector3 weaponRot = weaponT.localEulerAngles;
            weapon.weaponSettings.unEquipPosition = weaponPos;
            weapon.weaponSettings.unEquipRotation = weaponRot;
        }

        EditorGUILayout.LabelField("Debug Positioning");

        if (GUILayout.Button("Move gun to equip location"))
        {
            Transform weaponT = weapon.transform;
            weaponT.localPosition = weapon.weaponSettings.equipPosition;
            Quaternion eulerAngles = Quaternion.Euler(weapon.weaponSettings.equipRotation);
            weaponT.localRotation = eulerAngles;
        }

        if (GUILayout.Button("Move gun to UNequip location"))
        {
            Transform weaponT = weapon.transform;
            weaponT.localPosition = weapon.weaponSettings.unEquipPosition;
            Quaternion eulerAngles = Quaternion.Euler(weapon.weaponSettings.unEquipRotation);
            weaponT.localRotation = eulerAngles;
        }
    }
}
