/*	### THIS IS A SAMPLE ### */

If Not Exists(Select 1 From sys.schemas Where name = 'NetCore')
Begin
	Execute('Create Schema NetCore')
End
GO