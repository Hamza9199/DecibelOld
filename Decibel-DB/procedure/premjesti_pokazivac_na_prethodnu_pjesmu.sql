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

