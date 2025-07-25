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

