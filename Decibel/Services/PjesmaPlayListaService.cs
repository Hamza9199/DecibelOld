using Decibel.Data;
using Decibel.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Decibel.Services
{
        public class PjesmaPlayListaService
        {
                private readonly ApplicationDbContext _context;

                public PjesmaPlayListaService(ApplicationDbContext context)
                {
                        _context = context;
                }

                public async Task<List<RedoslijedPjesama>> GetPlaylistOrderAsync(int playlistaID)
                {
                        return _context.RedoslijedPjesama
                                .FromSqlRaw("EXEC get_redoslijed_pjesama @playlistaID", new SqlParameter("@playlistaID", playlistaID))
                                .ToList();
                }

                public async Task DodajPjesmuNaKrajListe(int pjesmaID, int playlistaID)
                {
                        await _context.Database
                               .ExecuteSqlInterpolatedAsync($"EXEC dodaj_pjesmu_na_kraj_liste @pjesmaID = {pjesmaID} , @playlistaID = {playlistaID}");
                }

                public async Task ObrisiPjesmu(int pjesmaID, int playlistaID)
                {
                        await _context.Database
                                .ExecuteSqlInterpolatedAsync($"EXEC obrisi_pjesmu @playlistaID = {playlistaID}, @pjesmaID = {pjesmaID};");
                }

                public async Task PromjeniRedoslijed(int pjesmaID, int pocetnaPjesmaID, int playlistaID)
                {
                        await _context.Database
                                .ExecuteSqlInterpolatedAsync(
                                        $"EXEC promjeni_redoslijed @pjesmaID = {pjesmaID}, @pocetnaPjesmaID = {pocetnaPjesmaID}, @playlistaID = {playlistaID}");
                }

                public async Task PremjestiNaPocetak(int pjesmaID, int playlistaID)
                {
                        await _context.Database
                                .ExecuteSqlInterpolatedAsync(
                                        $"EXEC premjesti_na_pocetak @pjesmaID = {pjesmaID}, @playlistaID = {playlistaID}");
                }
        }
}