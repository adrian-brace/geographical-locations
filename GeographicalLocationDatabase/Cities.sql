CREATE TABLE [dbo].[Cities]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Name] NVARCHAR(128) NOT NULL, 
    [SubRegion] NVARCHAR(128) NULL, 
    [Country] NVARCHAR(64) NOT NULL, 
    [TouristRating] TINYINT NULL, 
    [EstablishedOn] DATE NULL, 
    [EstimatedPopulation] INT NULL
)
