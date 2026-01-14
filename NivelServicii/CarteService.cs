using LibrarieModele;

using NivelAccessDate;
using NivelServicii.Cache;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NivelServicii
{
    public class CarteService : ICarteService
    {
        private readonly CarteAccessor carteAccessor;
        private readonly ICache cache;
        public CarteService(CarteAccessor carteAccessor, ICache cache)
        {
            this.carteAccessor = carteAccessor;
            this.cache = cache;
        }

        public List<Carte> GetAll()
        {
           string key = "carti_all";
          if (cache.IsSet(key))
          {
             return cache.Get<List<Carte>>(key);
          }
          var carti = carteAccessor.GetAll();
          cache.Set(key, carti);
          return carti;
        }

        public Carte GetById(int id)
        {
            string key = $"carte_{id}";
            if (cache.IsSet(key))
            {
                return cache.Get<Carte>(key);
            }
            var carte = carteAccessor.GetById(id);
            cache.Set(key, carte);
            return carte;
        }

        public void Add(Carte carte)
        {
           carteAccessor.Add(carte);
            cache.RemoveByPattern("carti");
            cache.Remove("carti_all");
        }

        public void Update(Carte carte)
        {
            carteAccessor.Update(carte);
            cache.RemoveByPattern("carte");
            cache.Remove("carti_all");
        }

        public void Delete(int id)
        {
            carteAccessor.Delete(id);
            cache.RemoveByPattern("carte");
            cache.Remove("carti_all");
        }
    }
}
