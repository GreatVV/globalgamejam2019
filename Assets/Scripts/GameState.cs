using System;
using System.Collections.Generic;

namespace Client
{
    public class GameState
    {
        public int ShipEntity;        

        public readonly Dictionary<int, PlayerRole> Roles = new Dictionary<int, PlayerRole> ();
        public float lastGenerationTime;

        internal void UpdateRoles (Dictionary<int, PlayerRole> roles)
        {
            Roles.Clear ();
            foreach (var item in roles)
            {
                Roles.Add (item.Key, item.Value);
            }
        }

        internal int GetPlayerWithRole (PlayerRole role)
        {
            foreach (var item in Roles)
            {
                if (item.Value == role)
                {
                    return item.Key;
                }
            }

            return -1;
        }

        internal PlayerRole NextFreeRole ()
        {
            var values = Enum.GetValues (typeof (PlayerRole)) as PlayerRole[];
            for (int j = 1; j < values.Length; j++)
            {
                if (GetPlayerWithRole ((PlayerRole) values[j]) == -1)
                {
                    return (PlayerRole) values[j];
                }
            }
            UnityEngine.Debug.LogError ("Can't find free role");
            return PlayerRole.None;
        }
    }
}
