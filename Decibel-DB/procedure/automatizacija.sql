insert into AspNetRoles (id, name)
values (1, 'Regularni'),
       (2, 'Izvodjac'),
       (3, 'Administrator');
go


CREATE PROCEDURE get_redoslijed_pjesama @playlistaID INT
AS
BEGIN
    SET NOCOUNT ON;

    CREATE TABLE #redoslijed
    (
        redoslijedPjesama            INT IDENTITY (1, 1),
        pjesmaID                     INT,
        playlistaID                  INT,
        pokazivacNaSljedecuPjesmuID  INT,
        pokazivacNaPrethodnuPjesmuID INT,
        kreiranDatumVrijeme          DATETIME
    );

    INSERT INTO #redoslijed (pjesmaID, playlistaID, pokazivacNaSljedecuPjesmuID, pokazivacNaPrethodnuPjesmuID, kreiranDatumVrijeme)
    SELECT pjesmaID,
           playlistaID,
           pokazivacNaSljedecuPjesmuID,
           pokazivacNaPrethodnuPjesmuID,
           kreiranDatumVrijeme
    FROM PjesmaPlaylista
    WHERE playlistaID = @playlistaID
      AND pokazivacNaPrethodnuPjesmuID IS NULL;

    WHILE EXISTS
        (SELECT 1
         FROM PjesmaPlaylista p
                  LEFT JOIN
              #redoslijed op
              ON
                  p.pjesmaID = op.pjesmaID
         WHERE op.pjesmaID IS NULL
           AND p.playlistaID = @playlistaID)
        BEGIN
            INSERT INTO #redoslijed (pjesmaID, playlistaID, pokazivacNaSljedecuPjesmuID, pokazivacNaPrethodnuPjesmuID, kreiranDatumVrijeme)
            SELECT p.pjesmaID,
                   p.playlistaID,
                   p.pokazivacNaSljedecuPjesmuID,
                   p.pokazivacNaPrethodnuPjesmuID,
                   p.kreiranDatumVrijeme
            FROM PjesmaPlaylista p
                     INNER JOIN
                 #redoslijed op
                 ON
                     p.pokazivacNaPrethodnuPjesmuID = op.pjesmaID
            WHERE p.playlistaID = @playlistaID
              AND NOT EXISTS
                (SELECT 1
                 FROM #redoslijed o
                 WHERE o.pjesmaID = p.pjesmaID);
        END;

    SELECT *
    FROM #redoslijed
    ORDER BY redoslijedPjesama;

    DROP TABLE
        #redoslijed;

    SET NOCOUNT OFF;
END;
go


CREATE PROCEDURE prebroj_pjesme @playlistaID INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @broj INT;

    SELECT @broj = COUNT(pjesmaID)
    FROM PjesmaPlayLista
    WHERE playlistaID = @playlistaID

    SET NOCOUNT OFF;

    RETURN @broj;
END
go


CREATE PROCEDURE pronadji_prvu_pjesmu @playlistaID INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @prvaPjesma INT;

    SELECT @prvaPjesma = pjesmaID

    FROM PjesmaPlayLista
    WHERE playlistaID = @playlistaID
      AND pokazivacNaPrethodnuPjesmuID IS NULL;

    SET NOCOUNT ON;

    RETURN @prvaPjesma;
END
go


CREATE PROCEDURE pronadji_zadnju_pjesmu @playlistaID INT,
                                        @prvaPjesmaID INT
AS
BEGIN
    DECLARE @zadnjaPjesmaID INT;

    SELECT TOP 1 @zadnjaPjesmaID = pjesmaID
    FROM PjesmaPlayLista
    WHERE playlistaID = @playlistaID
      AND pokazivacNaSljedecuPjesmuID = @prvaPjesmaID;

    RETURN @zadnjaPjesmaID;
END;
go


CREATE PROCEDURE postoji_pjesma_u_playlisti
    @playlistaID INT,
    @pjesmaID INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @pjesma INT;

    SELECT
        @pjesma = COUNT(pjesmaID)
    FROM
        PjesmaPlayLista
    WHERE
        playlistaID = @playlistaID
      AND
        pjesmaID = @pjesmaID

    SET NOCOUNT OFF;

    IF @pjesma = 0
        RETURN -1;
    ELSE
        RETURN 1;
END;
go


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


CREATE PROCEDURE premjesti_pokazivac_na_prethodnu_pjesmu @playlistaID INT,
                                                         @naPjesmuID INT = NULL,
                                                         @gdjeJePjesmaID INT
