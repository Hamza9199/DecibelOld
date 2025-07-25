CREATE PROCEDURE NEW_SHIFT_SHORT @playlistaID INT,
                                 @pocetnaPjesmaID INT,
                                 @pjesmaID INT AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @brojPjesama INT;

    EXEC @brojPjesama = dbo.prebroj_pjesme @playlistaID = @playlistaID;

    IF @brojPjesama <= 1 RETURN;

    DECLARE @prvaPjesmaID INT;

    EXEC @prvaPjesmaID = dbo.pronadji_prvu_pjesmu @playlistaID = @playlistaID;

    DECLARE @postoji_target INT, @postoji_pocetna INT;

    EXEC @postoji_target = dbo.postoji_pjesma_u_playlisti @playlistaID = @playlistaID, @pjesmaID = @pjesmaID;

    EXEC @postoji_pocetna = dbo.postoji_pjesma_u_playlisti @playlistaID = @playlistaID, @pjesmaID = @pocetnaPjesmaID;

    IF @postoji_target = -1 OR @postoji_pocetna = -1 RETURN;

    DECLARE @ogPrethodnikID INT, @ogSljedbenikID INT, @ogZadnjaID INT;

    EXEC @ogZadnjaID = dbo.pronadji_zadnju_pjesmu @playlistaID = @playlistaID, @prvaPjesmaID = @prvaPjesmaID

    SELECT @ogPrethodnikID = pokazivacNaPrethodnuPjesmuID, @ogSljedbenikID = pokazivacNaSljedecuPjesmuID FROM PjesmaPlayLista WHERE playlistaID = @playlistaID AND pjesmaID = @pjesmaID;

    DECLARE @sljedbenikID INT;

    SELECT @sljedbenikID = pokazivacNaSljedecuPjesmuID FROM PjesmaPlayLista WHERE playlistaID = @playlistaID AND pjesmaID = @pocetnaPjesmaID;

    -- Ako je nova dodana pjesma vec sljedbenik pocetnoj pjesmi
    IF @sljedbenikID = @pjesmaID AND @sljedbenikID != @prvaPjesmaID RETURN;

    IF @pocetnaPjesmaID = @pjesmaID RETURN;

    DECLARE @predzadnjaPjesmaID INT;

    SELECT
        @predzadnjaPjesmaID = @pjesmaID
    FROM PjesmaPlayLista
    WHERE playlistaID = @playlistaID AND pokazivacNaSljedecuPjesmuID = @ogZadnjaID;

    -- Ako je sljedbenik pocetne pjesme prva pjesma, to znaci da je pocetna pjesma zadnja pjesma,
    -- to je slucaj gdje se nova pjesma dodaje na kraj liste
    IF @sljedbenikID = @prvaPjesmaID
        BEGIN
            DECLARE @drugaPjesmaID INT;
            -- Pribavlja drugu pjesmu u playlisti
            SELECT @drugaPjesmaID = pokazivacNaSljedecuPjesmuID FROM PjesmaPlayLista WHERE playlistaID = @playlistaID AND pjesmaID = @prvaPjesmaID;

            -- Ako je trenutna prva pjesma u playlisti ta koju zelimo staviti na kraj
            IF @pjesmaID = @prvaPjesmaID
                BEGIN
                    EXEC dbo.premjesti_pokazivac_na_prethodnu_pjesmu @playlistaID = @playlistaID, @naPjesmuID = NULL, @gdjeJePjesmaID = @drugaPjesmaID;
                    EXEC dbo.premjesti_pokazivac_na_sljedecu_pjesmu @playlistaID = @playlistaID, @naPjesmuID = @drugaPjesmaID, @gdjeJePjesmaID = @pjesmaID;
                END;
            ELSE
                BEGIN
                    EXEC dbo.premjesti_pokazivac_na_sljedecu_pjesmu @playlistaID = @playlistaID, @naPjesmuID = @prvaPjesmaID, @gdjeJePjesmaID = @pjesmaID;
                    EXEC dbo.premjesti_pokazivac_na_sljedecu_pjesmu @playlistaID = @playlistaID, @naPjesmuID = @ogSljedbenikID, @gdjeJePjesmaID = @ogPrethodnikID;
                    EXEC dbo.premjesti_pokazivac_na_sljedecu_pjesmu @playlistaID = @playlistaID, @naPjesmuID = @pjesmaID, @gdjeJePjesmaID = @pocetnaPjesmaID;
                    EXEC dbo.premjesti_pokazivac_na_prethodnu_pjesmu @playlistaID = @playlistaID, @naPjesmuID = @ogPrethodnikID, @gdjeJePjesmaID = @ogSljedbenikID;
                END;
        END;
    ELSE
        -- Ako je pocetna pjesma prva u listi
        IF @pocetnaPjesmaID = @prvaPjesmaID
            BEGIN
                -- Ako je pjesma koju zelimo da promijenimo poziciju originalna zadjna u playlisti
                IF @ogZadnjaID = @pjesmaID
                    BEGIN
                        EXEC dbo.premjesti_pokazivac_na_sljedecu_pjesmu @playlistaID = @playlistaID, @naPjesmuID = @pjesmaID, @gdjeJePjesmaID = @pocetnaPjesmaID;
                        EXEC dbo.premjesti_pokazivac_na_prethodnu_pjesmu @playlistaID = @playlistaID, @naPjesmuID = @pocetnaPjesmaID, @gdjeJePjesmaID = @pjesmaID;
                        EXEC dbo.premjesti_pokazivac_na_sljedecu_pjesmu @playlistaID = @playlistaID, @naPjesmuID = @prvaPjesmaID, @gdjeJePjesmaID = @ogPrethodnikID;
                    END;
                ELSE
                    BEGIN
                        EXEC dbo.premjesti_pokazivac_na_sljedecu_pjesmu @playlistaID = @playlistaID, @naPjesmuID = @pjesmaID, @gdjeJePjesmaID = @prvaPjesmaID;
                        EXEC dbo.premjesti_pokazivac_na_prethodnu_pjesmu @playlistaID = @playlistaID, @naPjesmuID = @ogPrethodnikID, @gdjeJePjesmaID = @ogSljedbenikID;
                        EXEC dbo.premjesti_pokazivac_na_sljedecu_pjesmu @playlistaID = @playlistaID, @naPjesmuID = @ogSljedbenikID, @gdjeJePjesmaID = @ogPrethodnikID;
                        EXEC dbo.premjesti_pokazivac_na_prethodnu_pjesmu @playlistaID = @playlistaID, @naPjesmuID = @prvaPjesmaID, @gdjeJePjesmaID = @pjesmaID;
                    END;
            END;
        ELSE -- Pocetna pjesme je u sredini
            BEGIN
                IF @pjesmaID = @prvaPjesmaID
                    BEGIN
                        SELECT @drugaPjesmaID = pokazivacNaSljedecuPjesmuID FROM PjesmaPlayLista WHERE playlistaID = @playlistaID AND pjesmaID = @prvaPjesmaID;

                        EXEC dbo.premjesti_pokazivac_na_prethodnu_pjesmu @playlistaID = @playlistaID, @naPjesmuID = NULL, @gdjeJePjesmaID = @drugaPjesmaID;
                        EXEC dbo.premjesti_pokazivac_na_sljedecu_pjesmu @playlistaID = @playlistaID, @naPjesmuID = @drugaPjesmaID, @gdjeJePjesmaID = @ogZadnjaID;
                    END;
                ELSE
                    BEGIN
                        IF @pjesmaID = @ogZadnjaID
                            BEGIN
                                -- Zelimo prebaciti zadnju pjesmu negdje u sredinu


                                UPDATE PjesmaPlayLista SET pokazivacNaSljedecuPjesmuID = @prvaPjesmaID WHERE playlistaID = @playlistaID AND pokazivacNaSljedecuPjesmuID = @ogZadnjaID;

                            END;
                        ELSE -- Ako je pocetna pjesma && pjesma koju zelimo da premjestimo u sredini
                            BEGIN
                                EXEC dbo.premjesti_pokazivac_na_sljedecu_pjesmu @playlistaID = @playlistaID, @naPjesmuID = @ogSljedbenikID, @gdjeJePjesmaID = @ogPrethodnikID;
                                EXEC dbo.premjesti_pokazivac_na_prethodnu_pjesmu @playlistaID = @playlistaID, @naPjesmuID = @ogPrethodnikID, @gdjeJePjesmaID = @ogSljedbenikID;
                            END;
                    END;
                        EXEC dbo.premjesti_pokazivac_na_prethodnu_pjesmu @playlistaID = @playlistaID, @naPjesmuID = @pjesmaID, @gdjeJePjesmaID = @sljedbenikID;
                EXEC dbo.premjesti_pokazivac_na_sljedecu_pjesmu @playlistaID = @playlistaID, @naPjesmuID = @pjesmaID, @gdjeJePjesmaID = @pocetnaPjesmaID;
                EXEC dbo.premjesti_pokazivac_na_sljedecu_pjesmu @playlistaID = @playlistaID, @naPjesmuID = @sljedbenikID, @gdjeJePjesmaID = @pjesmaID;
            END;



    EXEC dbo.premjesti_pokazivac_na_prethodnu_pjesmu @playlistaID = @playlistaID, @naPjesmuID = @pocetnaPjesmaID, @gdjeJePjesmaID = @pjesmaID;

    SET NOCOUNT OFF;
END;
go