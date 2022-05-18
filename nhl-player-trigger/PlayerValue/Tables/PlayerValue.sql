CREATE TABLE [dbo].[PlayerValue]
(
	[id] INT NOT NULL,
	[name] VARCHAR(MAX) NOT NULL,
	[value] FLOAT NOT NULL
	[startYear] INT NOT NULL,
	CONSTRAINT PK_PlayerValue PRIMARY KEY (id, startYear),
)
