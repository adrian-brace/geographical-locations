CREATE TABLE [dbo].[Cities]
(
	[Id] INT NOT NULL IDENTITY(1,1), 
    [Name] NVARCHAR(128) NOT NULL, 
    [SubRegion] NVARCHAR(128) NULL, 
    [CountryCode] VARCHAR(2) NOT NULL, 
    [TouristRating] TINYINT NULL, 
    [EstablishedOn] DATE NULL, 
    [EstimatedPopulation] INT NULL,
	CONSTRAINT PK_Cities_Id PRIMARY KEY CLUSTERED (Id)
)
