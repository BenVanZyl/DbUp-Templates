/*	### THIS IS A SAMPLE ### */

If Not Exists(Select 1 From sys.schemas Where name = 'Net48')
Begin
	Execute('Create Schema Net48')
End
GO