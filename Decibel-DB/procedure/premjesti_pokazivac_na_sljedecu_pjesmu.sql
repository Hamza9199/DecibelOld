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

