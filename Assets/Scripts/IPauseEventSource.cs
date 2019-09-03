using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal interface IPauseEventSource
{
    event Action OnPause;
    event Action OnUnpause;
}
