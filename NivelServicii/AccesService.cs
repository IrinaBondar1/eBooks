using LibrarieModele;
using NivelAccessDate;
using NLog;
using Repository_CodeFirst;
using System;
using System.Data.Entity;
using System.Linq;

using Logger = NLog.Logger;

namespace NivelServicii
{
    public class AccesService : IAccesService
    {
        private readonly UtilizatorAccessor utilizatorAccessor;
        private readonly IstoricCitireAccessor istoricAccessor;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public AccesService(UtilizatorAccessor utilizatorAccessor, IstoricCitireAccessor istoricAccessor)
        {
            this.utilizatorAccessor = utilizatorAccessor;
            this.istoricAccessor = istoricAccessor;
        }

        public bool PoateAccesaCarte(Utilizator utilizator, Carte carte)
        {
            if (utilizator == null || carte == null || carte.IsDeleted)
            {
                logger.Warn($"ACCES : Verificare acces esuata - Utilizator: {utilizator?.id_utilizator}, Carte: {carte?.id_carte}, IsDeleted: {carte?.IsDeleted}");
                return false;
            }

            // Verifică dacă mai poate citi cărți în această lună
            if (!PoateCitireInca(utilizator))
            {
                logger.Info($"ACCES : Utilizator {utilizator.id_utilizator} a atins limita de carti pentru luna curenta");
                return false;
            }

            // Dacă carte face parte dintr-o serie
            if (carte.id_serie.HasValue && carte.nr_volum.HasValue)
            {
                var tipAbonament = utilizator.TipAbonament;
                
                // Free: doar primul volum
                if (tipAbonament != null && tipAbonament.denumire == "Free" && carte.nr_volum.Value > 1)
                {
                    logger.Info($"ACCES : Utilizator {utilizator.id_utilizator} (Free) nu poate accesa volumul {carte.nr_volum} din serie");
                    return false;
                }
                
                // Premium și VIP: acces la toate volumele dacă au acces_serii_complete
                if (tipAbonament != null && !tipAbonament.acces_serii_complete && carte.nr_volum.Value > 1)
                {
                    logger.Info($"ACCES : Utilizator {utilizator.id_utilizator} ({tipAbonament.denumire}) nu are acces la serii complete");
                    return false;
                }
            }

            logger.Debug($"ACCES : Utilizator {utilizator.id_utilizator} poate accesa carte {carte.id_carte}");
            return true;
        }

        public bool PoateAccesaSerieCompleta(Utilizator utilizator)
        {
            if (utilizator?.TipAbonament == null)
                return false;

            return utilizator.TipAbonament.acces_serii_complete;
        }

        public bool PoateDescarca(Utilizator utilizator)
        {
            if (utilizator?.TipAbonament == null)
                return false;

            return utilizator.TipAbonament.permite_descarcare;
        }

        public bool PoateCitireInca(Utilizator utilizator)
        {
            if (utilizator?.TipAbonament == null)
                return false;

            // VIP are acces nelimitat
            if (utilizator.TipAbonament.denumire == "VIP")
                return true;

            // Verifică limită pentru Free și Premium
            return utilizator.carti_citite_luna < utilizator.TipAbonament.limita_carti_pe_luna;
        }

        public bool InregistrareAccesCarte(Utilizator utilizator, Carte carte, string actiune = "citire")
        {
            if (utilizator == null || carte == null)
                return false;

            try
            {
                using (var ctx = new eBooksContext())
                {
                    // Inregistrează accesul în istoric
                    var istoric = new LibrarieModele.IstoricCitire
                    {
                        id_utilizator = utilizator.id_utilizator,
                        id_carte = carte.id_carte,
                        data_accesare = DateTime.Now,
                        actiune = actiune ?? "citire"
                    };

                    ctx.IstoricCitiri.Add(istoric);

                    // Incrementează contorul de cărți citite (doar pentru citire, doar pentru non-VIP)
                    if (actiune == "citire")
                    {
                        var utilizatorDb = ctx.Utilizatori
                            .Include("TipAbonament")
                            .FirstOrDefault(u => u.id_utilizator == utilizator.id_utilizator);
                            
                        if (utilizatorDb != null && utilizatorDb.TipAbonament != null && utilizatorDb.TipAbonament.denumire != "VIP")
                        {
                            utilizatorDb.carti_citite_luna++;
                        }
                    }

                    ctx.SaveChanges();
                    logger.Info($"ACCES : Inregistrat acces pentru Utilizator {utilizator.id_utilizator}, Carte {carte.id_carte}, Actiune: {actiune}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"ACCES : Eroare la inregistrare acces pentru Utilizator {utilizator?.id_utilizator}, Carte {carte?.id_carte}");
                return false;
            }
        }
    }
}
