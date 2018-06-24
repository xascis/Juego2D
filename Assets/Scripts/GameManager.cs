using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager {
    // Singleton donde se guardan las variables globales

    // número de monedas recogidas por el jugador
    public static int currentNumberCoins = 0;

    // número de corazones
    public static int currentNumberHearth = 3;

    // habilidad
    public static bool fireballSkill = false;
    // estado de la llave
    public static bool keyRedFound = false;

    // nivel actual de juego
    public static int currentLevel = 0;
    public static int maxLevel = 2;

    // configuración música
    public static bool musicSettings = true;
}
