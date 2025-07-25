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

