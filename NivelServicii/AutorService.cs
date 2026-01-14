using LibrarieModele;
using Microsoft.Build.Utilities;
using NivelAccessDate;
using NivelServicii.Cache;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Logger = NLog.Logger;

namespace NivelServicii
{
     public class AutorService : IAutorService
     {
        private readonly AutorAccessor autorAccessor = new AutorAccessor();
        public static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly ICache cache;
        public AutorService(AutorAccessor autorAccessor, ICache cache)
            {
                this.autorAccessor = autorAccessor;
                this.cache = cache;
            }


            public List<Autor> GetAll()
            {
                string cacheKey = "autori_all";
                if (cache.IsSet(cacheKey))
                {
                logger.Info("AUTORI : date luate din Cache");    
                return cache.Get<List<Autor>>(cacheKey);
                }
            logger.Info("AUTORI : date luate din BD");

            var autori = autorAccessor.GetAll()
                            .Where(a => !a.IsDeleted) 
                            .ToList();
                cache.Set(cacheKey, autori); 
                return autori;
            }

            public Autor GetById(int id)
            {
                string key = $"autor_{id}";
                if (cache.IsSet(key))
                {
                    return cache.Get<Autor>(key);
                }
                var autor = autorAccessor.GetById(id);
                
                cache.Set(key, autor); 
                    return autor;
            }

        public void Add(Autor autor)
        {
            autorAccessor.Add(autor);
            cache.Remove("autori_all");
        }

           public void Update(Autor autor)
        {
                           autorAccessor.Update(autor);
                cache.RemoveByPattern("autor");
                cache.Remove("autori_all");
        }

            public void Delete(int id)
        {
                autorAccessor.Delete(id);
                cache.RemoveByPattern("autor");
                cache.Remove("autori_all");
        }

            public void LogicalDelete(int id)
            {
                var autor = autorAccessor.GetById(id);
                if (autor != null)
                {
                    autor.IsDeleted = true;
                    autorAccessor.Update(autor); 
                }
            }
        }
    
}
