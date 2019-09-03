using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

interface IPlayerInput
{
    bool GetLeftKey();
    bool GetLeftKeyDown();
    bool GetRightKey();
    bool GetRightKeyDown();
    bool GetUpKey();
    bool GetUpKeyDown();
}