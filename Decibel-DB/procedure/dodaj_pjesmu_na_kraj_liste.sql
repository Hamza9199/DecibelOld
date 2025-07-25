CREATE PROCEDURE dodaj_pjesmu_na_kraj_liste @pjesmaID INT,
                                            @playlistaID INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @velicinaListe INT;

    EXEC @velicinaListe = dbo.prebroj_pjesme @playlistaID = @playlistaID;

    DECLARE @prvaPjesmaID INT;

    EXEC @prvaPjesmaID = dbo.pronadji_prvu_pjesmu @playlistaID = @playlistaID;

    DECLARE @zadnjaPjesmaID INT;

    EXEC @zadnjaPjesmaID = dbo.pronadji_zadnju_pjesmu @playlistaID = @playlistaID, @prvaPjesmaID = @prvaPjesmaID;

    IF @velicinaListe = 0
        BEGIN
            INSERT INTO PjesmaPlayLista (pjesmaID, playlistaID, pokazivacNaSljedecuPjesmuID, pokazivacNaPrethodnuPjesmuID)
            VALUES (@pjesmaID, @playlistaID, @pjesmaID, NULL);
        END
    ELSE
        BEGIN
            INSERT INTO PjesmaPlayLista (pjesmaID, playlistaID, pokazivacNaSljedecuPjesmuID, pokazivacNaPrethodnuPjesmuID)
            VALUES (@pjesmaID, @playlistaID, @PrvaPjesmaID, @ZadnjaPjesmaID);

            UPDATE
                PjesmaPlayLista
            SET pokazivacNaSljedecuPjesmuID = @pjesmaID
            WHERE pjesmaID = @ZadnjaPjesmaID;
        END

    SET NOCOUNT OFF;
END;
go

