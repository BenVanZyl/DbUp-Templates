/*	### THIS IS A SAMPLE ### */

If Not Exists(Select 1 From sys.schemas Where name = 'Net47')
Begin
	Execute('Create Schema Net47')
End
GO