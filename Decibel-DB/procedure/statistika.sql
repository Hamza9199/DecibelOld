SELECT COUNT(*) AS Broj_Reprodukcija
FROM HistorijaSlusanja
WHERE pjesmaID = 3
  AND trajanje >= 30;

CREATE TRIGGER UpdateStatistikaPjesme
    ON HistorijaSlusanja
    AFTER INSERT
    AS
BEGIN
    UPDATE sp
    SET sp.brojReprodukcija = sp.brojReprodukcija + 1,
        sp.ukupnoTrajanje   = sp.ukupnoTrajanje + i.trajanje
    FROM StatistikaReprodukcije sp
             INNER JOIN inserted i ON sp.PjesmaID = i.PjesmaID;

    INSERT INTO StatistikaReprodukcije (pjesmaID, brojReprodukcija, ukupnoTrajanje, zadnjePustanje, korisnikID)
    SELECT i.PjesmaID, 1, i.trajanje, i.kreiranDatumVrijeme, i.korisnikID
    FROM inserted i
    WHERE NOT EXISTS (SELECT 1
                      FROM StatistikaReprodukcije sp
                      WHERE sp.PjesmaID = i.PjesmaID);
END;
GO
SET NOCOUNT OFF;

MERGE StatistikaReprodukcije AS target
USING (SELECT hs.korisnikID,
              hs.pjesmaID,
              COUNT(*)                 AS broj_reprodukcija,
              SUM(trajanje)            AS ukupno_trajanje,
              MAX(kreiranDatumVrijeme) AS zadnje_pustanje
       FROM HistorijaSlusanja as hs
       WHERE trajanje >= 30
       GROUP BY hs.korisnikID, hs.pjesmaID) AS source
ON target.korisnikID = source.korisnikID
    AND target.pjesmaID = source.pjesmaID
WHEN MATCHED THEN
    UPDATE
    SET target.brojReprodukcija = source.broj_reprodukcija,
        target.ukupnoTrajanje   = source.ukupno_trajanje,
        target.zadnjePustanje   = source.zadnje_pustanje
WHEN NOT MATCHED BY TARGET THEN
    INSERT (korisnikID, pjesmaID, brojReprodukcija, ukupnoTrajanje, zadnjePustanje)
    VALUES (source.korisnikID, source.pjesmaID, source.broj_reprodukcija, source.ukupno_trajanje, source.zadnje_pustanje);