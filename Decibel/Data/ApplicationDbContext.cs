using Decibel.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using Microsoft.Data.SqlClient;

namespace Decibel.Data
{
        public class ApplicationDbContext : IdentityDbContext
        {
                public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                        : base(options)
                {
                }

                public DbSet<Korisnik> Korisnik { get; set; }

                public DbSet<Album> Album { get; set; }

                public DbSet<Komentar> Komentar { get; set; }

                public DbSet<Pjesma> Pjesma { get; set; }

                public DbSet<PlayLista> PlayLista { get; set; }

                public DbSet<Pretplata> Pretplata { get; set; }

                public DbSet<PjesmaPlayLista> PjesmaPlaylista { get; set; }

                public DbSet<KorisnikPjesma> KorisnikPjesma { get; set; }

                public DbSet<Zanr> Zanr { get; set; }

                public DbSet<PjesmaZanr> PjesmaZanr { get; set; }

                public DbSet<IzvodjacPjesma> IzvodjacPjesma { get; set; }

                public DbSet<HistorijaSlusanja> HistorijaSlusanja { get; set; }

                public DbSet<KorisnikPlayLista> KorisnikPlayLista { get; set; }

                public DbSet<KorisnikAlbum> KorisnikAlbum { get; set; }

                public DbSet<PratilacKorisnik> Pratilac { get; set; }

                public DbSet<KorisnikPretplata> KorisnikPretplata { get; set; }

                public DbSet<ObnovaPretplate> ObnovaPretplate { get; set; }

                public DbSet<StatistikaReprodukcije> StatistikaReprodukcije { get; set; }

                public DbSet<RedoslijedPjesama> RedoslijedPjesama { get; set; }