AS
BEGIN
    UPDATE
        PjesmaPlayLista
    SET pokazivacNaPrethodnuPjesmuID = @naPjesmuID
    WHERE playlistaID = @playlistaID
      AND pjesmaID = @gdjeJePjesmaID;
END
go


CREATE PROCEDURE premjesti_pokazivac_na_sljedecu_pjesmu @playlistaID INT,
                                                        @naPjesmuID INT = NULL,
                                                        @gdjeJePjesmaID INT
AS
BEGIN
    UPDATE
        PjesmaPlayLista
    SET pokazivacNaSljedecuPjesmuID = @naPjesmuID
    WHERE playlistaID = @playlistaID
      AND pjesmaID = @gdjeJePjesmaID;
END
go


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


CREATE PROCEDURE promjeni_redoslijed @playlistaID INT,
                                     @pjesmaID INT,
                                     @pocetnaPjesmaID INT
AS
BEGIN
    --===========================================================================--
-- Provjere (Validacija)
    DECLARE @brojPjesama INT;

    EXEC @brojPjesama = dbo.prebroj_pjesme @playlistaID = @playlistaID;
-- Ako playlista sadrzi manje od dvije pjesme, onda je nepotrebno ovako premjestati pjesme
    IF @brojPjesama <= 1 RETURN;

    DECLARE @postoji_target INT, @postoji_pocetna INT;

    EXEC @postoji_target = dbo.postoji_pjesma_u_playlisti @playlistaID = @playlistaID, @pjesmaID = @pjesmaID;

    EXEC @postoji_pocetna = dbo.postoji_pjesma_u_playlisti @playlistaID = @playlistaID, @pjesmaID = @pocetnaPjesmaID;
-- Ako ne postoje target pjesma (pjesma koju zelimo premjesiti) ili pocetna pjesma RETURN
    IF @postoji_target = -1 OR @postoji_pocetna = -1 RETURN;

    --===========================================================================--
-- Bitne varijable radi pozicioniranja pjesama
    DECLARE
        @ogPrethodnikID INT, -- Originalni prethodnik target pjesme
        @ogSljedbenikID INT, -- Originalni sljebenik target pjesme
        @ogPrvaID INT, -- Originalna prva pjesma playliste
        @sljedbenikID INT, -- Sljedbenik prve pjesme u playlisti
        @ogZadnjaID INT; -- Originalna zadnja pjesma playliste

    EXEC @ogPrvaID = dbo.pronadji_prvu_pjesmu @playlistaID = @playlistaID;

    EXEC @ogZadnjaID = dbo.pronadji_zadnju_pjesmu @playlistaID = @playlistaID, @prvaPjesmaID = @ogPrvaID;

-- Postavlja sljedbenika originalne prve pjesme u playlisti
    SELECT @sljedbenikID = pokazivacNaSljedecuPjesmuID
    FROM PjesmaPlayLista
    WHERE playlistaID = @playlistaID
      AND pjesmaID = @pocetnaPjesmaID;

-- Postavlja originalne susjede target pjesme
    SELECT @ogPrethodnikID = pokazivacNaPrethodnuPjesmuID,
           @ogSljedbenikID = pokazivacNaSljedecuPjesmuID
    FROM PjesmaPlayLista
    WHERE playlistaID = @playlistaID
      AND pjesmaID = @pjesmaID;

    --===========================================================================--
-- Varijable koje sadrze promijenjene pokazivace u odnosu na slucajeve, one se mijenjaju
    DECLARE
        -- Odnose se na pokazivace target pjesme (pjesmaID)
        @novaSljedIDOdPjesmaID INT = NULL, -- @pjesmaID.pokazivacNaSljedecuPjesmuID
        @novaPretIDOdPjesmaID INT = NULL, -- @pjesmaID.pokazivacNaPrethodnuPjesmuID
-- Odnose se na pokazivace originalnih susjeda target pjesme
        @noviOgSljedID INT = NULL, -- @ogPrethodnikID.pokazivacNaSljedecuPjesmuID
        @noviOgPretID INT = NULL, -- @ogSljedbenikID.pokazivacNaPrethodnuPjesmuID
-- Odnose se na pokazivace novih susjeda target pjesme
        @noviSljedID INT = NULL, -- @pocetnaPjesmaID.pokazivacNaSljedecuPjesmuID
        @noviPretID INT = NULL;
    -- @sljedbenikID.pokazivacNaPrethodnuPjesmuID

