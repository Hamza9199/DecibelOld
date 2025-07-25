CREATE PROCEDURE premjesti_na_pocetak @playlistaID INT,
                                      @pjesmaID INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @brojPjesama INT;

    EXEC @brojPjesama = dbo.prebroj_pjesme @playlistaID = @playlistaID;

    IF @brojPjesama <= 1
        RETURN;

    DECLARE @prvaPjesmaID INT;

    EXEC @prvaPjesmaID = dbo.pronadji_prvu_pjesmu @playlistaID = @playlistaID;

    IF @prvaPjesmaID = @pjesmaID
        RETURN;

    DECLARE @postoji INT;

    EXEC @postoji = dbo.postoji_pjesma_u_playlisti @playlistaID = @playlistaID, @pjesmaID = @pjesmaID;

    IF @postoji = -1
        RETURN;

    DECLARE @ogPrethodnikID INT, @ogSljedbenikID INT, @ogZadnjaID INT;

    EXEC @ogZadnjaID = dbo.pronadji_zadnju_pjesmu @playlistaID = @playlistaID, @prvaPjesmaID = @prvaPjesmaID

    SELECT @ogPrethodnikID = pokazivacNaPrethodnuPjesmuID,
           @ogSljedbenikID = pokazivacNaSljedecuPjesmuID
    FROM PjesmaPlayLista
    WHERE playlistaID = @playlistaID
      AND pjesmaID = @pjesmaID;


    UPDATE
        PjesmaPlayLista
    SET pokazivacNaSljedecuPjesmuID  = @prvaPjesmaID,
        pokazivacNaPrethodnuPjesmuID = NULL
    WHERE playlistaID = @playlistaID
      AND pjesmaID = @pjesmaID;


    UPDATE
        PjesmaPlayLista
    SET pokazivacNaPrethodnuPjesmuID = @pjesmaID
    WHERE playlistaID = @playlistaID
      AND pjesmaID = @prvaPjesmaID;


    IF @ogSljedbenikID != @prvaPjesmaID
        BEGIN
            UPDATE
                PjesmaPlayLista
            SET pokazivacNaSljedecuPjesmuID = @ogSljedbenikID
            WHERE playlistaID = @playlistaID
              AND pjesmaID = @ogPrethodnikID;

            UPDATE
                PjesmaPlayLista
            SET pokazivacNaPrethodnuPjesmuID = @ogPrethodnikID
            WHERE playlistaID = @playlistaID
              AND pjesmaID = @ogSljedbenikID;
        END;
    ELSE
        BEGIN
            UPDATE
                PjesmaPlayLista
            SET pokazivacNaPrethodnuPjesmuID = @pjesmaID
            WHERE playlistaID = @playlistaID
              AND pjesmaID = @ogSljedbenikID;
        END;

    IF @ogZadnjaID != @pjesmaID
        BEGIN
            UPDATE
                PjesmaPlayLista
            SET pokazivacNaSljedecuPjesmuID = @pjesmaID
            WHERE playlistaID = @playlistaID
              AND pjesmaID = @ogZadnjaID;
        END;

    IF @brojPjesama = 2
        BEGIN
            UPDATE
                PjesmaPlayLista
            SET pokazivacNaPrethodnuPjesmuID = @pjesmaID
            WHERE playlistaID = @playlistaID
              AND pjesmaID = @prvaPjesmaID

            UPDATE
                PjesmaPlayLista
            SET pokazivacNaPrethodnuPjesmuID = NULL
            WHERE playlistaID = @playlistaID
              AND pjesmaID = @pjesmaID
        END

    SET NOCOUNT OFF;
END;
go

