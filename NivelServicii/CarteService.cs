using LibrarieModele;

using NivelAccessDate;
using NivelServicii.Cache;
using NLog;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Logger = NLog.Logger;

namespace NivelServicii
{
    public class CarteService : ICarteService
    {
        private readonly CarteAccessor carteAccessor;
        private readonly ICache cache;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

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
             logger.Info("CARTI : date luate din Cache");
             return cache.Get<List<Carte>>(key);
          }
          logger.Info("CARTI : date luate din BD");
          var carti = carteAccessor.GetAll();
          cache.Set(key, carti);
          return carti;
        }

        public Carte GetById(int id)
        {
            string key = $"carte_{id}";
            if (cache.IsSet(key))
            {
                logger.Debug($"CARTE {id} : date luate din Cache");
                return cache.Get<Carte>(key);
            }
            logger.Debug($"CARTE {id} : date luate din BD");
            var carte = carteAccessor.GetById(id);
            if (carte != null)
            {
                cache.Set(key, carte);
            }
            return carte;
        }

        public void Add(Carte carte)
        {
           logger.Info($"CARTE : Adaugare carte noua - {carte.titlu}");
           carteAccessor.Add(carte);
            cache.RemoveByPattern("carti");
            cache.Remove("carti_all");
            logger.Info($"CARTE : Cache invalidat dupa adaugare");
        }

        public void Update(Carte carte)
        {
            logger.Info($"CARTE {carte.id_carte} : Actualizare carte - {carte.titlu}");
            carteAccessor.Update(carte);
            cache.RemoveByPattern("carte");
            cache.Remove("carti_all");
            logger.Info($"CARTE {carte.id_carte} : Cache invalidat dupa actualizare");
        }

        public void Delete(int id)
        {
            carteAccessor.Delete(id);
            cache.RemoveByPattern("carte");
            cache.Remove("carti_all");
        }

        public void SoftDelete(int id)
        {
            logger.Info($"CARTE {id} : Stergere logica (SoftDelete)");
            carteAccessor.SoftDelete(id);
            cache.RemoveByPattern("carte");
            cache.Remove("carti_all");
            logger.Info($"CARTE {id} : Cache invalidat dupa stergere logica");
        }
    }
}
