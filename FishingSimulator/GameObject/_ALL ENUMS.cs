using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoatGame
{
    #region States enum
    /// <summary>
    /// Enumerated value for the Game1 class to use to determine the current FSM of the game
    /// </summary>
    enum States
    {
        map,
        docked,
        titleMenu,
        gameOver,
        fishing,
        pause,
        controls,
        invent
    }
    #endregion

    #region Directions enum
    /// <summary>
    /// Enumerated value for any class that moves to determine orientation of the object moving
    /// </summary>
    enum Directions
    {
        up,
        down,
        left,
        right,
        idle
    }
    #endregion

    #region ObjectType enum
    /// <summary>
    ///Enumerated value to make sure to have any object inheriting this class identify itself
    /// </summary>

    enum ObjectType
    {
        player,
        island,
        fishingSpot,
        storm,
        pirate
    }
    #endregion
    class _ALL_ENUMS
    {}
}
