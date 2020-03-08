using DeskBooker.Core.Domain;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DeskBooker.Core.DataRepository
{
    public interface IDeskRepository
    {
        IEnumerable<Desk> GetAvailableDesk(DateTime dateTime);
    }
}