--===========================================================================--
-- Default vrijednosti pokazivaca, tj, kada bi se izvrsile oparcije UPDATE, tabela bi ostala ista
    SELECT @novaSljedIDOdPjesmaID = pokazivacNaSljedecuPjesmuID,
           @novaPretIDOdPjesmaID = pokazivacNaPrethodnuPjesmuID
    FROM PjesmaPlayLista
    WHERE playlistaID = @playlistaID
      AND pjesmaID = @pjesmaID;

    SELECT @noviOgSljedID = pokazivacNaSljedecuPjesmuID
    FROM PjesmaPlayLista
    WHERE playlistaID = @playlistaID
      AND pjesmaID = @ogPrethodnikID;

    SELECT @noviOgPretID = pokazivacNaPrethodnuPjesmuID
    FROM PjesmaPlayLista
    WHERE playlistaID = @playlistaID
      AND pjesmaID = @ogSljedbenikID;

    SELECT @noviSljedID = pokazivacNaSljedecuPjesmuID
    FROM PjesmaPlayLista
    WHERE playlistaID = @playlistaID
      AND pjesmaID = @pocetnaPjesmaID;

    SELECT @noviPretID = pokazivacNaPrethodnuPjesmuID
    FROM PjesmaPlayLista
    WHERE playlistaID = @playlistaID
      AND pjesmaID = @sljedbenikID;

    --===========================================================================--
-- Logika promjene varijabli pokazivaca

-- Ako je sljedbenik pocetne pjesme prva pjesma, to znaci da je pocetna pjesma zadnja pjesma,
-- i da je slucaj gdje se nova pjesma dodaje na kraj liste
    IF @pocetnaPjesmaID = @ogZadnjaID
        BEGIN
            DECLARE @drugaPjesmaID INT;
            -- Pribavlja originalnu drugu pjesmu u playlisti
            SELECT @drugaPjesmaID = pokazivacNaSljedecuPjesmuID
            FROM PjesmaPlayLista
            WHERE playlistaID = @playlistaID
              AND pjesmaID = @ogPrvaID;

            -- Ako je trenutna prva pjesma u playlisti ta koju zelimo staviti na kraj
            IF @pjesmaID = @ogPrvaID
                BEGIN
                    select 'in here 1';
                    SELECT @noviOgPretID = NULL,
                           @noviPretID = @pocetnaPjesmaID;
                END;
            ELSE
                BEGIN
                    select 'in here 2';
                    SELECT @novaSljedIDOdPjesmaID = @ogPrvaID,
                           @noviOgSljedID = @ogSljedbenikID,
                           @noviOgPretID = @ogPrethodnikID,
                           @noviSljedID = @pjesmaID;
                END;
            SELECT @novaPretIDOdPjesmaID = @pocetnaPjesmaID
        END;
    ELSE -- Ako pocetna nije zadnja
        IF @pocetnaPjesmaID = @ogPrvaID -- Ako pocetna nije zadnja + Ako je pocetna pjesma prva u playlisti
            BEGIN
                IF @ogZadnjaID = @pjesmaID -- Ako pocetna nije zadnja + Ako je pocetna pjesma prvu u playlisti + ako je target pjesma zadnja
                    BEGIN
                        SELECT 'in here 3';

                        SELECT @novaSljedIDOdPjesmaID = @sljedbenikID,
                               @novaPretIDOdPjesmaID = @pocetnaPjesmaID,
                               @noviSljedID = @pjesmaID,
                               @noviPretID = @pjesmaID,
                               @noviOgSljedID = @ogPrvaID;
                    END;
                ELSE -- Ako pocetna nije zadnja + Ako je pocetna pjesma prvu u playlisti + ako je target pjesma u sredini
                    BEGIN
                        SELECT 'in here 4';

                        SELECT @novaSljedIDOdPjesmaID = @sljedbenikID,
                               @novaPretIDOdPjesmaID = @ogPrvaID,
                               @noviOgPretID = @ogPrethodnikID,
                               @noviOgSljedID = @ogSljedbenikID,
                               @noviPretID = @pjesmaID,
                               @noviSljedID = @pjesmaID;
                    END;
            END;
        ELSE -- Ako pocetna nije zadnja + Pocetna pjesme je u sredini
            BEGIN
                IF @pjesmaID = @ogPrvaID -- Ako pocetna nije zadnja + Pocetna pjesme je u sredini + ako je target pjesma prva pjesma u playlisti
                    BEGIN
                        SELECT 'in here 5';

                        SELECT @novaSljedIDOdPjesmaID = @sljedbenikID,
                               @novaPretIDOdPjesmaID = @pocetnaPjesmaID,
                               @noviPretID = @pjesmaID,
                               @noviSljedID = @pjesmaID,
                               @noviOgPretID = NULL;

                        UPDATE
                            PjesmaPlayLista
                        SET pokazivacNaSljedecuPjesmuID = @ogSljedbenikID
                        WHERE playlistaID = @playlistaID
                          AND pjesmaID = @ogZadnjaID;
                    END;
                ELSE -- Ako pocetna nije zadnja + Pocetna pjesme je u sredini + ako nije target pjesma prva pjesma u playlisti
                    IF @pjesmaID = @ogZadnjaID -- Ako pocetna nije zadnja + Pocetna pjesme je u sredini + ako nije target pjesma prva pjesma u playlisti + ako je target zadnja pjesma u playlisti
                        BEGIN
                            -- Zelimo prebaciti zadnju pjesmu negdje u sredinu
                            SELECT 'in here 6';

                            SELECT @noviOgSljedID = @ogPrvaID,
                                   @novaSljedIDOdPjesmaID = @sljedbenikID,
                                   @novaPretIDOdPjesmaID = @pocetnaPjesmaID,
                                   @noviSljedID = @pjesmaID,
                                   @noviPretID = @pjesmaID
                        END;
                    ELSE -- Ako pocetna nije zadnja + Pocetna pjesme je u sredini + ako nije target pjesma prva pjesma u playlisti + ako nije target zadnja pjesma u playlisti
                        BEGIN
                            SELECT 'in here 7';

                            SELECT @noviSljedID = @pjesmaID,
                                   @noviPretID = @pjesmaID,
                                   @novaSljedIDOdPjesmaID = @sljedbenikID,
                                   @novaPretIDOdPjesmaID = @pocetnaPjesmaID,
                                   @noviOgSljedID = @ogSljedbenikID,
                                   @noviOgPretID = @ogPrethodnikID
                        END;
            END;

    --===========================================================================--
