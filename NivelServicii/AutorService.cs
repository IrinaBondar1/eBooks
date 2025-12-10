using LibrarieModele;

using NivelAccessDate;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NivelServicii
{
        public class AutorService : IAutorService
        {
            private readonly AutorAccessor autorAccessor = new AutorAccessor();

            public List<Autor> GetAll()
            {
                return autorAccessor.GetAll()
                        .Where(a => !a.IsDeleted) 
                        .ToList();
            }

            public Autor GetById(int id) => autorAccessor.GetById(id);

            public void Add(Autor autor) => autorAccessor.Add(autor);

            public void Update(Autor autor) => autorAccessor.Update(autor);

            public void Delete(int id) => autorAccessor.Delete(id); 

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
