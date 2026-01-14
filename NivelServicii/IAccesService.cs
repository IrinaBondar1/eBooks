using LibrarieModele;

namespace NivelServicii
{
    public interface IAccesService
    {
        bool PoateAccesaCarte(Utilizator utilizator, Carte carte);
        bool PoateAccesaSerieCompleta(Utilizator utilizator);
        bool PoateDescarca(Utilizator utilizator);
        bool PoateCitireInca(Utilizator utilizator);
        bool InregistrareAccesCarte(Utilizator utilizator, Carte carte, string actiune = "citire");
    }
}
