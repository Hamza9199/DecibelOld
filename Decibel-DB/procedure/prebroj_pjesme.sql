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