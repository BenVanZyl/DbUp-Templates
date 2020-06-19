/*	### THIS IS A SAMPLE ### */

If Not Exists(Select 1 From sys.tables Where name = 'SampleTable47')
Begin
	Create Table Net47.SampleTable47
	(
		Id					bigint	Identity(1,1) NOT NULL,
		CreateUserId		bigint NOT NULL,
		CreateDateTime		dateTime NOT NULL,
		ModifyUserId		bigint NOT NULL,
		ModifyDateTime		dateTime NOT NULL,
		Name				varchar(1024) NOT NULL,
		CONSTRAINT [PK_Package] PRIMARY KEY CLUSTERED
		(
			[Id] ASC
		)
	)
End
GO

If Not Exists(select 1 from sys.indexes Where name = 'IX_SampleTable_Name')
Begin
	CREATE NONCLUSTERED INDEX [IX_SampleTable_Name] ON [Net47].[SampleTable47]
	(
		Name			ASC
	)
End
Go

