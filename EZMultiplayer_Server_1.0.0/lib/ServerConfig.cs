using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkLib
{
    static class ServerConfig
    {
        /// <summary>
        /// Минимальное кол-во человек для старта игры
        /// </summary>
        internal static uint minCountPlayersForStartGame = 1;

        /// <summary>
        /// Игра автоматически начинается,когда комната полностью заполнена
        /// </summary>
        internal static bool autoStartGame=false;

        /// <summary>
        /// Кол-во спавн точек в игре
        /// </summary>
        internal static uint spawnPointCount = 3;


    }
}
