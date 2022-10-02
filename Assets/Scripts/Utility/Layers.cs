using UnityEngine;
public static class Layers {
    public static int Default = LayerMask.NameToLayer("Default");
    public static int TransparentFX = LayerMask.NameToLayer("TransparentFX");
    public static int IgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
    public static int Water = LayerMask.NameToLayer("Water");
    public static int UI = LayerMask.NameToLayer("UI");

    public static int Player = LayerMask.NameToLayer("Player");
    public static int BeatBox = LayerMask.NameToLayer("BeatBox");
    public static int Note = LayerMask.NameToLayer("Note");
    public static int Enemy = LayerMask.NameToLayer("Enemy");
    public static int BuildTile = LayerMask.NameToLayer("BuildTile");

    public static int GetMask(int layer) {
        return 1 << layer;
    }

    public static int GetMask(params int[] layers) {
        int mask = 0;
        for (int i = 0; i < layers.Length; i++) {
            mask |= 1 << layers[i];
        }
        return mask;
    }
}