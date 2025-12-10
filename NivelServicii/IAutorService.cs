using LibrarieModele;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NivelServicii
{
    public interface IAutorService
    {
        List<Autor> GetAll();
        Autor GetById(int id);
        void Add(Autor autor);
        void Update(Autor autor);
        void Delete(int id); 
        void LogicalDelete(int id); 
    }
}
