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

