using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Traffic_Simulator
{
    /// <summary>
    /// Represents the directions.
    /// </summary>
    [Serializable]
    public enum Direction
    {
        North=0,
        East=1,
        South=2,
        West=3,
        Center=4,
    }
}
