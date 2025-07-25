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