-- Promjene Pokazivaca

-- Promjena pokazivaca sljedece pjesme target pjesme, tj. pjesme cijoj mjenjamo poziciju -> pjesmaID.pokazivacNaSljedecuPjesmuID
    EXEC dbo.premjesti_pokazivac_na_sljedecu_pjesmu @playlistaID = @playlistaID, @naPjesmuID = @novaSljedIDOdPjesmaID,
         @gdjeJePjesmaID = @pjesmaID;
-- Promjena pokazivaca prethodne pjesme target pjesme, tj. pjesme cijoj mjenjamo poziciju -> pjesmaID.pokazivacNaPrethodnuPjesmuID
    EXEC dbo.premjesti_pokazivac_na_prethodnu_pjesmu @playlistaID = @playlistaID, @naPjesmuID = @novaPretIDOdPjesmaID,
         @gdjeJePjesmaID = @pjesmaID;
-- Promjena pokazivaca originalnog prethodnika pjesmeID, tj. prethodnik pjesme cija je pozicija promijenjena prije promjene -> ogPrethodnikID.pokazivacNaSljedecuPjesmuID
    EXEC dbo.premjesti_pokazivac_na_sljedecu_pjesmu @playlistaID = @playlistaID, @naPjesmuID = @noviOgSljedID,
         @gdjeJePjesmaID = @ogPrethodnikID;
-- Promjena pokazivaca originalnog sljedbenika pjesmeID, tj. prethodnik pjesme cija je pozicija promijenjena prije promjene -> ogSljedbenikID.pokazivacNaPrethodnuPjesmuID
    EXEC dbo.premjesti_pokazivac_na_prethodnu_pjesmu @playlistaID = @playlistaID, @naPjesmuID = @noviOgPretID,
         @gdjeJePjesmaID = @ogSljedbenikID;
-- Promjena pokazivaca novog prethodnika pjesmeID -> @pocetnaPjesmaID.pokazivacNaSljedecuPjesmuID
    EXEC dbo.premjesti_pokazivac_na_sljedecu_pjesmu @playlistaID = @playlistaID, @naPjesmuID = @noviSljedID,
         @gdjeJePjesmaID = @pocetnaPjesmaID;
-- Promjena pokazivaca novog sljedbenika pjesmeID -> @sljedbenikID.pokazivacNaPrethodnuPjesmuID
    EXEC dbo.premjesti_pokazivac_na_prethodnu_pjesmu @playlistaID = @playlistaID, @naPjesmuID = @noviPretID,
         @gdjeJePjesmaID = @sljedbenikID;

END;
go