                protected override void OnModelCreating(ModelBuilder builder)
                {
                        builder.Entity<Korisnik>().ToTable(nameof(Korisnik));
                        builder.Entity<Album>().ToTable(nameof(Album));
                        builder.Entity<Komentar>().ToTable(nameof(Komentar));
                        builder.Entity<Pjesma>().ToTable(nameof(Pjesma));
                        builder.Entity<PlayLista>().ToTable(nameof(PlayLista));
                        builder.Entity<Pretplata>().ToTable(nameof(Pretplata));
                        builder.Entity<PjesmaPlayLista>().ToTable(nameof(PjesmaPlayLista));
                        builder.Entity<KorisnikPjesma>().ToTable(nameof(KorisnikPjesma));
                        builder.Entity<Zanr>().ToTable(nameof(Zanr));
                        builder.Entity<PjesmaZanr>().ToTable(nameof(PjesmaZanr));
                        builder.Entity<IzvodjacPjesma>().ToTable(nameof(IzvodjacPjesma));
                        builder.Entity<HistorijaSlusanja>()
                                 .ToTable(tb => tb.UseSqlOutputClause(false));
                        builder.Entity<KorisnikPlayLista>().ToTable(nameof(KorisnikPlayLista));
                        builder.Entity<KorisnikAlbum>().ToTable(nameof(KorisnikAlbum));
                        builder.Entity<PratilacKorisnik>().ToTable(nameof(PratilacKorisnik));
                        builder.Entity<KorisnikPretplata>().ToTable(nameof(KorisnikPretplata));
                        builder.Entity<ObnovaPretplate>().ToTable(nameof(ObnovaPretplate));
                        builder.Entity<StatistikaReprodukcije>().ToTable(nameof(StatistikaReprodukcije));




                        builder.Entity<Korisnik>()
                                .HasOne(c => c.AspNetUser).WithOne()
                                .HasForeignKey<Korisnik>(c => c.ID)
                                .IsRequired();

                        //---------------------- Povezana Lista --------------------------//
                        builder.Entity<PjesmaPlayLista>()
                            .HasOne(ppl => ppl.sljedecaPjesma)
                            .WithMany()
                            .HasForeignKey(ppl => ppl.pokazivacNaSljedecuPjesmuID)
                            .OnDelete(DeleteBehavior.Restrict);

                        builder.Entity<PjesmaPlayLista>()
                            .HasOne(ppl => ppl.prethodnaPjesma)
                            .WithMany()
                            .HasForeignKey(ppl => ppl.pokazivacNaPrethodnuPjesmuID)
                            .OnDelete(DeleteBehavior.Restrict);


                        //----------------------- Default DateTime -------------------------//
                        builder.Entity<Album>()
                                .Property(z => z.kreiranDatumVrijeme)
                                .HasDefaultValueSql("GETUTCDATE()");

                        builder.Entity<IzvodjacPjesma>()
                                .Property(z => z.kreiranDatumVrijeme)
                                .HasDefaultValueSql("GETUTCDATE()");

                        builder.Entity<HistorijaSlusanja>()
                                .Property(z => z.kreiranDatumVrijeme)
                                .HasDefaultValueSql("GETUTCDATE()");


                        builder.Entity<Komentar>()
                                .Property(z => z.kreiranDatumVrijeme)
                                .HasDefaultValueSql("GETUTCDATE()");

                        builder.Entity<KorisnikAlbum>()
                                .Property(z => z.kreiranDatumVrijeme)
                                .HasDefaultValueSql("GETUTCDATE()");

                        builder.Entity<KorisnikPjesma>()
                                .Property(z => z.kreiranDatumVrijeme)
                                .HasDefaultValueSql("GETUTCDATE()");

                        builder.Entity<KorisnikPlayLista>()
                                .Property(z => z.kreiranDatumVrijeme)
                                .HasDefaultValueSql("GETUTCDATE()");

                        builder.Entity<KorisnikPretplata>()
                                .Property(z => z.kreiranDatumVrijeme)
                                .HasDefaultValueSql("GETUTCDATE()");

                        builder.Entity<ObnovaPretplate>()
                                .Property(z => z.kreiranDatumVrijeme)
                                .HasDefaultValueSql("GETUTCDATE()");

                        builder.Entity<Pjesma>()
                                .Property(z => z.kreiranDatumVrijeme)
                                .HasDefaultValueSql("GETUTCDATE()");

                        builder.Entity<PjesmaPlayLista>()
                                .Property(z => z.kreiranDatumVrijeme)
                                .HasDefaultValueSql("GETUTCDATE()");

                        builder.Entity<PjesmaZanr>()
                                .Property(z => z.kreiranDatumVrijeme)
                                .HasDefaultValueSql("GETUTCDATE()");

                        builder.Entity<PlayLista>()
                                .Property(z => z.kreiranDatumVrijeme)
                                .HasDefaultValueSql("GETUTCDATE()");

                        builder.Entity<PratilacKorisnik>()
                                .Property(z => z.kreiranDatumVrijeme)
                                .HasDefaultValueSql("GETUTCDATE()");

                        builder.Entity<Pretplata>()
                                .Property(z => z.kreiranDatumVrijeme)
                                .HasDefaultValueSql("GETUTCDATE()");

                        builder.Entity<Zanr>()
                                .Property(z => z.kreiranDatumVrijeme)
                                .HasDefaultValueSql("GETUTCDATE()");

                        //--------------------- Broj Reprodukcija, Lajkova, Pratilaca ------------------------//
                        builder.Entity<Pjesma>()
                                .Property(p => p.brojReprodukcija)
                                .HasDefaultValue(0);

                        builder.Entity<Pjesma>()
                                .Property(p => p.brojLajkova)
                                .HasDefaultValue(0);

                        builder.Entity<PlayLista>()
                                .Property(p => p.brojLajkova)
                                .HasDefaultValue(0);

                        builder.Entity<PlayLista>()
                                .Property(p => p.javno)
                                .HasDefaultValue(0);

                        builder.Entity<Album>()
                                .Property(p => p.brojLajkova)
                                .HasDefaultValue(0);

                        builder.Entity<Album>()
                                .Property(p => p.javno)
                                .HasDefaultValue(0);

                        builder.Entity<Pjesma>()
                                .HasOne(p => p.Korisnik)
                                .WithMany(k => k.Pjesma)
                                .HasForeignKey(p => p.korisnikID)
                                .OnDelete(DeleteBehavior.Restrict);


                        builder.Entity<Korisnik>()
                                .Property(p => p.brojPratilaca)
                                .HasDefaultValue(0);

                        builder.Entity<Korisnik>()
                                .Property(p => p.obrisan)
                                .HasDefaultValue(0);

                        builder.Entity<Korisnik>()
                                .Property(p => p.statusKorisnika)
                                .HasDefaultValue(KorisnikStatusEnum.Aktivan);

                        builder.Entity<Korisnik>()
                                .Property(z => z.datumRegistracije)
                                .HasDefaultValueSql("GETUTCDATE()");


                        builder.Entity<Pjesma>()
                                .Property(p => p.odobreno)
                                .HasDefaultValue(0);

                        builder.Entity<Pjesma>()
                                .Property(p => p.javno)
                                .HasDefaultValue(0);

                        base.OnModelCreating(builder);
                }
        }
}
