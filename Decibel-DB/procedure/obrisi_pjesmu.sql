CREATE PROCEDURE obrisi_pjesmu @playlistaID INT,
                               @pjesmaID INT
AS
BEGIN
    SET
        NOCOUNT ON;

    DECLARE
        @postoji INT;
    EXEC @postoji = dbo.postoji_pjesma_u_playlisti @playlistaID = @playlistaID, @pjesmaID = @pjesmaID;

    IF
        @postoji = -1
        RETURN;

    DECLARE
        @prethodnaPjesmaID INT, @sljedecaPjesmaID INT, @prvaPjesmaID INT;

    EXEC @prvaPjesmaID = dbo.pronadji_prvu_pjesmu @playlistaID = @playlistaID;

    SELECT @prethodnaPjesmaID = pokazivacNaPrethodnuPjesmuID,
           @sljedecaPjesmaID = pokazivacNaSljedecuPjesmuID
    FROM PjesmaPlayLista
    WHERE playlistaID = @playlistaID
      AND pjesmaID = @pjesmaID;

    IF
        @prethodnaPjesmaID IS NULL
        BEGIN
            UPDATE PjesmaPlayLista
            SET pokazivacNaPrethodnuPjesmuID = NULL
            WHERE playlistaID = @playlistaID
              AND pjesmaID = @sljedecaPjesmaID;

            DECLARE
                @zadnjaPjesmaID INT;
            SELECT @zadnjaPjesmaID = pjesmaID
            FROM PjesmaPlayLista
            WHERE playlistaID = @playlistaID
              AND pokazivacNaSljedecuPjesmuID IS NULL;

            UPDATE PjesmaPlayLista
            SET pokazivacNaSljedecuPjesmuID = @sljedecaPjesmaID
            WHERE playlistaID = @playlistaID
              AND pjesmaID = @zadnjaPjesmaID;
        END
    ELSE
        IF @sljedecaPjesmaID = @prvaPjesmaID
            BEGIN
                UPDATE PjesmaPlayLista
                SET pokazivacNaSljedecuPjesmuID = NULL
                WHERE playlistaID = @playlistaID
                  AND pjesmaID = @prethodnaPjesmaID;

                UPDATE PjesmaPlayLista
                SET pokazivacNaSljedecuPjesmuID = @prvaPjesmaID
                WHERE playlistaID = @playlistaID
                  AND pjesmaID = @prethodnaPjesmaID;
            END
        ELSE
            BEGIN
                UPDATE PjesmaPlayLista
                SET pokazivacNaSljedecuPjesmuID = @sljedecaPjesmaID
                WHERE playlistaID = @playlistaID
                  AND pjesmaID = @prethodnaPjesmaID;

                UPDATE PjesmaPlayLista
                SET pokazivacNaPrethodnuPjesmuID = @prethodnaPjesmaID
                WHERE playlistaID = @playlistaID
                  AND pjesmaID = @sljedecaPjesmaID;
            END;

    DELETE
    FROM PjesmaPlayLista
    WHERE playlistaID = @playlistaID
      AND pjesmaID = @pjesmaID;

    SET
        NOCOUNT OFF;
END;
go