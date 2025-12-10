using LibrarieModele;

using NivelAccessDate;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NivelServicii
{
    public class CarteService : ICarteService
    {
        private readonly CarteAccessor accessor;

        public CarteService(CarteAccessor accessor)
        {
            this.accessor = accessor;
        }

        public List<Carte> GetAll()
        {
            return accessor.GetAll();
        }

        public Carte GetById(int id)
        {
            return accessor.GetById(id);
        }

        public void Add(Carte carte)
        {
            accessor.Add(carte);
        }

        public void Update(Carte carte)
        {
            accessor.Update(carte);
        }

        public void Delete(int id)
        {
            accessor.Delete(id);
        }
    }
}
