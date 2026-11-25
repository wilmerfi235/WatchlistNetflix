using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces
{
    internal class IApplicationDbContext
    {
        /*
         * 
         * En este diseño utilizaremos interfaces de repositorio para cada agregado y para las consultas del catálogo. 
         * Por tanto, no necesitamos introducir IApplicationDbContext como una abstracción adicional de EF Core.

         * Así mantenemos una separación clara entre el dominio, los casos de uso y la persistencia.
         * 
         */
    }
}
