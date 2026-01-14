using LibrarieModele;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NivelServicii
{
    public interface ICarteService
    {
        List<Carte> GetAll();
        Carte GetById(int id);
        void Add(Carte carte);
        void Update(Carte carte);
        void Delete(int id);
        void SoftDelete(int id);
    }
}
